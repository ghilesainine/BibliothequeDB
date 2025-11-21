using BibliothequeApp.Domain;

namespace BibliothequeApp.DataAccess
{
    public interface IUsagerRepository : IRepository<Usage>
    {
        List<Usage> SearchByName(string name);
    }
}
