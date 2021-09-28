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
    public class IspitController : Controller
    {
        

        // GET: Ispit
        public ActionResult Index()
        {


            ChannelFactory<IReader> factory = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy = factory.CreateChannel();

            List<Ispit> ispiti = proxy.VratiSveIspite();

            

            return View(ispiti);
        }


        public ActionResult Napravi()
        {
            return View("DodavanjeIspita");
        }

        public ActionResult ObrisiIspit(string idIspita)
        {

            ChannelFactory<IReader> factory1 = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy1 = factory1.CreateChannel();


            Ispit zaBrisanje = proxy1.PronadjiIspit(idIspita); // mora postojati


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

                uslov = proxy.ObrisiIspit(zaBrisanje);
            }



            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult DodavanjeIspita(String idIspita, DateTime datum, bool polozen)
        {


            if (datum == null)
                datum = DateTime.MinValue;

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

                uslov = proxy.DodajIspit(idIspita, datum, polozen);
            }

            return RedirectToAction("Index");
        }


        public ActionResult Modifikovanje(string idIspita)
        {
            ChannelFactory<IReader> factory1 = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy1 = factory1.CreateChannel();

            Ispit s = proxy1.PronadjiIspit(idIspita);

            return View(s);
        }

        [HttpPost]
        public ActionResult ModifikujIspit(String idIspita, DateTime datum, bool polozen)
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


                uslov = proxy.AzurirajIspit(idIspita, datum, polozen);

            }



            return RedirectToAction("Index");
        }




    }
}