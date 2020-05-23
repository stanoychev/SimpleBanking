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
        readonly IBankEngine bankEngine;
        readonly IKernel kernel;

        public Service()
        {
            kernel = new StandardKernel(new BankingModules());
            kernel.Get<IDbService>().CreateDbAndSeed();

            bankEngine = kernel.Get<IBankEngine>();
        }

        public string ExecuteCommand(string command) => bankEngine.ExecuteCommand_(command);

        public void EndSession() { }
    }
}