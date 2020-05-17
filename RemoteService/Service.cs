using Ninject;
using SimpleBanking;
using System.ServiceModel;

namespace RemoteService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string ExecuteCommand(string command);
    }

    public class Service : IService
    {
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

        ~Service()
        {
            throw new System.Exception("hoi");
        }
    }
}