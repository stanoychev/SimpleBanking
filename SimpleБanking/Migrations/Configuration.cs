using System.Data.Entity.Migrations;

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
            => new DbService(context).CreateContextAndSeed();
    }
}