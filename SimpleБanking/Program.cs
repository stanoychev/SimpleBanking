using Ninject;
using System;
using System.IO;

namespace SimpleBanking
{
    public class Program
    {
        
        public static void Main()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            var kernel = new StandardKernel(new BankingModules());
            kernel.Get<IDbService>().CreateDbAndSeed();

            kernel.Get<IConsoleBankEngine>().Run();
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Application crashed with the following message:\n");
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception:\n" + ex.InnerException.Message);

                Console.WriteLine("Press ENTER to close the application.");
                Console.ReadLine();
            }
        }
    }
}