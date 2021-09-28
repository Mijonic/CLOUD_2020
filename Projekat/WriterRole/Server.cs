using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WriterRole
{
    public class Server
    {
        private ServiceHost serviceHost;
        private string internalEndpoint = "InternalRequest";

        public Server()
        {
            RoleInstanceEndpoint inputEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[internalEndpoint];

            string endpoint = String.Format("net.tcp://{0}/{1}", inputEndpoint.IPEndpoint, internalEndpoint);

            NetTcpBinding binding = new NetTcpBinding();
            serviceHost = new ServiceHost(typeof(ServerProvider));

            serviceHost.AddServiceEndpoint(typeof(IWriter), binding, endpoint);




        }

        public void Open()
        {
            Trace.WriteLine("Otvaranje konekcije sa WorkerRole");
            try
            {
                serviceHost.Open();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace.ToString());
            }

        }

        public void Close()
        {
            Trace.WriteLine("Zatvaranje konekcije sa WorkerRole");
            try
            {
                serviceHost.Close();

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace.ToString());
            }

        }
    }
}
