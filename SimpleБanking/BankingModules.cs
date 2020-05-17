using Ninject.Modules;

namespace SimpleBanking
{
    public class BankingModules : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IBankDb>().To<BankDb>().InSingletonScope();
            this.Bind<IDbService>().To<DbService>().InSingletonScope();

            this.Bind<IATM>().To<ATM>().InSingletonScope();
            
            this.Bind<ICommandParser>().To<CommandParser>().InSingletonScope();

            this.Bind<IConsoleBankEngine>().To<ConsoleBankEngine>().InSingletonScope();
            this.Bind<IRemoteEngine>().To<RemoteEngine>().InSingletonScope();
        }
    }
}