using Ninject.Modules;

namespace SimpleBanking
{
    public class BankingModules : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBankDb>().To<BankDb>().InSingletonScope();
            this.Bind<IDbService>().To<DbService>().InSingletonScope();

            this.Bind<IBankManager>().To<BankManager>().InSingletonScope();
            
            this.Bind<ICommandParser>().To<CommandParser>().InSingletonScope();

            this.Bind<IConsoleBankEngine>().To<ConsoleBankEngine>().InSingletonScope();
        }
    }
}