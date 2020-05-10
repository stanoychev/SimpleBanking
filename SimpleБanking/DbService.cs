namespace SimpleBanking
{
    public interface IDbService
    {
        int? GetAccount(string user, string pin);
        double GetBalance(int accountId);
        void Withdraw(int accountId, double amount);
    }

    public class DbService : IDbService
    {
        readonly IBankDb bankDb;
        public DbService(IBankDb bankDb_)
        {
            bankDb = bankDb_;
        }

        public int? GetAccount(string user, string pin)
        {
            throw new System.NotImplementedException();
        }

        public double GetBalance(int accountId)
        {
            throw new System.NotImplementedException();
        }

        public void Withdraw(int accountId, double amount)
        {
            throw new System.NotImplementedException();
        }
    }
}