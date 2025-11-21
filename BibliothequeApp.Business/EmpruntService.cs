using BibliothequeApp.DataAccess;
using BibliothequeApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.Business
{
    public class EmpruntService
    {
        private readonly IEmpruntRepository _empruntRepo;
        private readonly ILivreRepository _livreRepo;

        public EmpruntService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository)
        {
            _empruntRepo = empruntRepository
            ?? throw new ArgumentNullException(nameof(empruntRepository));
            _livreRepo = livreRepository
            ?? throw new ArgumentNullException(nameof(livreRepository));
        }

        // ----------- Lecture ------------

        public List<Emprunt> GetAll()
        {
            return _empruntRepo.GetAll();
        }

        public Emprunt? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id d'emprunt invalide.", nameof(id));

            return _empruntRepo.GetById(id);
        }

        public List<Emprunt> GetEmpruntsParUsager(int idUsager)
        {
            if (idUsager <= 0)
                throw new ArgumentException("Id d'usager invalide.", nameof(idUsager));

            return _empruntRepo.GetByUsager(idUsager);
        }

        // ----------- Création ------------

        /// <summary>
        /// Enregistre un nouvel emprunt :
        /// - Vérifie que le livre existe
        /// - Vérifie qu'il reste des exemplaires (Quantite > 0)
        /// - Ajoute l'emprunt
        /// - Diminue la quantité du livre de 1
        /// </summary>
        public void EnregistrerEmprunt(Emprunt emprunt)
        {
            if (emprunt == null)
                throw new ArgumentNullException(nameof(emprunt));

            if (emprunt.IdLivre <= 0)
                throw new ArgumentException("IdLivre invalide.");

            if (emprunt.IdUsager <= 0)
                throw new ArgumentException("IdUsager invalide.");

            if (emprunt.DateRetourPrevue < emprunt.DateEmprunt)
                throw new ArgumentException("La date de retour prévue doit être >= à la date d'emprunt.");

            var livre = _livreRepo.GetById(emprunt.IdLivre)
            ?? throw new ArgumentException("Livre inexistant.");

            if (livre.Quantite <= 0)
                throw new InvalidOperationException("Plus d'exemplaires disponibles pour ce livre.");

            // 1. Ajouter l'emprunt
            _empruntRepo.Add(emprunt);

            // 2. Mettre à jour la quantité du livre
            livre.Quantite -= 1;
            _livreRepo.Update(livre);
        }

        // ----------- Modification ------------

        public void ModifierEmprunt(Emprunt emprunt)
        {
            if (emprunt == null)
                throw new ArgumentNullException(nameof(emprunt));

            if (emprunt.IdEmprunt <= 0)
                throw new ArgumentException("Id d'emprunt invalide.");

            if (emprunt.IdLivre <= 0)
                throw new ArgumentException("IdLivre invalide.");

            if (emprunt.IdUsager <= 0)
                throw new ArgumentException("IdUsager invalide.");

            if (emprunt.DateRetourPrevue < emprunt.DateEmprunt)
                throw new ArgumentException("La date de retour prévue doit être >= à la date d'emprunt.");

            // Ici, on ne change pas la quantité de livres (on suppose que
            // l'emprunt existe déjà et que la quantité avait déjà été ajustée).

            _empruntRepo.Update(emprunt);
        }

        // ----------- Suppression ------------

        public void SupprimerEmprunt(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id d'emprunt invalide.", nameof(id));

            _empruntRepo.Delete(id);
        }

        public void RetournerEmprunt(int idEmprunt)
        {
            var emprunt = _empruntRepo.GetById(idEmprunt)
            ?? throw new ArgumentException("Emprunt introuvable.");

            var livre = _livreRepo.GetById(emprunt.IdLivre)
            ?? throw new ArgumentException("Livre introuvable.");

            // Incrémenter la quantité du livre
            livre.Quantite += 1;
            _livreRepo.Update(livre);

            // Supprimer l'emprunt
            _empruntRepo.Delete(idEmprunt);
        }
    }

}
