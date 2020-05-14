using System.Collections.Generic;

namespace SimpleBanking
{
    public enum Command
    {
        Login,
        Logout,
        GetBalance,
        Withdraw,
        Deposit,
        Transfer,
        History,
        Help,
        InvalidCommand,
        Quit
    }

    public enum ArgumentType
    {
        User,
        Pin,
        Amount
    }

    public interface IBankCommand
    {
        Command CommandId { get; }
        Dictionary<ArgumentType, string> InputParameters { get; }
    }

    public class BankCommand : IBankCommand
    {
        public BankCommand(Command commandId, Dictionary<ArgumentType, string> inputParameters = null)
        {
            CommandId = commandId;
            InputParameters = inputParameters;
        }

        public Command CommandId { get; }
        public Dictionary<ArgumentType, string> InputParameters { get; }
    }
}