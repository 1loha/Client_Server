using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ChatClient.ServiceChat;

namespace ChatClient
{
    public partial class MainWindow : Window, IServiceChatCallback
    {
        static string fullPath = Path.GetFullPath("..\\..\\..\\ChatHost\\bin\\Debug\\HistoryChat.txt");
        bool isConnected = false;
        ServiceChatClient client;
        int ID;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConnDicon.Content = "Отключиться";
                isConnected = true;
                AddHistoryChat(fullPath);
            }
        }

        private void AddHistoryChat(string PathToHistory)
        {
            if (File.Exists(PathToHistory))
            {
                using (StreamReader sr = new StreamReader(PathToHistory))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lbChat.Items.Add(line);
                    }
                }
                lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
            }
        }

        private void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                tbUserName.IsEnabled = true;
                bConnDicon.Content = "Подключиться";
                isConnected = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
                DisconnectUser();
            else
                ConnectUser();
        }

        static public void SaveMsg(string msg)
        {
            using (StreamWriter sw = new StreamWriter(@"HistoryChat.txt", append: true))
            {
                fullPath = Path.GetFullPath(@"HistoryChat.txt");
                sw.WriteLine(msg);
            }
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tbMessage.Text == "cls")
                {
                    lbChat.Items.Clear();
                    tbMessage.Text = string.Empty;
                    return;
                }
                if (client!=null)
                {
                    client.SendMsg(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;
                }               
            }
        }

        private void watermarkedTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            watermarkedTxt.Visibility = Visibility.Collapsed;
            tbMessage.Visibility = Visibility.Visible;
            tbMessage.Focus();
            tbMessage.Visibility = Visibility.Visible;
        }

        private void tbMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbMessage.Text))
            {
                tbMessage.Visibility = Visibility.Collapsed;
                watermarkedTxt.Visibility = Visibility.Visible;
            }
        }

        private void watermarkedName_GotFocus(object sender, RoutedEventArgs e)
        {
            watermarkedName.Visibility = Visibility.Collapsed;
            tbUserName.Visibility = Visibility.Visible;
            tbUserName.Focus();
            tbUserName.Visibility = Visibility.Visible;
        }

        private void tbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUserName.Text))
            {
                tbUserName.Visibility = Visibility.Collapsed;
                watermarkedName.Visibility = Visibility.Visible;
            }
        }
    }
}
