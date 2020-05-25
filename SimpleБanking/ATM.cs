using System.Collections.Generic;
using System;

namespace SimpleBanking
{
    public interface IATM
    {
        string ExecuteCommand(string input);
    }

    public class ATM : IATM
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
        readonly IDbService dbService;
        readonly ICustomerService customerService;
        string name;
        string cookie;

        public ATM(IDbService dbService_, ICustomerService customerService_, ICommandParser commandParser_)
        {
            dbService = dbService_;
            customerService = customerService_;
            commandParser = commandParser_;
        }

        public string ExecuteCommand(string input)
        {
            var command = commandParser.ParseCommand(input.ToLower());

            if (command.CommandId == Command.Quit)
                return null;
            else if (command.CommandId == Command.Help)
                return help;
            else if (command.CommandId == Command.InvalidCommand)
                return invalidCommand;

            return ExecuteParsedCommand(command);
        }

        string ExecuteParsedCommand(IBankCommand command)
        {
            switch (command.CommandId)
            {
                case Command.Login: return LogIn(command.InputParameters);
                case Command.Logout: return Logout();
                case Command.GetBalance: return GetBalance();
                case Command.Withdraw: return Withdraw(command.InputParameters);
                case Command.Deposit: return Deposit(command.InputParameters);
                case Command.Transfer: return Transfer(command.InputParameters);
                case Command.History: return GetHistory();
                default: throw new ArgumentOutOfRangeException("Given command is not implemented.");
            }
        }

        string LogIn(Dictionary<ArgumentType, string> inputParameters)
        {
            if (customerService.IsLoggedIn(cookie) && !customerService.IsExpired(cookie))
                return "Already logged in. Log out first";

            var credentials = (inputParameters[ArgumentType.User], inputParameters[ArgumentType.Pin]);
            var info = customerService.Login(credentials);

            if (info.cookie == default)
                return "Wrong credentials.";

            cookie = info.cookie;
            name = info.name;
            return $"[{name}] logged in.";
        }

        string Logout()
        {
            var isLoggedIn = customerService.IsLoggedIn(cookie);

            if (!isLoggedIn)
                return "Already logged out.";

            customerService.Logout(cookie);
            cookie = default;
            return "User logged out.";
        }

        string GetBalance()
        {
            if (!customerService.IsLoggedIn(cookie))
                return "Not logged in.";

            if (customerService.IsExpired(cookie))
                return "Session expired";
            else
                customerService.UpdateExpiry(cookie);

            return string.Format("{0:0.00}", dbService.GetBalance(cookie) ?? 0d);
        }

        string Withdraw(Dictionary<ArgumentType, string> inputParameters)
        {
            if (!customerService.IsLoggedIn(cookie))
                return "Not logged in.";

            if (customerService.IsExpired(cookie))
                return "Session expired";
            else
                customerService.UpdateExpiry(cookie);

            var amount = double.Parse(inputParameters[ArgumentType.Amount]);
            var balance = dbService.GetBalance(cookie);

            if (!balance.HasValue)
                return $"[System error] Try logout and login again.";

            if (amount <= 0)
                return "Withdraw amount must be positive value.";

            if (balance < amount)
                return "Insufficient funds.";

            return dbService.Withdraw(cookie, amount) ? string.Format("Withdrawn {0:0.00}", amount) : "[System error.] Witdraw failed.";
        }

        string Deposit(Dictionary<ArgumentType, string> inputParameters)
        {
            if (!customerService.IsLoggedIn(cookie))
                return "Not logged in.";

            if (customerService.IsExpired(cookie))
                return "Session expired";
            else
                customerService.UpdateExpiry(cookie);

            var amount = double.Parse(inputParameters[ArgumentType.Amount]);
            if (amount <= 0)
                return "Deposit amount must be positive value.";

            return dbService.Deposit(cookie, amount) ? "Deposit successful." : "[System error.] Deposit failed.";
        }

        string Transfer(Dictionary<ArgumentType, string> inputParameters)
        {
            if (!customerService.IsLoggedIn(cookie))
                return "Not logged in.";

            if (customerService.IsExpired(cookie))
                return "Session expired";
            else
                customerService.UpdateExpiry(cookie);

            var amount = double.Parse(inputParameters[ArgumentType.Amount]);
            var recipient = inputParameters[ArgumentType.User];
            var userExist = customerService.CustomerExists(recipient);
            var balance = dbService.GetBalance(cookie);

            if (!balance.HasValue)
                return $"[System error] User [{name}] has changed credentials. Logout and login again.";

            if (amount <= 0)
                return "Deposit amount must be positive value.";

            if (!userExist)
                return $"Recipient [{recipient}] not found.";

            if (string.Equals(recipient, name))
                return $"Can not transfer to self.";

            if (balance < amount)
                return $"[{name}] has insufficient funds.";

            return dbService.Transfter(cookie, amount, recipient) ? "Transfter successful." : "[System error.] Transfter failed.";
        }

        string GetHistory()
        {
            if (!customerService.IsLoggedIn(cookie))
                return "Not logged in.";

            if (customerService.IsExpired(cookie))
                return "Session expired";
            else
                customerService.UpdateExpiry(cookie);

            return dbService.GetFormatedHistory(cookie);
        }
    }
}