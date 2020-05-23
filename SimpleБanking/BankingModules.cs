using Ninject.Modules;

namespace SimpleBanking
{
    public class BankingModules : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICookieManager>().To<CookieManager>().InSingletonScope();
            this.Bind<IBankDb>().To<BankDb>().InSingletonScope();
            this.Bind<ICustomerService>().To<CustomerService>().InSingletonScope();
            this.Bind<IDbService>().To<DbService>().InSingletonScope();

            this.Bind<IATM>().To<ATM>().InSingletonScope();

            this.Bind<ICommandParser>().To<CommandParser>().InSingletonScope();

            this.Bind<IBankEngine>().To<BankEngine>().InSingletonScope();
        }
    }
}