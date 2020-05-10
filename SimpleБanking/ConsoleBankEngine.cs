using System;

namespace SimpleBanking
{
    public interface IConsoleBankEngine
    {
        void Run();
    }

    public class ConsoleBankEngine : IConsoleBankEngine
    {
        #region Messages
        const string help =
            "Available commands:\n" +
            "[login] [user] [pin]\n" +
            "[logout]\n" +
            "[get balance]\n" +
            "[withdraw] [amount]\n" +
            "[deposit] [amount]\n" +
            "[transfer] [amount] to [user]\n" +
            "[history]\n" +
            "[q] (to quit)\n";

        const string invalidCommand = 
            "Invalid input.\n" +
            "Type [h] for list of available commands or [q] to quit.\n";

        const string wellcome =
            "Bank started.\n" +
            "Please type command.\n" +
            "Type [h] for list of available commands or [q] to quit.\n";
        #endregion
        readonly ICommandParser commandParser;
        readonly IBankManager bankManager;

        public ConsoleBankEngine(ICommandParser commandParser_, IBankManager bankManager_)
        {
            commandParser = commandParser_;
            bankManager = bankManager_;
        }

        public void Run()
        {
            Console.WriteLine(wellcome);

            while (true)
            {
                var input = Console.ReadLine().ToLower();

                var command = commandParser.ParseCommand(input);

                if (command.CommandId == Command.Quit)
                    return;
                else if (command.CommandId == Command.Help)
                {
                    Console.WriteLine(help);
                    break;
                }
                else if (command.CommandId == Command.Help)
                {
                    Console.WriteLine(invalidCommand);
                    break;
                }

                bankManager.ExecuteCommand(command);
                Console.WriteLine(bankManager.Result);
            }
        }
    }
}