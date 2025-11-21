using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.Domain
{
    public class Emprunt
    {
        public int IdEmprunt { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }

        public int IdLivre { get; set; }
        public int IdUsager { get; set; }

        
        public Livre? Livre { get; set; }
        public Usage? Usager { get; set; }
    }
}
