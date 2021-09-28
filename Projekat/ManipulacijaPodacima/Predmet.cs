using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulacijaPodacima
{
    public class Predmet : TableEntity
    {


        private string nazivPredmeta;
        private string oznakaPredmeta; // e227a 

        public Predmet()
        {

        }


        [Required(ErrorMessage = "Morate uneti naziv predmeta!")]
        public string NazivPredmeta { get => nazivPredmeta; set => nazivPredmeta = value; }

        [Required(ErrorMessage = "Morate uneti oznaku predmeta!")]
        public string OznakaPredmeta { get => oznakaPredmeta; set => oznakaPredmeta = value; }

        public Predmet(string oznakaPredmeta, string nazivPredmeta)
        {
            PartitionKey = "Predmet";
            RowKey = oznakaPredmeta;
            NazivPredmeta = nazivPredmeta;
            OznakaPredmeta = oznakaPredmeta;
        }


    }
}
