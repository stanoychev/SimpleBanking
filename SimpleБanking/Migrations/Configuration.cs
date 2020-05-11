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
            foreach (var account in context.Accounts)
                context.Accounts.Remove(account);

            foreach (var customer in context.Customers)
                context.Customers.Remove(customer);

            var tools = new Tools();

            Register(context, tools, 100000, "bank", "123");
            Register(context, tools, 1000, "pesho", "111");
            Register(context, tools, 2000, "misho", "222");
            Register(context, tools, 3000, "gosho", "333");
        }

        void Register(BankDb context, Tools tools, double amount, string name, string pin)
        {
            //var customer = new Customer()
            //{
            //    User = tools.HashString(name),
            //    Pin = tools.HashString(pin)
            //};

            //var account = new Account()
            //{
            //    Balance = amount,
            //};

            //context.Accounts.Add(account);
            ////context.SaveChanges();

            //customer.Account = context.Accounts.Last();
            //context.Customers.Add(customer);

            //context.SaveChanges();


            //throw new System.Exception(account.AccountId.ToString());

            //account.Customer = new Customer()
            //{
            //    User = tools.HashString(name),
            //    Pin = tools.HashString(pin),
            //    //Account = context.Accounts.Last()
            //};

            ////context.Customers.Add(customer);
            //context.SaveChanges();
        }
    }
}
