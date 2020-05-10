using Ninject;

namespace SimpleBanking
{
    public class Program
    {
        public static void Main()
            => new StandardKernel(new BankingModules())
            .Get<IConsoleBankEngine>()
            .Run();
    }
}