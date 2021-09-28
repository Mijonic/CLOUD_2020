using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulacijaPodacima
{
    public class Ispit : TableEntity
    {

        private bool polozen;
        private DateTime datum;
        private string idIspita;
      



        public Ispit()
        {

        }

        public Ispit(string idIspita, DateTime datum, bool polozen)
        {
            PartitionKey = "Ispit";
            RowKey = idIspita;
            IdIspita = idIspita;
            Datum = datum;
            Polozen = polozen;
        }

        

        public bool Polozen { get => polozen; set => polozen = value; }

        [DataType(DataType.Date, ErrorMessage = "Pogresan format!")]
        [Required(ErrorMessage = "Morate izabrati datum!")]
        public DateTime Datum { get => datum; set => datum = value; }

        [Required(ErrorMessage = "Morate uneti ID ispita!")]
        public string IdIspita { get => idIspita; set => idIspita = value; }
    }
}
