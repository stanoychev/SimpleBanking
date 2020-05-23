namespace SimpleBanking
{
    public interface IBankEngine
    {
        string ExecuteCommand_(string command);
    }

    public class BankEngine : IBankEngine
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
        #endregion
        readonly ICommandParser commandParser;
        readonly IATM atm;

        public BankEngine(ICommandParser commandParser_, IATM atm_)
        {
            commandParser = commandParser_;
            atm = atm_;
        }

        public string ExecuteCommand_(string command_)
        {
            var command = command_.ToLower();
            var result = ExecuteCommand(command);
            if (result == null)
                return null;

            return result;
        }

        string ExecuteCommand(string input)
        {
            var command = commandParser.ParseCommand(input);

            if (command.CommandId == Command.Quit)
                return null;
            else if (command.CommandId == Command.Help)
                return help;
            else if (command.CommandId == Command.InvalidCommand)
                return invalidCommand;

            atm.ExecuteCommand(command);
            return atm.Result;
        }
    }
}