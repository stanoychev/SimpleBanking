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
            if (context.Accounts.Any())
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
                Name = name,
                Account = new Account()
                {
                    Balance = amount,
                }
            };

            context.Customers.Add(customer);
            context.SaveChanges();
        }
    }
}
