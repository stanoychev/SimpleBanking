﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBanking
{
    public interface IDbService
    {
        void CreateContextAndSeed();
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

        public void CreateContextAndSeed()
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

        Customer GetCustomer((string user, string pin) credentials)
        {
            var hashedUser = HashString(credentials.user);
            var hashedPin = HashString(credentials.pin);

            var customer = bankDb.Customers
                .FirstOrDefault(x => string.Equals(x.User, hashedUser));

            return string.Equals(customer.Pin, hashedPin) ? customer : null;
        }

        Customer GetCustomer(string user)
        {
            var hashedUser = HashString(user);

            return bankDb.Customers.FirstOrDefault(x => string.Equals(x.User, hashedUser));
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