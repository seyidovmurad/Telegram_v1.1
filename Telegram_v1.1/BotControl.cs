using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Telegram_v1._1
{
    class BotControl
    {
        List<Bot> bots;

        public bool isItReply { get; private set; } = true;
        public bool isResponse { get; private set; } = true;
        string lastMessage;

        public BotControl()
        {
            if (!File.Exists("bot.json"))
            {
                File.WriteAllText("bot.json", "");
            }

            bots = new List<Bot>();

            var jsonStr = File.ReadAllText("bot.json");

            if (jsonStr.Length > 0)
                bots = JsonConvert.DeserializeObject<List<Bot>>(jsonStr);
        }

        private void UpdateJson()
        {
            var jsonFile = JsonConvert.SerializeObject(this.bots, Formatting.Indented);
            File.WriteAllText("bot.json", jsonFile);
        }

        public string ReplyToPerson(string text)
        {
            text = text.ToLower();
            isResponse = true;
            if (text == "ctn")
            {
                isItReply = true;
                return "OK";
            }
            if (isItReply)
            {
                if (text.Contains( "saat necedi"))
                    return DateTime.Now.ToShortTimeString();
                if (text.Contains("+") || text.Contains("+"))
                    return "Ordan baxanda kalkulyatora oxsayiram";
                if (bots.Count == 0)
                {
                    lastMessage = text;
                    isItReply = false;
                }
                foreach (var bot in bots)
                {
                    if (bot.Message == text) 
                    {
                        isItReply = true;
                        if (bot.BotReply == "no reply")
                            isResponse = false;
                        return bot.BotReply;
                    }
                    else
                    {
                        lastMessage = text;
                        isItReply = false;
                    }
                }
                if(!isItReply)
                    return "i didn't understand your message please help me improve myself. how do you think i should reply to this message";
            }
            else
            {
                Bot bot = new Bot();
                bot.BotReply = text;
                bot.Message = lastMessage;
                bots.Add(bot);
                isItReply = true;
                UpdateJson();
                return "thank you for help:)";
            }
            return "Ok";
        }

    }
}
