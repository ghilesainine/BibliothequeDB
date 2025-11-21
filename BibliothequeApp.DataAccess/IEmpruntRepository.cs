using BibliothequeApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.DataAccess
{
    public interface IEmpruntRepository : IRepository<Emprunt>
    {
        List<Emprunt> GetByUsager(int idUsager);
    }
}
