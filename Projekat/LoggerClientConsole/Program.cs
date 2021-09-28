using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerClientConsole
{
    class Program
    {

        public static List<string> logovi = new List<string>();
        
        static void Main(string[] args)
        {


            bool uslovIzvrsavanja = true;

            Thread thread = new Thread(Program.AutomatskiUcitaj);
            thread.IsBackground = true;
            thread.Start();
           

            while(uslovIzvrsavanja)
            {

                bool validan = false;
                int izbor = -1;

                do
                {
                    Console.WriteLine("\tMENU");
                    Console.WriteLine("1 - Prikazi sve logove");
                    Console.WriteLine("2 - Zatvorite LoggerClientConsole program");
                    Console.WriteLine("Izaberite neku od opcija:");

                    validan = int.TryParse(Console.ReadLine(), out izbor);


                } while (!validan);

                switch (izbor)
                {
                    case 1:
                        {
                            ChannelFactory<ILogger> factory = new ChannelFactory<ILogger>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10101/InputRequest"));
                            ILogger proxy = factory.CreateChannel();

                            List<string> sviLogovi = proxy.VratiSveLogove();

                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("SVI LOGOVI NA ZAHTEV KLIJENTA");
                            foreach (string log in sviLogovi)
                                Console.WriteLine(log);

                            Console.WriteLine("--------------------------------------------");

                            break;

                        }

                    case 2: uslovIzvrsavanja = false; break;
                    default: Console.WriteLine("Pogresan izbor"); break;
                        
    
            }


            
        }


 
        


        }

    
        public static void AutomatskiUcitaj()
        {
            while (true)
            {
                ChannelFactory<ILogger> factory = new ChannelFactory<ILogger>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10101/InputRequest"));
                ILogger proxy = factory.CreateChannel();

                List<string> novi = proxy.AutomatskoSlanjeLogova(logovi);

                if (novi.Count != 0)
                {

                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("Pristigli novi logovi koji se automatski salju");

                    foreach (string s in novi)
                        Console.WriteLine(s);

                    foreach (string s in novi)
                        logovi.Add(s);

                    Console.WriteLine("-------------------------------------------------------");

                    Console.WriteLine("\n\tMENU");
                    Console.WriteLine("1 - Prikazi sve logove");
                    Console.WriteLine("2 - Zatvorite LoggerClientConsole program");
                    Console.WriteLine("Izaberite neku od opcija:");

                    
                }

                Thread.Sleep(3000);

            }



        }



       
      

       

    

    }
}
