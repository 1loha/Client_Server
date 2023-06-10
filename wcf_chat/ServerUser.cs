using System.ServiceModel;

namespace wcf_chat
{
    public class ServerUser
    {
        // ID пользователя
        public int ID { get; set; }

        // Имя пользователя
        public string Name { get; set; }

        // Предоставляет доступ к контексту выполнения метода службы
        public OperationContext operationContext { get; set; }
    }
}
