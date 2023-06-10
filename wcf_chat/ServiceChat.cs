using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ChatClient;

namespace wcf_chat
{
    // указывает, что служба будет работать в режиме единственного контекста экземпляра.
    // значит будет создан лишь один экземпляр службы и он будет обрабатывать все запросы от клиентов
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {        
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser() {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;

            SendMsg(": " + user.Name + " подключился к чату!", 0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user!=null)
            {
                users.Remove(user);
                SendMsg(": " + user.Name + " покинул чат!", 0);
            }
        }

        public void SendMsg(string msg, int id)
        {
            string answer = "";
            
            foreach (var item in users)
            {
                answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": (" + user.Name + ") ";
                }

                if (msg.Substring(0, 1) == "/")
                    msg = SecretMsg(msg);
                
                answer += msg;
                
                item.operationContext.GetCallbackChannel<IServerChatCallback>().MsgCallback(answer);
            }
            MainWindow.SaveMsg(answer);
        }
        public string SecretMsg(string msg)
        {
            msg.Substring(1);
            msg = msg.ToUpper();

            
            string morseMessage = "";

            var myDict = new Dictionary<char, string>
            {
                {'A', ".-"},
                {'B', "-..."},
                {'C', "-.-."},
                {'D', "-.."},
                {'E', "."},
                {'F', "..-."},
                {'G', "--."},
                {'H', "...."},
                {'I', ".."},
                {'J', ".---"},
                {'K', "-.-"},
                {'L', ".-.."},
                {'M', "--"},
                {'N', "-."},
                {'O', "---"},
                {'P', ".--."},
                {'Q', "--.-"},
                {'R', ".-."},
                {'S', "..."},
                {'T', "-"},
                {'U', "..-"},
                {'V', "...-"},
                {'W', ".--"},
                {'X', "-..-"},
                {'Y', "-.--"},
                {'Z', "--.."},

                {'А', ".-"},
                {'Б', "-..."},
                {'В', ".--"},
                {'Г', "--."},
                {'Д', "-.."},
                {'Е', "."},
                {'Ж', "...-"},
                {'З', "--.."},
                {'И', ".."},
                {'Й', ".---"},
                {'К', "-.-"},
                {'Л', ".-.."},
                {'М', "--"},
                {'Н', "-."},
                {'О', "---"},
                {'П', ".--."},
                {'Р', ".-."},
                {'С', "..."},
                {'Т', "-"},
                {'У', "..-"},
                {'Ф', "..-."},
                {'Х', "...."},
                {'Ц', "-.-."},
                {'Ч', "---."},
                {'Ш', "----"},
                {'Щ', "--.-"},
                {'Ъ', "--.--"},
                {'Ы', "-.--"},
                {'Ь', "-..-"},
                {'Э', "..-.."},
                {'Ю', "..--"},
                {'Я', ".-.-"},

                {'0', "-----"},
                {'1', ".----"},
                {'2', "..---"},
                {'3', "...--"},
                {'4', "....-"},
                {'5', "....."},
                {'6', "-...."},
                {'7', "--..."},
                {'8', "---.."},
                {'9', "----."}
            };
            
            for (int i = 0; i < msg.Length; i++)
            {
                char c = msg[i];
                if (myDict.ContainsKey(c))
                    morseMessage += myDict[c];
                else
                    morseMessage += ' ';
            }

            return morseMessage;
        }
    }
}
