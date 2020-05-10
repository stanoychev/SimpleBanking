using System.Data.Entity;

namespace SimpleBanking
{
    public interface IBankDb
    {

    }

    public class BankDb : DbContext, IBankDb
    {
        public BankDb() : base("BankDb")
        {

        }
    }
}