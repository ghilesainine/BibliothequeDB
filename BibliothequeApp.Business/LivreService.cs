using BibliothequeApp.DataAccess;
using BibliothequeApp.Domain;

namespace BibliothequeApp.Business
{
    public class LivreService
    {
        private readonly ILivreRepository _repo;

        public LivreService(ILivreRepository repo)
        {
            _repo = repo;
        }
        public List<Livre> GetAll() => _repo.GetAll();
        public void AjouterLivre(Livre livre)
        {
            if (string.IsNullOrWhiteSpace(livre.Titre))
                throw new ArgumentException("Le titre est obligatoire");
            if (string.IsNullOrWhiteSpace(livre.Auteur))
                throw new ArgumentException("L'auteur est obligatoire");
            if (livre.Quantite < 0)
                throw new ArgumentException("La quantité doit être >= 0");

            _repo.Add(livre);
        }
        public void ModifierLivre(Livre livre)
        {
            if (livre.IdLivre <= 0)
                throw new ArgumentException("IdLivre invalide");
            _repo.Update(livre);
        }

        public void SupprimerLivre(int id)
        {
            _repo.Delete(id);
        }

        public List<Livre> GetAvailableBooks() => _repo.GetAvailableBooks();
    }

}
