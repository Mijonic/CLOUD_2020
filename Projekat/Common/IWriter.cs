using ManipulacijaPodacima;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IWriter
    {
        [OperationContract]
        bool DodajStudenta(string index, string ime, string prezime);

        [OperationContract]
        bool DodajPredmet(string oznakaPredmeta,string nazivPredmeta);

        [OperationContract]
        bool DodajIspit(string idIspita, DateTime datum, bool polozen);

        [OperationContract]
        bool AzurirajStudenta(string index, string ime, string prezime);

        [OperationContract]
        bool AzurirajPredmet(string oznakaPredmeta, string nazivPredmeta);

        [OperationContract]
        bool AzurirajIspit(string idIspita, DateTime datum, bool polozen);

        [OperationContract]
        bool ObrisiStudenta(Student student);

        [OperationContract]
        bool ObrisiPredmet(Predmet predmet);

        [OperationContract]
        bool ObrisiIspit(Ispit ispit);
       


    }
}
