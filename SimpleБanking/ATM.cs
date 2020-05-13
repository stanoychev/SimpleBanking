using System.Collections.Generic;

namespace SimpleBanking
{
    public interface IATM
    {
        void ExecuteCommand(IBankCommand command);
        string Result { get; }
    }

    public class ATM : IATM
    {
        readonly IDbService dbService;
        private (string user, string pass) credentials;

        public ATM(IDbService dbService_) => dbService = dbService_;

        public string Result { get; private set; }
        private bool IsLoggedIn => credentials.user != null;

        public void ExecuteCommand(IBankCommand command)
        {
            switch (command.CommandId)
            {
                case Command.Login:
                    Result = LogIn(command.InputParameters);
                    break;
                case Command.Logout:
                    Result = Logout();
                    break;
                case Command.GetBalance:
                    Result = GetBalance();
                    break;
                case Command.Withdraw:
                    Result = Withdraw(command.InputParameters);
                    break;
                case Command.Deposit:
                    Result = Deposit(command.InputParameters);
                    break;
                case Command.Transfer:
                    Result = Transfer(command.InputParameters);
                    break;
                case Command.History:
                    Result = GetHistory();
                    break;
            }
        }

        private string LogIn(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
                return $"[{credentials}] is currently logged in. Log out first.";
            else
            {
                var enteredCredentials = (inputParameters[ArgumentType.User], inputParameters[ArgumentType.Pin]);

                var isValid = dbService.CustomerExists(enteredCredentials);

                if (isValid)
                {
                    credentials = enteredCredentials;
                    return $"[{credentials.user}] successuly logged in.";
                }
                else
                    return "User or pin incorrect.";
            }
        }

        private string Logout()
        {
            if (IsLoggedIn)
            {
                credentials = default;
                return "User successfully logged out.";
            }
            else
                return "User already logged out.";
        }

        private string GetBalance()
            => IsLoggedIn ? string.Format("{0:0.00}", dbService.GetBalance(credentials) ?? 0d) : "Not logged in.";

        private string Withdraw(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
            {
                var amount = double.Parse(inputParameters[ArgumentType.Amount]);
                var balance = dbService.GetBalance(credentials);

                if (!balance.HasValue)
                    return $"[System error] User [{credentials.user}] has changed credentials. Logout and login again.";
                else if (amount <= 0)
                    return "Withdraw amount must be positive value.";
                else if (balance > amount)
                    return dbService.Withdraw(credentials, amount) ? string.Format("Withdrawn {0:0.00}", amount) : "[System error.] Witdraw failed.";
                else
                    return "Insufficient funds.";
            }
            else
                return "Not logged in.";
        }

        private string Deposit(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
            {
                var amount = double.Parse(inputParameters[ArgumentType.Amount]);

                return amount <= 0 ? "Deposit amount must be positive value." : dbService.Deposit(credentials, amount) ? "Deposit successful." : "[System error.] Deposit failed.";
            }
            else
                return "Not logged in.";
        }

        private string Transfer(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
            {
                var amount = double.Parse(inputParameters[ArgumentType.Amount]);
                var recipient = inputParameters[ArgumentType.User];
                var userExist = dbService.CustomerExists(recipient);
                var balance = dbService.GetBalance(credentials);

                if (!balance.HasValue)
                    return $"[System error] User [{credentials.user}] has changed credentials. Logout and login again.";
                else if (amount <= 0)
                    return "Deposit amount must be positive value.";
                else if (!userExist)
                    return $"Recipient [{recipient}] not found.";
                else if (string.Equals(recipient, credentials.user))
                    return $"Can not transfer to self.";
                else if (balance < amount)
                    return $"[{credentials.user}] has insufficient funds.";
                else
                    return dbService.Transfter(credentials, amount, recipient) ? "Transfter successful." : "[System error.] Transfter failed.";
            }
            else
                return "Not logged in.";
        }

        private string GetHistory() => IsLoggedIn ? dbService.GetFormatedHistory(credentials) : "Not logged in.";
    }
}