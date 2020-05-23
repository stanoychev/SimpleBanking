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
        readonly ICustomerService customerService;
        string name;
        string cookie;

        public ATM(IDbService dbService_, ICustomerService customerService_)
        {
            dbService = dbService_;
            customerService = customerService_;
        }

        public string Result { get; private set; }

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

        private string Logout()
        {
            var isLoggedIn = customerService.IsLoggedIn(cookie);

            if (!isLoggedIn)
                return "Already logged out.";

            customerService.Logout(cookie);
            cookie = default;
            return "User logged out.";
        }

        private string GetBalance()
        {
            if (!customerService.IsLoggedIn(cookie))
                return "Not logged in.";

            if (customerService.IsExpired(cookie))
                return "Session expired";
            else
                customerService.UpdateExpiry(cookie);

            return string.Format("{0:0.00}", dbService.GetBalance(cookie) ?? 0d);
        }

        private string Withdraw(Dictionary<ArgumentType, string> inputParameters)
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

        private string Deposit(Dictionary<ArgumentType, string> inputParameters)
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

        private string Transfer(Dictionary<ArgumentType, string> inputParameters)
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

        private string GetHistory()
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