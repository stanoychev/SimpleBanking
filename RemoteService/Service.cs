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
        string SayWellcome();
        [OperationContract]
        string ExecuteCommand(string command);
        [OperationContract(IsTerminating = true)]
        void EndSession();
    }

    public class Service : IService
    {
        const string wellcome =
            "Bank started.\n" +
            "Please type command.\n" +
            "Type [h] for list of available commands or [q] to quit.";

        readonly IRemoteEngine remoteEngine;
        readonly IKernel kernel;

        public Service()
        {
            kernel = new StandardKernel(new BankingModules());
            kernel.Get<IDbService>().CreateContextAndSeed();

            remoteEngine = kernel.Get<IRemoteEngine>();
        }

        public string ExecuteCommand(string command)
        {
            remoteEngine.SendCommand(command);
            return remoteEngine.Status;
        }

        public void EndSession() { }

        public string SayWellcome() => wellcome;
    }
}