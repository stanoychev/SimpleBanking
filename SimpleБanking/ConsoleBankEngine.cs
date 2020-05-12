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
            "[q] (to quit)";

        const string invalidCommand = 
            "Invalid input.\n" +
            "Type [h] for list of available commands or [q] to quit.";

        const string wellcome =
            "Bank started.\n" +
            "Please type command.\n" +
            "Type [h] for list of available commands or [q] to quit.";
        #endregion
        readonly ICommandParser commandParser;
        readonly IATM atm;

        public ConsoleBankEngine(ICommandParser commandParser_, IATM atm_)
        {
            commandParser = commandParser_;
            atm = atm_;
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
                    continue;
                }
                else if (command.CommandId == Command.InvalidCommand)
                {
                    Console.WriteLine(invalidCommand);
                    continue;
                }

                atm.ExecuteCommand(command);
                Console.WriteLine(atm.Result);
            }
        }
    }
}