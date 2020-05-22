using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBanking
{
    public interface IDbService
    {
        void CreateDbAndSeed();
        double? GetBalance(string cookie);
        bool Deposit(string cookie, double amount);
        bool Withdraw(string cookie, double amount);
        bool Transfter(string cookie, double amount, string user);
        string GetFormatedHistory(string cookie);
    }

    public class DbService : IDbService
    {
        readonly IBankDb bankDb;
        readonly ICookieManager cookieManager;

        public DbService(IBankDb bankDb_, ICookieManager cookieManager_)
        {
            bankDb = bankDb_;
            cookieManager = cookieManager_;
        }

        public bool Deposit(string cookie, double amount)
        {
            var customer = GetCustomer(cookie);
            if (customer == null)
                return false;

            AddTransaction(amount, null, customer);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        public double? GetBalance(string cookie)
        {
            var customer = GetCustomer(cookie);
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

        public string GetFormatedHistory(string cookie)
        {
            var customer = GetCustomer(cookie);
            if (customer == null)
                return $"User not found.";

            return $"History for [{customer.Name}]:\n" +
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

        public bool Transfter(string cookie, double amount, string user)
        {
            var customer = GetCustomer(cookie);
            if (customer == null)
                return false;

            var recipient = GetCustomerByUserName(user);
            if (recipient == null)
                return false;

            AddTransaction(amount, customer, recipient);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 8;
        }

        public bool Withdraw(string cookie, double amount)
        {
            var customer = GetCustomer(cookie);
            if (customer == null)
                return false;

            AddTransaction(amount, customer, null);

            var numberOfSavedItems = bankDb.SaveChanges();
            return numberOfSavedItems == 3;
        }

        public void CreateDbAndSeed()
        {
            //CleanContext(context);

            if (bankDb.Customers.Any())
                return;

            Register(bankDb, 100000, "bank", "123", "Bank Manager");
            Register(bankDb, 1000, "pesho", "111", "Petur Petkov");
            Register(bankDb, 2000, "misho", "222", "Mihail Mihailov");
            Register(bankDb, 3000, "gosho", "333", "Georgi Georgiev");
        }

        void AddTransaction(double amount, Customer from, Customer to)
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

        Customer GetCustomer(string cookie)
        {
            var id = cookieManager.GetUserId(cookie);
            return id > 0 ? bankDb.Customers.FirstOrDefault(x => x.Id == id) : null;
        }

        Customer GetCustomerByUserName(string user)
        {
            var hashedUser = HashString(user);
            return bankDb.Customers.FirstOrDefault(x => x.User == hashedUser);
        }

        void Register(IBankDb context, double amount, string user, string pin, string name)
        {
            var customer = new Customer()
            {
                User = HashString(user),
                Pin = HashString(pin),
                Name = name
            };

            context.Customers.Add(customer);

            var transaction = new Transaction()
            {
                Amount = amount,
                Sender = null,
                Receiver = customer,
                Date = DateTime.Now
            };

            context.Transactions.Add(transaction);
            context.SaveChanges();
            context.Customers.First(x => string.Equals(x.User, customer.User)).Transactions.Add(transaction);
            context.SaveChanges();
        }

        void CleanContext(BankDb context)
        {
            foreach (var item in context.Customers)
                context.Customers.Remove(item);

            foreach (var item in context.Transactions)
                context.Transactions.Remove(item);

            context.SaveChanges();
        }

        string HashString(string input)
        {
            using (SHA1 sha = SHA1.Create())
                return string.Join(string.Empty, sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}