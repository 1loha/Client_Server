using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    [ServiceContract(CallbackContract = typeof(IServerChatCallback))] // чтобы сервис знал, что есть интерфейс callback
    public interface IServiceChat // описание того, что может делать наш чат
    {
        [OperationContract] // для каждого метода, который взаимодействует с сервисом со стороны клиента
                            // будет виден со стороны клиента
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);
        
        [OperationContract(IsOneWay = true)]
        void SendMsg(string msg, int id);
    }

    public interface IServerChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
    }
}
