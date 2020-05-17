namespace SimpleBanking
{
    public interface IRemoteEngine
    {
        string Status { get; }
        void SendCommand(string command);
    }

    public class RemoteEngine : IRemoteEngine
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

        public RemoteEngine(ICommandParser commandParser_, IATM atm_)
        {
            throw new System.Exception("todo fix concurrency");
            commandParser = commandParser_;
            atm = atm_;
        }
        
        public string Status { get; private set; }

        public void SendCommand(string input)
        {
            Status = string.Empty;
            var command = commandParser.ParseCommand(input);

            if (command.CommandId == Command.Quit)
                return;
            else if (command.CommandId == Command.Help)
            {
                Status = help;
                return;
            }
            else if (command.CommandId == Command.InvalidCommand)
            {
                Status = invalidCommand;
                return;
            }

            atm.ExecuteCommand(command);
            Status = atm.Result;
        }
    }
}