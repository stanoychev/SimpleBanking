using RemoteClient.ServiceReference1;
using System;

namespace RemoteClient
{
    public class Program
    {
        public static void Main()
        {
            var service = new ServiceClient();
            Console.WriteLine(service.SayWellcome());
            while (true)
            {
                var command = Console.ReadLine();
                if (command.ToLower() == "q")
                {
                    service.EndSession();
                    break;
                }
                Console.WriteLine(service.ExecuteCommand(command));
            }
        }
    }
}