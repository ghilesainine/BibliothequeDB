using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.Domain
{
    public class Usage
    {
        public int IdUsager { get; set; }
        public string Nom { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Telephone { get; set; }
    }
}
