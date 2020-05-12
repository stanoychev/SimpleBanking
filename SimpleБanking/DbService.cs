using System;
using System.Linq;

namespace SimpleBanking
{
    public interface IDbService
    {
        bool CustomerExists((string user, string pin) credentials);
        bool VerifyUserExist(string user);
        double? GetBalance((string user, string pin) credentials);
        bool Deposit((string user, string pin) credentials, double amount);
        bool Withdraw((string user, string pin) credentials, double amount);
        bool Transfter((string user, string pin) credentials, double amount, string user);
        string GetFormatedHistory((string user, string pin) credentials);
    }

    public class DbService : IDbService
    {
        readonly IBankDb bankDb;
        readonly Tools tools = new Tools();

        public DbService(IBankDb bankDb_) => bankDb = bankDb_;

        public bool Deposit((string user, string pin) credentials, double amount)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            customer.Account.Balance += amount;

            AddTransaction(amount, null, customer);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        public double? GetBalance((string user, string pin) credentials)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return null;

            return customer.Account.Balance;
        }

        public string GetFormatedHistory((string user, string pin) credentials)
        {
            return default;
        }

        public bool Transfter((string user, string pin) credentials, double amount, string user)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            var hashedUser = tools.HashString(user);
            var recipient = bankDb.Customers.ToList()//I am aware this is bad, but at least is secure and for small simple application should be relatively fine
                .FirstOrDefault(x => tools.CompareHashes(x.User, hashedUser));
            if (recipient == null)
                return false;

            customer.Account.Balance -= amount;
            recipient.Account.Balance += amount;

            AddTransaction(amount, customer, recipient);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 5;
        }

        public bool CustomerExists((string user, string pin) credentials) => GetCustomer(credentials) != null;

        public bool VerifyUserExist(string user)
        {
            var hashedUser = tools.HashString(user);
            return bankDb.Customers.ToList()//I am aware this is bad, but at least is secure and for small simple application should be relatively fine
                .FirstOrDefault(x => tools.CompareHashes(x.User, hashedUser)) != null;
        }

        public bool Withdraw((string user, string pin) credentials, double amount)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            customer.Account.Balance -= amount;

            AddTransaction(amount, null, customer);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        private void AddTransaction(double amount, Customer from, Customer to)
            => bankDb.Transactions.Add(new Transaction()
            {
                Amount = amount,
                TimeOfExecution = DateTime.Now,
                From = from,
                To = to
            });

        private Customer GetCustomer((string user, string pin) credentials)
        {
            var hashedUser = tools.HashString(credentials.user);
            var hashedPin = tools.HashString(credentials.pin);

            return bankDb.Customers.ToList()//I am aware this is bad, but at least is secure and for small simple application should be relatively fine
                .FirstOrDefault(x => tools.CompareHashes(x.User, hashedUser) && tools.CompareHashes(x.Pin, hashedPin));
        }
    }
}