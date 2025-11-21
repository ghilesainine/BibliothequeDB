using BibliothequeApp.DataAccess;
using BibliothequeApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.Business
{
    public class UsagerService
    {
        private readonly IUsagerRepository _usagerRepo;

        public UsagerService(IUsagerRepository usagerRepository)
        {
            _usagerRepo = usagerRepository
            ?? throw new ArgumentNullException(nameof(usagerRepository));
        }

        public List<Usage> GetAll()
        {
            return _usagerRepo.GetAll();
        }

        public Usage? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id d'usager invalide.", nameof(id));

            return _usagerRepo.GetById(id);
        }

        public List<Usage> RechercherParNom(string nom)
        {
            nom ??= string.Empty;
            return _usagerRepo.SearchByName(nom);
        }

        public void AjouterUsager(Usage usager)
        {
            if (usager == null)
                throw new ArgumentNullException(nameof(usager));

            if (string.IsNullOrWhiteSpace(usager.Nom))
                throw new ArgumentException("Le nom de l'usager est obligatoire.");

            if (string.IsNullOrWhiteSpace(usager.Email))
                throw new ArgumentException("L'email de l'usager est obligatoire.");

            // Ici tu pourrais ajouter une validation simple de format email si tu veux.

            _usagerRepo.Add(usager);
        }

        public void ModifierUsager(Usage usager)
        {
            if (usager == null)
                throw new ArgumentNullException(nameof(usager));

            if (usager.IdUsager <= 0)
                throw new ArgumentException("Id d'usager invalide pour la modification.");

            if (string.IsNullOrWhiteSpace(usager.Nom))
                throw new ArgumentException("Le nom de l'usager est obligatoire.");

            if (string.IsNullOrWhiteSpace(usager.Email))
                throw new ArgumentException("L'email de l'usager est obligatoire.");

            _usagerRepo.Update(usager);
        }

        public void SupprimerUsager(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id d'usager invalide.", nameof(id));

            _usagerRepo.Delete(id);
        }
    }
}
