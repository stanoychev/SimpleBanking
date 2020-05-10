using System;
using System.Collections.Generic;

namespace SimpleBanking
{
    public interface IBankManager
    {
        void ExecuteCommand(IBankCommand command);
        string Result { get; }
    }

    public class BankManager : IBankManager
    {
        readonly IDbService dbService;
        private string loggedUser;
        private int? accountId;

        public BankManager(IDbService dbService_) => dbService = dbService_;

        public string Result { get; private set; }
        private bool IsLoggedIn => loggedUser != null;
        
        public void ExecuteCommand(IBankCommand command)
        {
            switch (command.CommandId)
            {
                case Command.Login:
                    Result = TryLogIn(command.InputParameters);
                    break;
                case Command.Logout:
                    Result = TryLogout();
                    break;
                case Command.GetBalance:
                    Result = TryGetBalance();
                    break;
                case Command.Withdraw:
                    Result = TryWithdraw(command.InputParameters);
                    break;
                case Command.Deposit:
                    Result = TryDeposit(command.InputParameters);
                    break;
                case Command.Transfer:
                    Result = TryTransfer(command.InputParameters);
                    break;
                case Command.History:
                    Result = TryGetHistory();
                    break;
            }
        }
        
        private string TryLogIn(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
                return $"[{loggedUser}] is currently logged in. Log out first.";
            else
            {
                var user = inputParameters[ArgumentType.User];
                var pin = inputParameters[ArgumentType.Pin];

                accountId = dbService.GetAccount(user, pin);

                if (accountId == null)
                    return "User or pin incorrect.";
                else
                {
                    loggedUser = user;
                    return $"[{loggedUser}] successuly logged in.";
                }
            }
        }
    
        private string TryLogout()
        {
            if (IsLoggedIn)
            {
                loggedUser = null;
                accountId = null;
                return "User successfully logged out.";
            }
            else
                return "User already logged out.";
        }

        private string TryGetBalance()
        {
            if (IsLoggedIn)
                return string.Format("{0:0.00}", dbService.GetBalance(accountId.Value));
            else
                return "Not logged in.";
        }

        private string TryWithdraw(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
            {
                var amount = double.Parse(inputParameters[ArgumentType.Amount]);
                var balance = dbService.GetBalance(accountId.Value);
                if (amount <= 0)
                    return "Withdraw amount must be positive value.";
                else if (balance > amount)
                {
                    dbService.Withdraw(accountId.Value, amount);
                    return string.Format("Withdrawn {0:0.00}", amount);
                }
                else
                    return "Insufficient funds.";
            }
            else
                return "Not logged in.";
        }

        private string TryDeposit(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
            {
                throw new NotImplementedException();
            }
            else
                return "Not logged in.";
        }

        private string TryTransfer(Dictionary<ArgumentType, string> inputParameters)
        {
            if (IsLoggedIn)
            {
                throw new NotImplementedException();
            }
            else
                return "Not logged in.";
        }

        private string TryGetHistory()
        {
            if (IsLoggedIn)
            {
                throw new NotImplementedException();
            }
            else
                return "Not logged in.";
        }
    }
}