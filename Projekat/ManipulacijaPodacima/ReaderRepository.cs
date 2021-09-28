using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulacijaPodacima
{
    public class ReaderRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public ReaderRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new
            Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("ZadatakTabela");
            _table.CreateIfNotExists();

        }

        public IQueryable<Student> VratiSveStudente()
        {
            var results = from g in _table.CreateQuery<Student>() where g.PartitionKey == "Student" select g;
            return results;
        }

        public IQueryable<Predmet> VratiSvePredmete()
        {
            var results = from g in _table.CreateQuery<Predmet>() where g.PartitionKey == "Predmet" select g;
            return results;
        }

        public IQueryable<Ispit> VratiSveIspite()
        {
            var results = from g in _table.CreateQuery<Ispit>() where g.PartitionKey == "Ispit" select g;
            return results;
        }


        public Student PronadjiStudenta(string index)
        {
            TableOperation operacija = TableOperation.Retrieve<Student>("Student", index);
            return (Student)_table.Execute(operacija).Result;

        }

        public Predmet PronadjiPredmet(string oznakaPredmeta)
        {
            TableOperation operacija = TableOperation.Retrieve<Predmet>("Predmet", oznakaPredmeta);
            return (Predmet)_table.Execute(operacija).Result;

        }

        public Ispit PronadjiIspit(string idIspita)
        {
            TableOperation operacija = TableOperation.Retrieve<Ispit>("Ispit", idIspita);
            return (Ispit)_table.Execute(operacija).Result;

        }






    }
}
