using Common;
using ManipulacijaPodacima;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ChannelFactory<IReader> factory = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy = factory.CreateChannel();

            List<Student> studenti = proxy.VratiSveStudente();

            return View(studenti);
        }

        public ActionResult Napravi()
        {
            return View("Dodavanje");
        }

        public ActionResult ObrisiStudenta(string index)
        {

            ChannelFactory<IReader> factory1 = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy1 = factory1.CreateChannel();


            Student zaBrisanje = proxy1.PronadjiStudenta(index); // mora postojati


            Random rand = new Random();


            List<RoleInstance> pomoc = new List<RoleInstance>();

            foreach (RoleInstance role in RoleEnvironment.Roles["WriterRole"].Instances)
            {
                pomoc.Add(role);
            }

            bool uslov = false;

            while (!uslov)
            {
                int instanca = rand.Next(3);
                ChannelFactory<IWriter> factory = new ChannelFactory<IWriter>(new NetTcpBinding(), new EndpointAddress(String.Format("net.tcp://{0}/{1}", pomoc[instanca].InstanceEndpoints["InternalRequest"].IPEndpoint, "InternalRequest")));
                IWriter proxy = factory.CreateChannel();

                uslov = proxy.ObrisiStudenta(zaBrisanje);
            }



            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Dodavanje(String index, String ime, String prezime)
        {
            Random rand = new Random();
           
            List<RoleInstance> pomoc = new List<RoleInstance>();

            foreach (RoleInstance role in RoleEnvironment.Roles["WriterRole"].Instances)
            {
                pomoc.Add(role);
            }

            bool uslov = false;

            while (!uslov)
            {
                int instanca = rand.Next(3);
                ChannelFactory<IWriter> factory = new ChannelFactory<IWriter>(new NetTcpBinding(), new EndpointAddress(String.Format("net.tcp://{0}/{1}", pomoc[instanca].InstanceEndpoints["InternalRequest"].IPEndpoint, "InternalRequest")));
                IWriter proxy = factory.CreateChannel();

                uslov = proxy.DodajStudenta(index, ime, prezime);
            }


                return RedirectToAction("Index");
        }



        public ActionResult Modifikovanje(string index)
        {
            ChannelFactory<IReader> factory1 = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy1 = factory1.CreateChannel();

            Student s = proxy1.PronadjiStudenta(index);

            return View(s);
        }

        [HttpPost]
        public ActionResult ModifikujStudenta(string index, string ime, string prezime)
        {
            Random rand = new Random();


            List<RoleInstance> pomoc = new List<RoleInstance>();

            foreach (RoleInstance role in RoleEnvironment.Roles["WriterRole"].Instances)
            {
                pomoc.Add(role);
            }

            bool uslov = false;
          

            while (!uslov)
            {
                int instanca = rand.Next(3);
                ChannelFactory<IWriter> factory = new ChannelFactory<IWriter>(new NetTcpBinding(), new EndpointAddress(String.Format("net.tcp://{0}/{1}", pomoc[instanca].InstanceEndpoints["InternalRequest"].IPEndpoint, "InternalRequest")));
                IWriter proxy = factory.CreateChannel();

               
                 uslov = proxy.AzurirajStudenta(index, ime, prezime);
               
            }



            return RedirectToAction("Index");
        }


        private int GetIndex(string instanceId)
        {
            int instanceIndex = 0;
            if (!int.TryParse(instanceId.Substring(instanceId.LastIndexOf(".") + 1), out
            instanceIndex))
            {
                int.TryParse(instanceId.Substring(instanceId.LastIndexOf("_") + 1), out
                instanceIndex);
            }
            return instanceIndex;
        }



    }
}