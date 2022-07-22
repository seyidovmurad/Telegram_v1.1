using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_v1._1
{
    [AddINotifyPropertyChangedInterface]
    public class User
    {
        public string Username { get; set; }

        public string ImgSource { get; set; }

        public DateTime Time { get; private set; }
        public string LastMessage { get; set; }

        public string LastMessageTime { get; set; }

        public ObservableCollection<Chat> Chats { get; set; }

        public void LastText()
        {
            if (Chats.Last().Text.Length > 100)
                LastMessage = Chats.Last().Text.Substring(0, 100);
            else
                LastMessage = Chats.Last().Text;
            LastMessageTime = Chats.Last().Time;
            Time = DateTime.Now;
        }

        public User(string username, string imgSource)
        {
            Username = username;
            ImgSource = imgSource;
            Chats = new ObservableCollection<Chat>();
        }
    }
}
