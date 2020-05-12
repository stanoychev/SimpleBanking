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

            AddTransaction(amount, null, customer);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        public double? GetBalance((string user, string pin) credentials)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return null;

            throw new Exception();
            return default;// customer.Transactions.Where(x => ).Balance;
        }

        public string GetFormatedHistory((string user, string pin) credentials)
        {
            throw new Exception();
            return default;
        }

        public bool Transfter((string user, string pin) credentials, double amount, string user)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            var recipient = GetCustomer(user);
            if (recipient == null)
                return false;

            AddTransaction(amount, customer, recipient);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 5;
        }

        public bool CustomerExists((string user, string pin) credentials) => GetCustomer(credentials) != null;

        public bool VerifyUserExist(string user) => GetCustomer(user) != null;

        public bool Withdraw((string user, string pin) credentials, double amount)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            AddTransaction(amount, customer, null);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        private void AddTransaction(double amount, Customer from, Customer to)
        { }
        //=> bankDb.Transactions.Add(new Transaction()
        //{
        //    Amount = amount,
        //    TimeOfExecution = DateTime.Now,
        //    From = from,
        //    To = to
        //});

        private Customer GetCustomer((string user, string pin) credentials)
        {
            return default;
            //var hashedUser = tools.HashString(credentials.user);
            //var hashedPin = tools.HashString(credentials.pin);

            //return bankDb.Customers
            //    .FirstOrDefault(x => string.Equals(x.User, hashedUser) && string.Equals(x.Pin, hashedPin));
        }

        private Customer GetCustomer(string user)
        {
            return default;
            //var hashedUser = tools.HashString(user);

            //return bankDb.Customers
            //    .FirstOrDefault(x => string.Equals(x.User, hashedUser));
        }
    }
}