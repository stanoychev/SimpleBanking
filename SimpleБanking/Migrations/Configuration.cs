using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SimpleBanking.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SimpleBanking.BankDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(BankDb context)
        {
            foreach (var item in context.Customers)
                context.Customers.Remove(item);

            foreach (var item in context.Transactions)
                context.Transactions.Remove(item);
            
            context.SaveChanges();

            if (context.Customers.Any())
                return;

            var tools = new Tools();

            Register(context, tools, 100000, "bank", "123", "Bank Manager");
            Register(context, tools, 1000, "pesho", "111", "Petur Petkov");
            Register(context, tools, 2000, "misho", "222", "Mihail Mihailov");
            Register(context, tools, 3000, "gosho", "333", "Georgi Georgiev");
        }

        void Register(BankDb context, Tools tools, double amount, string user, string pin, string name)
        {
            var customer = new Customer()
            {
                User = tools.HashString(user),
                Pin = tools.HashString(pin),
                Name = name
            };
            
            context.Customers.Add(customer);

            var transaction = new Transaction()
            {
                Amount = amount,
                From = null,
                To = customer,
                Date = DateTime.Now
            };

            context.Transactions.Add(transaction);
            context.SaveChanges();
            context.Customers.First(x => string.Equals(x.User, customer.User)).Transactions.Add(transaction);
            context.SaveChanges();
        }
    }
}
