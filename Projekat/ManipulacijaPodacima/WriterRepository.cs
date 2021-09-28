using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulacijaPodacima
{
    public class WriterRepository
    {

        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public WriterRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new
            Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("ZadatakTabela");
            _table.CreateIfNotExists();

        }


        public void DodajStudenta(Student noviStudent)
        {
            
            TableOperation insertOperation = TableOperation.Insert(noviStudent);
            _table.Execute(insertOperation);
        }

        public void DodajPredmet(Predmet noviPredmet)
        {

            TableOperation insertOperation = TableOperation.Insert(noviPredmet);
            _table.Execute(insertOperation);
        }

        public void DodajIspit(Ispit noviIspit)
        {

            TableOperation insertOperation = TableOperation.Insert(noviIspit);
            _table.Execute(insertOperation);
        }

        public void AzurirajStudenta(Student noviStudent)
        {
            noviStudent.ETag = "*";
            TableOperation updateOperation = TableOperation.Replace(noviStudent);
            _table.Execute(updateOperation);
        }


        public void AzurirajPredmet(Predmet noviPredmet)
        {
            noviPredmet.ETag = "*";
            TableOperation updateOperation = TableOperation.Replace(noviPredmet);
            _table.Execute(updateOperation);
        }

        public void AzurirajIspit(Ispit noviIspit)
        {
            noviIspit.ETag = "*";
            TableOperation updateOperation = TableOperation.Replace(noviIspit);
            _table.Execute(updateOperation);
        }


        public void ObrisiStudenta(Student zaBrisanje)
        {
        
            TableOperation izvrsi = TableOperation.Delete(zaBrisanje);
            _table.Execute(izvrsi);
           
    
        }

        public void ObrisiPredmet(Predmet zaBrisanje)
        {
            TableOperation izvrsi = TableOperation.Delete(zaBrisanje);
            _table.Execute(izvrsi);
        }


        public void ObrisiIspit(Ispit zaBrisanje)
        {
            TableOperation izvrsi = TableOperation.Delete(zaBrisanje);
            _table.Execute(izvrsi);
        }






    }
}
