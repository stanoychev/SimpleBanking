using Ninject;
using SimpleBanking;
using System.ServiceModel;

namespace RemoteService
{
    [ServiceContract(SessionMode = SessionMode.Required,
        ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    public interface IService
    {
        [OperationContract(IsInitiating = true)]
        string ExecuteCommand(string command);
        [OperationContract(IsTerminating = true)]
        void EndSession();
    }

    public class Service : IService
    {
        readonly IATM ATM;
        readonly IKernel kernel;

        public Service()
        {
            kernel = new StandardKernel(new BankingModules());
            kernel.Get<IDbService>().CreateDbAndSeed();

            ATM = kernel.Get<IATM>();
        }

        public string ExecuteCommand(string command) => ATM.ExecuteCommand(command);

        public void EndSession() { }
    }
}