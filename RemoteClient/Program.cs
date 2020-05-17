using RemoteClient.ServiceReference1;
using System;

namespace RemoteClient
{
    public class Program
    {
        public static void Main()
        {
            var service = new ServiceClient();
            while (true)
            {
                var command = Console.ReadLine();
                Console.WriteLine(service.ExecuteCommand(command));
            }
        }
    }
}