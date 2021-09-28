using Common;
using ManipulacijaPodacima;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WriterRole
{
    public class ServerProvider : IWriter
    {

        WriterRepository writerRepo = new WriterRepository();

        public bool AzurirajIspit(string idIspita, DateTime datum, bool polozen)
        {
            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 1)
                return false;

            Ispit ispit = new Ispit(idIspita, datum, polozen);
            writerRepo.AzurirajIspit(ispit);


            CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
            string poruka = $" [{DateTime.Now}] Azuriranje ispita: IdIspita: {ispit.IdIspita} Datun: {ispit.Datum} Polozen: {ispit.Polozen}";
            CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
            queue.AddMessage(queue_poruka, null, null);


            return true;
        }

        public bool AzurirajPredmet(string oznakaPredmeta, string nazivPredmeta)
        {
            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 1)
                return false;

            Predmet predmet = new Predmet(oznakaPredmeta, nazivPredmeta);
            writerRepo.AzurirajPredmet(predmet);

            CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
            string poruka = $" [{DateTime.Now}] Azuriranje predmeta: Oznaka predmeta: {predmet.OznakaPredmeta} Naziv Predmeta: {predmet.NazivPredmeta}";
            CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);

            queue.AddMessage(queue_poruka, null, null);

            return true;
        }

        public bool AzurirajStudenta(string index, string ime, string prezime)
        {

            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 0 && pomInd != 2)
                return false;

            Student s = new Student(ime, prezime,index);
            writerRepo.AzurirajStudenta(s);

            CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
            string poruka = $" [{DateTime.Now}] Azuriranje studenta: Index: {s.Index} Ime: {s.Ime} Prezime: {s.Prezime}";
            CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
            queue.AddMessage(queue_poruka, null, null);


            return true;
        }

        public bool DodajIspit(string idIspita, DateTime datum, bool polozen)
        {

            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 1)
                return false;

            ChannelFactory<IReader> factory = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy = factory.CreateChannel();

            Ispit noviIspit = new Ispit(idIspita, datum, polozen);

            Ispit ispit = proxy.PronadjiIspit(noviIspit.IdIspita);
            if (ispit == null)
            {
                writerRepo.DodajIspit(noviIspit);

                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
                string poruka = $" [{DateTime.Now}] Dodavanje ispita: IdIspita: {noviIspit.IdIspita} Datum: {noviIspit.Datum} Polozen: {noviIspit.Polozen}";
                CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
                queue.AddMessage(queue_poruka, null, null);
            }
            else
            {
                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
                string poruka = $" [{DateTime.Now}] Vec postoji ispit koji ima ID: {noviIspit.IdIspita}";
                CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
                queue.AddMessage(queue_poruka, null, null);

                Trace.WriteLine($"Vec postoji ispit koji ima ID: {noviIspit.IdIspita}");
            }

            return true;
        }

        public bool DodajPredmet(string oznakaPredmeta, string nazivPredmeta)
        {
            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 1)
                return false;

            ChannelFactory<IReader> factory = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy = factory.CreateChannel();


            Predmet noviPredmet = new Predmet(oznakaPredmeta, nazivPredmeta);

            Predmet predmet = proxy.PronadjiPredmet(noviPredmet.OznakaPredmeta);
            if (predmet == null)
            {
                writerRepo.DodajPredmet(noviPredmet);
               
                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
                string poruka = $" [{DateTime.Now}] Dodavanje predmeta: Oznaka predmeta: {noviPredmet.OznakaPredmeta} Naziv Predmeta: {noviPredmet.NazivPredmeta}";
                CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);  
               
                queue.AddMessage(queue_poruka, null, null);

            }
            else
            {
                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
                string poruka = $" [{DateTime.Now}] Vec postoji predmet sa oznakom predmeta: {noviPredmet.OznakaPredmeta}";
                CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);

                queue.AddMessage(queue_poruka, null, null);

                Trace.WriteLine($"Vec postoji predmet sa oznakom predmeta: {noviPredmet.OznakaPredmeta}");
            }

            return true;
        }

        public bool DodajStudenta(string index, string ime, string prezime)
        {

            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 0 && pomInd != 2)
                return false;


            ChannelFactory<IReader> factory = new ChannelFactory<IReader>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:10100/InputRequest"));
            IReader proxy = factory.CreateChannel();

           

            Student noviStudent = new Student(ime,prezime,index);

            Student student = proxy.PronadjiStudenta(noviStudent.Index);
            if (student == null)
            {
                writerRepo.DodajStudenta(noviStudent);
                //Trace.WriteLine($"DOODAT STUDENT: {RoleEnvironment.CurrentRoleInstance.Id}");

                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
                string poruka = $" [{DateTime.Now}] Dodavanje studenta: Index: {noviStudent.Index} Ime: {noviStudent.Ime} Prezime: {noviStudent.Prezime}";
                CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
                queue.AddMessage(queue_poruka, null, null);

            }
            else
            {
                CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
                string poruka = $" [{DateTime.Now}] Vec postoji student sa ovim indeksom: {noviStudent.Index}";
                CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
                queue.AddMessage(queue_poruka, null, null);

                Trace.WriteLine($"Vec postoji student sa ovim indeksom: {noviStudent.Index}");
            }

            return true;
        }


        public bool ObrisiIspit(Ispit ispit)
        {
            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 1)
                return false;

            writerRepo.ObrisiIspit(ispit);

            CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
            string poruka = $" [{DateTime.Now}] Brisanje ispita: IdIspita: {ispit.IdIspita} Datun: {ispit.Datum} Polozen: {ispit.Polozen}";
            CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
            queue.AddMessage(queue_poruka, null, null);

            return true;

        }

        public bool ObrisiPredmet(Predmet predmet)
        {
            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 1)
                return false;

            writerRepo.ObrisiPredmet(predmet);

            CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
            string poruka = $" [{DateTime.Now}] Brisanje predmeta: Oznaka predmeta: {predmet.OznakaPredmeta} Naziv Predmeta: {predmet.NazivPredmeta}";
            CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
            queue.AddMessage(queue_poruka, null, null);

            return true;
        }

        public bool ObrisiStudenta(Student student)
        {
            int pomInd = GetIndex(RoleEnvironment.CurrentRoleInstance.Id);
            if (pomInd != 0 && pomInd != 2)
                return false;

            writerRepo.ObrisiStudenta(student);

            CloudQueue queue = QueueKreiranje.GetQueueReference("zadatakqueue");
            string poruka = $" [{DateTime.Now}] Brisanje studenta: Index: {student.Index} Ime: {student.Ime} Prezime: {student.Prezime}";
            CloudQueueMessage queue_poruka = new CloudQueueMessage(poruka);
            queue.AddMessage(queue_poruka, null, null);


            return true;
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
