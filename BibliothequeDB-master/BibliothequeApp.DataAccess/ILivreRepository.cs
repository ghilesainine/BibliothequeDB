using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliothequeApp.Domain;

namespace BibliothequeApp.DataAccess
{
    public interface ILivreRepository : IRepository<Livre>
    {
        List<Livre> GetAvailableBooks();
    }
}
