using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleBanking
{
    public interface ICommandParser
    {
        IBankCommand ParseCommand(string command);
    }

    public class CommandParser : ICommandParser
    {
        public IBankCommand ParseCommand(string command)
        {
            if (command == null)
                return new BankCommand(Command.InvalidCommand);

            var commandParameters = command.Split();
            switch (commandParameters[0])
            {
                case "login": return ValidateLogin(commandParameters);
                case "logout": return ValidateLogout(commandParameters);
                case "get": return ValidateGetBalance(commandParameters);
                case "withdraw": return ValidateWithdraw(commandParameters);
                case "deposit": return ValidateDeposit(commandParameters);
                case "transfer": return ValidateTransfer(commandParameters);
                case "history": return ValidateHistory(commandParameters);
                case "h": return new BankCommand(Command.Help);
                case "q": return new BankCommand(Command.Quit);
                default: return new BankCommand(Command.InvalidCommand);
            }
        }

        //assumes only latin upper and lower case letters
        private bool UserIsValid(string user) => Regex.IsMatch(user, @"^[a-z]+$", RegexOptions.IgnoreCase);
        //assumes only digits
        private bool PinIsValid(string pin) => Regex.IsMatch(pin, @"^[0-9]+$");
        //amount <= 0 will be validated in the command itself
        private bool AmountIsValid(string amount) => 
            double.TryParse(amount, out var a) && 
            (amount.Contains(".") || amount.Contains(",")) ?
                amount.Split(new char[] { ',', '.'})[1].Length <= 2 :
                true;

        private IBankCommand ValidateLogin(string[] commandParameters)
        {
            if (commandParameters.Length != 3)
                return new BankCommand(Command.InvalidCommand);

            if(!UserIsValid(commandParameters[1]))
                return new BankCommand(Command.InvalidCommand);

            if(!PinIsValid(commandParameters[2]))
                return new BankCommand(Command.InvalidCommand);

            var commandArguments = new Dictionary<ArgumentType, string>() 
            {
                { ArgumentType.User, commandParameters[1] },
                { ArgumentType.Pin, commandParameters[2] }
            };

            return new BankCommand(Command.Login, commandArguments);
        }
    
        private IBankCommand ValidateLogout(string[] commandParameters)
        {
            if (commandParameters.Length != 1)
                return new BankCommand(Command.InvalidCommand);

            return new BankCommand(Command.Logout);
        }

        private IBankCommand ValidateGetBalance(string[] commandParameters)
        {
            if (commandParameters.Length == 2 && string.Equals(commandParameters[1], "balance"))
                return new BankCommand(Command.GetBalance);
            else
                return new BankCommand(Command.InvalidCommand);
        }
        
        private IBankCommand ValidateWithdraw(string[] commandParameters)
        {
            if (commandParameters.Length != 2)
                return new BankCommand(Command.InvalidCommand);

            if (!AmountIsValid(commandParameters[1]))
                return new BankCommand(Command.InvalidCommand);

            var commandArguments = new Dictionary<ArgumentType, string>()
            {
                { ArgumentType.Amount, commandParameters[1] }
            };

            return new BankCommand(Command.Withdraw, commandArguments);
        }
        
        private IBankCommand ValidateDeposit(string[] commandParameters)
        {
            if (commandParameters.Length != 2)
                return new BankCommand(Command.InvalidCommand);

            if (!AmountIsValid(commandParameters[1]))
                return new BankCommand(Command.InvalidCommand);

            var commandArguments = new Dictionary<ArgumentType, string>()
            {
                { ArgumentType.Amount, commandParameters[1] }
            };

            return new BankCommand(Command.Deposit, commandArguments);
        }
        
        private IBankCommand ValidateTransfer(string[] commandParameters)
        {
            if (commandParameters.Length != 4)
                return new BankCommand(Command.InvalidCommand);

            if (!AmountIsValid(commandParameters[1]))
                return new BankCommand(Command.InvalidCommand);

            if (!string.Equals(commandParameters[2], "to"))
                return new BankCommand(Command.InvalidCommand);

            if (!UserIsValid(commandParameters[3]))
                return new BankCommand(Command.InvalidCommand);

            var commandArguments = new Dictionary<ArgumentType, string>()
            {
                { ArgumentType.Amount, commandParameters[1] },
                { ArgumentType.User, commandParameters[3] }
            };

            return new BankCommand(Command.Transfer, commandArguments);
        }
        
        private IBankCommand ValidateHistory(string[] commandParameters)
        {
            if (commandParameters.Length != 1)
                return new BankCommand(Command.InvalidCommand);

            return new BankCommand(Command.History);
        }
    }
}