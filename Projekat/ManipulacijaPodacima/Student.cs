using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManipulacijaPodacima
{

    public class Student : TableEntity
    {
       
        private string ime;
        private string prezime;
        private string index;

        [Required(ErrorMessage = "Morate uneti prezime studenta!")]
        public string Prezime { get => prezime; set => prezime = value; }

        [Required(ErrorMessage = "Morate uneti naziv indeksa studenta!")]
        public string Index { get => index; set => index = value; }

        [Required(ErrorMessage = "Morate uneti ime studenta!")]
        public string Ime { get => ime; set => ime = value; }

        /*
        public Student(String index)
        {
            PartitionKey = "Student";
            RowKey = indexNo;
        }
        */

        public Student() { }

        public Student(string ime, string prezime, string index)
        {
            PartitionKey = "Student";
            RowKey = index;
            Prezime = prezime;
            Index = index;
            Ime = ime;
        }
    }
}
