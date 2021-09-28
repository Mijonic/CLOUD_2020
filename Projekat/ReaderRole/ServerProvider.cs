using Common;
using ManipulacijaPodacima;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderRole
{
    public class ServerProvider : IReader
    {
        ReaderRepository readereRepo = new ReaderRepository();

        public Ispit PronadjiIspit(string idIspita)
        {
            return readereRepo.PronadjiIspit(idIspita);
        }

        public Predmet PronadjiPredmet(string oznakaPredmeta)
        {
            return readereRepo.PronadjiPredmet(oznakaPredmeta);
        }

        public Student PronadjiStudenta(string index)
        {
            return readereRepo.PronadjiStudenta(index);
        }

        public List<Ispit> VratiSveIspite()
        {
            return readereRepo.VratiSveIspite().ToList();
        }

        public List<Predmet> VratiSvePredmete()
        {
            return readereRepo.VratiSvePredmete().ToList();
        }

        public List<Student> VratiSveStudente()
        {
            return readereRepo.VratiSveStudente().ToList();
        }
    }
}
