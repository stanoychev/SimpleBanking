using Ninject;
using System;
using System.IO;

namespace SimpleBanking
{
    public class Program
    {
        
        public static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
                var kernel = new StandardKernel(new BankingModules());
                kernel.Get<IDbService>().CreateContextAndSeed();
                
                kernel.Get<IConsoleBankEngine>().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}