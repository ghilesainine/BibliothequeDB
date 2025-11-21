using System;
using BibliothequeApp.DataAccess;
using BibliothequeApp.Business;
using BibliothequeApp.Domain;

namespace BibliothequeDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // instanciation des repositories
            ILivreRepository livreRepo = new SqlLivreRepository();
            IUsagerRepository usagerRepo = new SqlUsageRepository();
            IEmpruntRepository empruntRepo = new SqlEmpruntRepository();
            // services
            var livreService = new LivreService(livreRepo);
            var usagerService = new UsagerService(usagerRepo);
            var empruntService = new EmpruntService(empruntRepo, livreRepo);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("-*-*-*- GESTION BIBLIOTHEQUE -*-*-*");
                Console.WriteLine("1. Gestion des livres");
                Console.WriteLine("2. Gestion des usagers");
                Console.WriteLine("3. Gestion des emprunts");
                Console.WriteLine("4. Rapport: emprunts d'un usager");
                Console.WriteLine("0. Quitter");
                Console.Write("Choix: ");
                var choix = Console.ReadLine();
                switch (choix) {
                    case "1":
                        MenuLivres(livreService);
                        break;
                    case "2":
                        MenuUsagers(usagerService);
                        break;
                    case "3":
                        MenuEmprunts(empruntService, livreService, usagerService);
                        break;
                    case "4":
                        MenuRapport(empruntService, usagerService, livreService);
                        break;
                    case "0":
                        Console.WriteLine("Au revoir !");
                        return;
                    default:
                        Console.WriteLine("Choix invalide. Appuyez sur une touche pour continuer.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void MenuLivres(LivreService livreService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("*-*-*- GESTION DES LIVRES *-*-*-");
                Console.WriteLine("1. Lister les livres");
                Console.WriteLine("2. Lister les livres disponibles (Quantité > 0)");
                Console.WriteLine("3. Ajouter un livre");
                Console.WriteLine("4. Modifier un livre");
                Console.WriteLine("5. Supprimer un livre");
                Console.WriteLine("0. Retour");
                Console.Write("Choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherLivres(livreService.GetAll());
                        break;

                    case "2":
                        AfficherLivres(livreService.GetAvailableBooks());
                        break;

                    case "3":
                        AjouterLivre(livreService);
                        break;

                    case "4":
                        ModifierLivre(livreService);
                        break;

                    case "5":
                        SupprimerLivre(livreService);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Choix invalide.");
                        Pause();
                        break;
                }
            }
        }
        static void AfficherLivres(List<Livre> livres)
        {
            Console.Clear();
            Console.WriteLine("*-*-*-*-* LISTE DES LIVRES -*-*-*--*-");

            if (livres.Count == 0)
            {
                Console.WriteLine("Aucun livre.");
            }
            else
            {
                foreach (var l in livres)
                {
                    Console.WriteLine($"{l.IdLivre,3} | {l.Titre,-30} | {l.Auteur,-20} | Qte: {l.Quantite}");
                }
            }

            Pause();
        }
        static void AjouterLivre(LivreService livreService)
        {
            Console.Clear();
            Console.WriteLine("*-*-*-* AJOUT D'UN LIVRE *-*-*-");

            var livre = new Livre();

            Console.Write("Titre : ");
            livre.Titre = Console.ReadLine() ?? "";

            Console.Write("Auteur : ");
            livre.Auteur = Console.ReadLine() ?? "";

            Console.Write("Année (optionnel, vide = null) : ");
            var anneeStr = Console.ReadLine();
            if (int.TryParse(anneeStr, out int annee))
                livre.Annee = annee;

            Console.Write("ISBN : ");
            livre.ISBN = Console.ReadLine();

            Console.Write("Catégorie : ");
            livre.Categorie = Console.ReadLine();

            Console.Write("Quantité : ");
            livre.Quantite = LireEntierObligatoire();

            try
            {
                livreService.AjouterLivre(livre);
                Console.WriteLine("Livre ajouté avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }
        static void ModifierLivre(LivreService livreService)
        {
            Console.Clear();
            Console.WriteLine("-*-*-*- MODIFICATION D'UN LIVRE *-*-*-*-");
            Console.Write("Id du livre à modifier : ");

            int id = LireEntierObligatoire();

            var livres = livreService.GetAll();
            var livre = livres.Find(l => l.IdLivre == id);

            if (livre == null)
            {
                Console.WriteLine("Livre non trouvé.");
                Pause();
                return;
            }

            Console.WriteLine($"Livre actuel : {livre.Titre} ({livre.Auteur}), Qte : {livre.Quantite}");

            Console.Write($"Nouveau titre ({livre.Titre}) : ");
            var titre = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(titre))
                livre.Titre = titre;

            Console.Write($"Nouvel auteur ({livre.Auteur}) : ");
            var auteur = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(auteur))
                livre.Auteur = auteur;

            Console.Write($"Nouvelle quantité ({livre.Quantite}) : ");
            var qteStr = Console.ReadLine();
            if (int.TryParse(qteStr, out int qte))
                livre.Quantite = qte;

            try
            {
                livreService.ModifierLivre(livre);
                Console.WriteLine("Livre modifié avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }
        static void SupprimerLivre(LivreService livreService)
        {
            Console.Clear();
            Console.WriteLine("*-*-*-*- SUPPRESSION D'UN LIVRE *-*-*-*-");
            Console.Write("Id du livre à supprimer : ");

            int id = LireEntierObligatoire();

            try
            {
                livreService.SupprimerLivre(id);
                Console.WriteLine("Livre supprimé (et emprunts liés supprimés si nécessaire).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }
        static void MenuUsagers(UsagerService usagerService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("*-*-*-*-- GESTION DES USAGERS *-*-*-*");
                Console.WriteLine("1. Lister les usagers");
                Console.WriteLine("2. Rechercher un usager par nom");
                Console.WriteLine("3. Ajouter un usager");
                Console.WriteLine("4. Modifier un usager");
                Console.WriteLine("5. Supprimer un usager");
                Console.WriteLine("0. Retour");
                Console.Write("Choix : ");

                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherUsagers(usagerService.GetAll());
                        break;

                    case "2":
                        RechercherUsager(usagerService);
                        break;

                    case "3":
                        AjouterUsager(usagerService);
                        break;

                    case "4":
                        ModifierUsager(usagerService);
                        break;

                    case "5":
                        SupprimerUsager(usagerService);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Choix invalide.");
                        Pause();
                        break;
                }
            }
        }
        static void AfficherUsagers(List<Usage> usagers)
        {
            Console.Clear();
            Console.WriteLine("*-*-* LISTE DES USAGERS *-*-*");

            if (usagers.Count == 0)
            {
                Console.WriteLine("Aucun usager.");
            }
            else
            {
                foreach (var u in usagers)
                {
                    Console.WriteLine($"{u.IdUsager,3} | {u.Nom,-25} | {u.Email,-25} | {u.Telephone}");
                }
            }

            Pause();
        }
        static void RechercherUsager(UsagerService usagerService)
        {
            Console.Clear();
            Console.WriteLine("*-*-* RECHERCHE D'USAGER PAR NOM *-*-*");
            Console.Write("Nom ou partie du nom : ");
            var nom = Console.ReadLine() ?? "";

            var resultats = usagerService.RechercherParNom(nom);
            AfficherUsagers(resultats);
        }
        static void AjouterUsager(UsagerService usagerService)
        {
            Console.Clear();
            Console.WriteLine("*-*-*- AJOUT D'UN USAGER *-*-*-");

            var u = new Usage();

            Console.Write("Nom : ");
            u.Nom = Console.ReadLine() ?? "";

            Console.Write("Email : ");
            u.Email = Console.ReadLine() ?? "";

            Console.Write("Téléphone (optionnel) : ");
            u.Telephone = Console.ReadLine();

            try
            {
                usagerService.AjouterUsager(u);
                Console.WriteLine("Usager ajouté avec succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }
        static void ModifierUsager(UsagerService usagerService)
        {
            Console.Clear();
            Console.WriteLine("=== MODIFICATION D'UN USAGER ===");
            Console.Write("Id de l'usager a modifier : ");

            int id = LireEntierObligatoire();

            var usager = usagerService.GetById(id);
            if (usager == null)
            {
                Console.WriteLine("Usager non trouve.");
                Pause();
                return;
            }

            Console.WriteLine($"Usager actuel : {usager.Nom} ({usager.Email})");

            Console.Write($"Nouveau nom ({usager.Nom}) : ");
            var nom = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nom))
                usager.Nom = nom;

            Console.Write($"Nouvel email ({usager.Email}) : ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                usager.Email = email;

            Console.Write($"Nouveau téléphone ({usager.Telephone}) : ");
            var tel = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tel))
                usager.Telephone = tel;

            try
            {
                usagerService.ModifierUsager(usager);
                Console.WriteLine("Usager modifié avec succes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }

        static void SupprimerUsager(UsagerService usagerService)
        {
            Console.Clear();
            Console.WriteLine("*-*--*-*- SUPPRESSION D'UN USAGER *-*-*-*-");
            Console.Write("Id de l'usager à supprimer : ");

            int id = LireEntierObligatoire();

            try
            {
                usagerService.SupprimerUsager(id);
                Console.WriteLine("Usager supprimé (et emprunts liés supprimés si configuré au niveau DB).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }

        //  MENU EMPRUNTS
        // -------------------------------------------------------------------
        static void MenuEmprunts(
            EmpruntService empruntService,
            LivreService livreService,
            UsagerService usagerService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-*-*-* GESTION DES EMPRUNTS -*-*-*");
                Console.WriteLine("1. Lister tous les emprunts");
                Console.WriteLine("2. Enregistrer un nouvel emprunt");
                Console.WriteLine("3. Modifier un emprunt");
                Console.WriteLine("4. Supprimer un emprunt");
                Console.WriteLine("5. Retourner un emprunt (rendre le livre)");
                Console.WriteLine("0. Retour");
                Console.Write("Choix : ");
                var choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherEmprunts(empruntService.GetAll());
                        break;

                    case "2":
                        EnregistrerEmprunt(empruntService, livreService, usagerService);
                        break;

                    case "3":
                        ModifierEmprunt(empruntService);
                        break;

                    case "4":
                        SupprimerEmprunt(empruntService);
                        break;

                    case "5":
                        RetournerEmprunt(empruntService);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Choix invalide.");
                        Pause();
                        break;
                }
            }
        }

        static void AfficherEmprunts(List<Emprunt> emprunts)
        {
            Console.Clear();
            Console.WriteLine("*--*-*-* LISTE DES EMPRUNTS -*-*-*-");

            if (emprunts.Count == 0)
            {
                Console.WriteLine("Aucun emprunt.");
            }
            else
            {
                foreach (var e in emprunts)
                {
                    Console.WriteLine($"{e.IdEmprunt,3} | Livre: {e.IdLivre,3} | Usager: {e.IdUsager,3} | " +
                                      $"Du {e.DateEmprunt:yyyy-MM-dd} au {e.DateRetourPrevue:yyyy-MM-dd}");
                }
            }

            Pause();
        }
        static void EnregistrerEmprunt(
           EmpruntService empruntService,
           LivreService livreService,
           UsagerService usagerService)
        {
            Console.Clear();
            Console.WriteLine("=== ENREGISTRER UN NOUVEL EMPRUNT ===");

            Console.WriteLine("Liste des livres disponibles :");
            AfficherLivres(livreService.GetAvailableBooks());

            Console.Write("Id du livre à emprunter : ");
            int idLivre = LireEntierObligatoire();

            Console.WriteLine("Liste des usagers :");
            AfficherUsagers(usagerService.GetAll());

            Console.Write("Id de l'usager : ");
            int idUsager = LireEntierObligatoire();

            Console.Write("Date d'emprunt (yyyy-MM-dd) : ");
            var dateEmprunt = LireDateObligatoire();

            Console.Write("Date de retour prévue (yyyy-MM-dd) : ");
            var dateRetour = LireDateObligatoire();

            var emprunt = new Emprunt
            {
                IdLivre = idLivre,
                IdUsager = idUsager,
                DateEmprunt = dateEmprunt,
                DateRetourPrevue = dateRetour
            };

            try
            {
                empruntService.EnregistrerEmprunt(emprunt);
                Console.WriteLine("Emprunt enregistré avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }

        static void ModifierEmprunt(EmpruntService empruntService)
        {
            Console.Clear();
            Console.WriteLine("=== MODIFICATION D'UN EMPRUNT ===");
            Console.Write("Id de l'emprunt : ");

            int id = LireEntierObligatoire();

            var emprunt = empruntService.GetById(id);
            if (emprunt == null)
            {
                Console.WriteLine("Emprunt introuvable.");
                Pause();
                return;
            }

            Console.WriteLine($"Emprunt actuel : Livre {emprunt.IdLivre}, Usager {emprunt.IdUsager}, " +
                              $"Du {emprunt.DateEmprunt:yyyy-MM-dd} au {emprunt.DateRetourPrevue:yyyy-MM-dd}");

            Console.Write($"Nouvelle date d'emprunt ({emprunt.DateEmprunt:yyyy-MM-dd}) : ");
            var dateEmpruntStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateEmpruntStr) &&
                DateTime.TryParse(dateEmpruntStr, out var newDateEmprunt))
            {
                emprunt.DateEmprunt = newDateEmprunt;
            }

            Console.Write($"Nouvelle date de retour prévue ({emprunt.DateRetourPrevue:yyyy-MM-dd}) : ");
            var dateRetourStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateRetourStr) &&
                DateTime.TryParse(dateRetourStr, out var newDateRetour))
            {
                emprunt.DateRetourPrevue = newDateRetour;
            }

            try
            {
                empruntService.ModifierEmprunt(emprunt);
                Console.WriteLine("Emprunt modifié avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }

        static void SupprimerEmprunt(EmpruntService empruntService)
        {
            Console.Clear();
            Console.WriteLine("=== SUPPRESSION D'UN EMPRUNT ===");
            Console.Write("Id de l'emprunt : ");

            int id = LireEntierObligatoire();

            try
            {
                empruntService.SupprimerEmprunt(id);
                Console.WriteLine("Emprunt supprimé.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }

        static void RetournerEmprunt(EmpruntService empruntService)
        {
            Console.Clear();
            Console.WriteLine("=== RETOUR D'UN EMPRUNT (RENDU DU LIVRE) ===");
            Console.Write("Id de l'emprunt : ");

            int id = LireEntierObligatoire();

            try
            {
                empruntService.RetournerEmprunt(id);
                Console.WriteLine("Emprunt retourné, quantité du livre mise à jour.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }

            Pause();
        }

        static void MenuRapport(
                    EmpruntService empruntService,
                    UsagerService usagerService,
                    LivreService livreService)
        {
            Console.Clear();
            Console.WriteLine("=== RAPPORT : EMPRUNTS D'UN USAGER ===");

            Console.WriteLine("Liste des usagers :");
            AfficherUsagers(usagerService.GetAll());

            Console.Write("Id de l'usager pour le rapport : ");
            int idUsager = LireEntierObligatoire();

            var usager = usagerService.GetById(idUsager);
            if (usager == null)
            {
                Console.WriteLine("Usager introuvable.");
                Pause();
                return;
            }

            var emprunts = empruntService.GetEmpruntsParUsager(idUsager);

            Console.Clear();
            Console.WriteLine($"=== EMPRUNTS POUR {usager.Nom} ===");

            if (emprunts.Count == 0)
            {
                Console.WriteLine("Aucun emprunt pour cet usager.");
            }
            else
            {
                foreach (var e in emprunts)
                {
                    // Option simple : afficher IdLivre
                    Console.WriteLine($"Emprunt {e.IdEmprunt,3} | Livre {e.IdLivre,3} | " +
                                      $"Du {e.DateEmprunt:yyyy-MM-dd} au {e.DateRetourPrevue:yyyy-MM-dd}");
                }
            }

            Pause();
        }

        // =====================================================================
        //  FONCTIONS UTILITAIRES
        // =====================================================================

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static int LireEntierObligatoire()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int valeur))
                    return valeur;

                Console.Write("Valeur invalide, réessayez : ");
            }
        }

        static DateTime LireDateObligatoire()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (DateTime.TryParse(input, out var date))
                    return date;

                Console.Write("Date invalide, format attendu (yyyy-MM-dd), réessayez : ");
            }
        }

    }
}
        