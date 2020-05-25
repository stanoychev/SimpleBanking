using Ninject.Modules;

namespace SimpleBanking
{
    public class BankingModules : NinjectModule
    {
        public override void Load()
        {
            Bind<ICookieManager>().To<CookieManager>().InSingletonScope();
            Bind<IBankDb>().To<BankDb>();
            Bind<ICustomerService>().To<CustomerService>().InSingletonScope();
            Bind<IDbService>().To<DbService>().InSingletonScope();
            Bind<ICommandParser>().To<CommandParser>().InSingletonScope();
            Bind<IATM>().To<ATM>().InSingletonScope();
        }
    }
}