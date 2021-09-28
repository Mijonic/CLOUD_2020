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
    public class PredmetController : Controller
    {
        // GET: Predmet
        public ActionResult Index()
        {

            ChannelFactory<IReader> factory = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy = factory.CreateChannel();

            List<Predmet> predmeti = proxy.VratiSvePredmete();

            return View(predmeti);
        }


        public ActionResult Napravi()
        {
            return View("DodavanjePredmeta");
        }

        public ActionResult ObrisiPredmet(string oznakaPredmeta)
        {

            ChannelFactory<IReader> factory1 = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy1 = factory1.CreateChannel();


            Predmet zaBrisanje = proxy1.PronadjiPredmet(oznakaPredmeta); // mora postojati


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

                uslov = proxy.ObrisiPredmet(zaBrisanje);
            }



            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult DodavanjePredmeta(String oznakaPredmeta, String nazivPredmeta)
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

                uslov = proxy.DodajPredmet(oznakaPredmeta, nazivPredmeta);
            }
          


            return RedirectToAction("Index");
        }


        public ActionResult Modifikovanje(string oznakaPredmeta)
        {
            ChannelFactory<IReader> factory1 = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy1 = factory1.CreateChannel();

            Predmet s = proxy1.PronadjiPredmet(oznakaPredmeta);

            return View(s);
        }

        [HttpPost]
        public ActionResult ModifikujPredmet(string oznakaPredmeta, string nazivPredmeta)
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


                uslov = proxy.AzurirajPredmet(oznakaPredmeta, nazivPredmeta);

            }



            return RedirectToAction("Index");
        }




    }
}