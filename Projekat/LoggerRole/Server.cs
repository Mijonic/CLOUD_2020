using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LoggerRole
{
    public class Server
    {
        private ServiceHost service;

        public Server()
        {
            NetTcpBinding binding = new NetTcpBinding();

            RoleInstanceEndpoint role = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InputRequest"];


            string adresa = string.Format("net.tcp://{0}/{1}", role.IPEndpoint, "InputRequest");
            service = new ServiceHost(typeof(ServerProvider));
            service.AddServiceEndpoint(typeof(ILogger), binding, adresa);
        }


        public void Open()
        {
            Trace.WriteLine("Otvaranje konekcije sa LoggerRole");
            try
            {
                service.Open();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace.ToString());
            }

        }

        public void Close()
        {
            Trace.WriteLine("Zatvaranje konekcije sa LoggerRole");
            try
            {
                service.Close();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace.ToString());
            }

        }
    }
}
