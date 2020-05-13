using System;
using System.Linq;

namespace SimpleBanking
{
    public interface IDbService
    {
        bool CustomerExists((string user, string pin) credentials);
        bool CustomerExists(string user);
        double? GetBalance((string user, string pin) credentials);
        bool Deposit((string user, string pin) credentials, double amount);
        bool Withdraw((string user, string pin) credentials, double amount);
        bool Transfter((string user, string pin) credentials, double amount, string user);
        string GetFormatedHistory((string user, string pin) credentials);
    }

    //public class DbService : IDbService
    //{
    //    public bool CustomerExists((string user, string pin) credentials)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool CustomerExists(string user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Deposit((string user, string pin) credentials, double amount)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public double? GetBalance((string user, string pin) credentials)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string GetFormatedHistory((string user, string pin) credentials)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Transfter((string user, string pin) credentials, double amount, string user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Withdraw((string user, string pin) credentials, double amount)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

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

            AddTransaction(customer, amount, null, customer);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        public double? GetBalance((string user, string pin) credentials)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return null;
            throw new Exception();
            return customer.Transactions
                .Select(transaction => transaction.From == null ? transaction.Amount : -transaction.Amount).Sum();
        }

        public string GetFormatedHistory((string user, string pin) credentials)
        {
            throw new Exception();
            var customer = GetCustomer(credentials);
            if (customer == null)
                return $"[{credentials.user}] not found.";

            return $"History for [{credentials.user}]:" +
                string.Join("\n", customer.Transactions
                .OrderByDescending(x => x.Date)
                .ToList()
                .Select(transaction =>
                        "[Date" + transaction.Date.Value.ToString("hh:mm:ss dd/MM/yyyy") + "]" +
                        transaction.From == null ?
                        "[Deposit]" : transaction.To == null ?
                        "[Withdraw]" :
                        $"[Transaction from [{transaction.From.Name}] to [{transaction.To.Name}]]" +
                        $"[Amount {string.Format("{0:0.00}", transaction.Amount)}]"));
        }

        public bool Transfter((string user, string pin) credentials, double amount, string user)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            var recipient = GetCustomer(user);
            if (recipient == null)
                return false;

            AddTransaction(customer, amount, customer, recipient);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 4;
        }

        public bool CustomerExists((string user, string pin) credentials) => GetCustomer(credentials) != null;

        public bool CustomerExists(string user) => GetCustomer(user) != null;

        public bool Withdraw((string user, string pin) credentials, double amount)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return false;

            AddTransaction(customer, amount, customer, null);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        private void AddTransaction(Customer customer, double amount, Customer from, Customer to)
            => bankDb.Customers.First(x => x.CustomerId == customer.CustomerId)
            .Transactions.Add(new Transaction()
            {
                Amount = amount,
                Date = DateTime.Now,
                From = from,
                To = to
            });

        private Customer GetCustomer((string user, string pin) credentials)
        {
            var hashedUser = tools.HashString(credentials.user);
            var hashedPin = tools.HashString(credentials.pin);

            var customer = bankDb.Customers
                .FirstOrDefault(x => string.Equals(x.User, hashedUser));

            return string.Equals(customer.Pin, hashedPin) ? customer : null;
        }

        private Customer GetCustomer(string user)
        {
            var hashedUser = tools.HashString(user);

            return bankDb.Customers.FirstOrDefault(x => string.Equals(x.User, hashedUser));
        }
    }
}