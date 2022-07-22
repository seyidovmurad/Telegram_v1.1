using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Telegram_v1._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        //Colors
        SolidColorBrush TextColor = new SolidColorBrush(Color.FromRgb(234, 26, 156));
        SolidColorBrush MyMsgColor = new SolidColorBrush(Color.FromRgb(20, 13, 53));
        SolidColorBrush OtherMsgColor = new SolidColorBrush(Color.FromRgb(210, 201, 222));

        BotControl BotControl = new BotControl();

        public string Username { get; set; }
        User user;
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Chat> Chats { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ObservableCollection<Chat> chat = new ObservableCollection<Chat>
            {
                new Chat("salam","Left",OtherMsgColor,TextColor),
                new Chat("salam","Right",MyMsgColor,TextColor),
                new Chat("Botla dansimaq ucun Anex Bota yaz","Left",OtherMsgColor,TextColor),
            };

            Users = new ObservableCollection<User>
            {
                new User("Eva Summer", "pack://application:,,,/Images/user.png"),
                new User("Mike", "pack://application:,,,/Images/user.png"),
                new User("Alexandra Smith", "pack://application:,,,/Images/user.png"),
                new User("Anex Bot", "pack://application:,,,/Images/bot.png")
            };

            Users[0].Chats = chat;
            Users[0].LastText();

            Users[1].Chats = chat;
            Users[1].LastText();

            Users[2].Chats = chat;
            Users[2].LastText();

            Users[3].LastMessage = "this is bot";
            Users[3].LastMessageTime = DateTime.Now.ToShortTimeString();
            DataContext = this;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            user = UserListView.SelectedItem as User;
            Chats = (UserListView.SelectedItem as User).Chats;
            Username = user.Username;
            MsgBox.Visibility = Visibility.Visible;
            UserInfo.Visibility = Visibility.Visible;
        }


        //Message Box MouseEnter
        private void TextBox_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb) {
                if (tb.Text == "Write a message...")
                {
                    tb.Text = "";
                    tb.Foreground = TextColor;
                    ChatScroll.ScrollToEnd();
                }
            }
        }

        private void TextBox_MouseLeave(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox tb)
            {
                if (string.IsNullOrEmpty(tb.Text) && string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Foreground = Brushes.LightGray;
                    tb.Text = "Write a message...";
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (e.Key == Key.Enter && !string.IsNullOrEmpty(tb.Text) && !string.IsNullOrWhiteSpace(tb.Text))
                {
                    Chats.Add(new Chat(tb.Text, "Right", MyMsgColor, TextColor));
                    string botText = tb.Text;
                    tb.Text = "";
                    if (Username == "Anex Bot")
                    {
                        botText = BotControl.ReplyToPerson(botText);
                        if (BotControl.isResponse)
                        {
                            Chats.Add(new Chat(botText, "Left", OtherMsgColor, TextColor));
                            if (!BotControl.isItReply)
                                Chats.Add(new Chat("if you don't want to write ctn", "Left", OtherMsgColor, TextColor));
                            ChatScroll.ScrollToEnd();
                        }
                    }
                    Users.ToList().Find(u => u.Username == Username).LastText();
                    Users = new ObservableCollection<User>(Users.OrderByDescending(u => u.Time));
                }
            }
        }
    }
}
