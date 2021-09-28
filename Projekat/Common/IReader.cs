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
    public interface IReader
    {
        [OperationContract]
        List<Student> VratiSveStudente();

        [OperationContract]
        List<Predmet> VratiSvePredmete();

        [OperationContract]
        List<Ispit> VratiSveIspite();

        [OperationContract]
        Student PronadjiStudenta(string index);

        [OperationContract]
        Predmet PronadjiPredmet(string oznakaPredmeta);

        [OperationContract]
        Ispit PronadjiIspit(string idIspita);




    }
}
