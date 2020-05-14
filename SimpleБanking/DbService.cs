using System;
using System.Collections.Generic;
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

            return customer.Transactions.ToList()
                .Select(transaction =>
                {
                    if (transaction.Sender == null || transaction.Receiver == null)
                        return transaction.Sender == null ? transaction.Amount : -transaction.Amount;

                    return transaction.Sender.Id == customer.Id ? -transaction.Amount : transaction.Amount;
                }).Sum();
        }

        public string GetFormatedHistory((string user, string pin) credentials)
        {
            var customer = GetCustomer(credentials);
            if (customer == null)
                return $"[{credentials.user}] not found.";

            return $"History for [{credentials.user}]:\n" +
                string.Join("\n", customer.Transactions
                .OrderByDescending(x => x.Date)
                .ToList()
                .Select(transaction =>
                {
                    var type = transaction.Sender == null ? "[Deposit]" : 
                    transaction.Receiver == null ? "[Withdraw]" :
                    $"[Transfer from [{transaction.Sender.Name}] to [{transaction.Receiver.Name}]]";

                    return "[Date " + transaction.Date.Value.ToString("dd/MM/yyyy hh:mm:ss") + "]" +
                    type + $"[Amount {string.Format("{0:0.00}", transaction.Amount)}]";
                }));
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
            return numberOfSavedItems == 8;
        }

        public bool CustomerExists((string user, string pin) credentials) => GetCustomer(credentials) != null;

        public bool CustomerExists(string user) => GetCustomer(user) != null;

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
        {
            if (from == null && to == null)
                throw new ArgumentNullException("Sender and reciver are null.");

            var customers = new List<int>();
            if (from != null)
                customers.Add(from.Id);

            if (to != null)
                customers.Add(to.Id);

            bankDb.Customers.Where(x => customers.Contains(x.Id)).ToList()
                .ForEach(customer => customer.Transactions.Add(new Transaction()
                {
                    Amount = amount,
                    Date = DateTime.Now,
                    Sender = from,
                    Receiver = to
                }));
        }

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