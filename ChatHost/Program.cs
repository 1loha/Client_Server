using System;
using System.ServiceModel;

namespace ChatHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(wcf_chat.ServiceChat)))
            {
                Console.WriteLine("Запуск программы... \nЧтобы закрыть программу, введите слово 'end' \n\n");

                host.Open();
                string s;

                Console.WriteLine("Хост стартовал!");
                Console.WriteLine("Ожидаются подключения клиентов...");

                while ((s = Console.ReadLine()) != "end");
                Console.WriteLine("Хост закрылся!");
                host.Close();
            }
        }
    }
}
