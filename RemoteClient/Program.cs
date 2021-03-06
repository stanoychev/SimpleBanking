﻿using RemoteClient.ServiceReference1;
using System;
using System.Collections.Generic;

namespace RemoteClient
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("This is RemoteClient");
            var manageExceptions = true;
            var isAuto = false;

            if (manageExceptions)
                try
                {
                    Run(isAuto);
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage(ex);
                }
            else
                Run(isAuto);
        }

        static void Run(bool isAuto)
        {
            Console.WriteLine("Bank started.\n" +
            "Please type command.\n" +
            "Type [h] for list of available commands or [q] to quit.");
            var service = new ServiceClient();

            if (isAuto)
            {
                var commands = GetAutoCommands();

                while (commands.Count != 0)
                    if (PrintAndExit(commands.Dequeue(), service))
                        return;
            }
            else
                while (true)
                    if (PrintAndExit(Console.ReadLine(), service))
                        return;


            service.EndSession();
            Console.WriteLine("Bank stopped. Pres ENTER to exit.");
            Console.ReadLine();
        }

        static Queue<string> GetAutoCommands()
        {
            var commands = new Queue<string>();
            commands.Enqueue("login bank 123");
            commands.Enqueue("get balance");
            commands.Enqueue("deposit 3");
            commands.Enqueue("get balance");
            commands.Enqueue("withdraw 1");
            commands.Enqueue("get balance");
            commands.Enqueue("transfer 1 to pesho");
            commands.Enqueue("get balance");
            commands.Enqueue("history");

            return commands;
        }

        static bool PrintAndExit(string input, ServiceClient service)
        {
            if (input.ToLower() == "q")
                return true;

            Console.WriteLine(service.ExecuteCommand(input));
            return false;
        }

        static void DisplayErrorMessage(Exception ex)
        {
            Console.Clear();
            Console.WriteLine("Application crashed with the following message:\n");
            Console.WriteLine(ex.Message);

            if (ex.InnerException != null)
                Console.WriteLine("Inner exception:\n" + ex.InnerException.Message);

            Console.WriteLine("Press ENTER to close the application.");
            Console.ReadLine();
        }
    }
}