using BibliothequeApp.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.DataAccess
{
    public class SqlLivreRepository: ILivreRepository
    {
        public void Add(Livre entity)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"INSERT INTO Livres (Titre, Auteur, Annee, ISBN, Categorie, Quantite)
                           VALUES (@Titre, @Auteur, @Annee, @ISBN, @Categorie, @Quantite);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Titre", entity.Titre);
            cmd.Parameters.AddWithValue("@Auteur", entity.Auteur);
            cmd.Parameters.AddWithValue("@Annee", (object?)entity.Annee ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ISBN", (object?)entity.ISBN ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Categorie", (object?)entity.Categorie ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Quantite", entity.Quantite);

            cmd.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = "DELETE FROM Emprunts WHERE IdLivre = @Id; DELETE FROM Livres WHERE IdLivre = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
        public List<Livre> GetAll()
        {
            var list = new List<Livre>();

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = "SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, Quantite FROM Livres;";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var livre = new Livre
                {
                    IdLivre = reader.GetInt32(0),
                    Titre = reader.GetString(1),
                    Auteur = reader.GetString(2),
                    Annee = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    ISBN = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Categorie = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Quantite = reader.GetInt32(6)
                };
                list.Add(livre);
            }

            return list;
        }
        public Livre? GetById(int id)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, Quantite
                           FROM Livres WHERE IdLivre = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Livre
                {
                    IdLivre = reader.GetInt32(0),
                    Titre = reader.GetString(1),
                    Auteur = reader.GetString(2),
                    Annee = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    ISBN = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Categorie = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Quantite = reader.GetInt32(6)
                };
            }

            return null;
        }
        public void Update(Livre entity)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"UPDATE Livres
                           SET Titre = @Titre,
                               Auteur = @Auteur,
                               Annee = @Annee,
                               ISBN = @ISBN,
                               Categorie = @Categorie,
                               Quantite = @Quantite
                           WHERE IdLivre = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.IdLivre);
            cmd.Parameters.AddWithValue("@Titre", entity.Titre);
            cmd.Parameters.AddWithValue("@Auteur", entity.Auteur);
            cmd.Parameters.AddWithValue("@Annee", (object?)entity.Annee ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ISBN", (object?)entity.ISBN ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Categorie", (object?)entity.Categorie ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Quantite", entity.Quantite);

            cmd.ExecuteNonQuery();
        }
        public List<Livre> GetAvailableBooks()
        {
            var result = new List<Livre>();

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdLivre, Titre, Auteur, Annee, ISBN, Categorie, Quantite
                           FROM Livres
                           WHERE Quantite > 0;";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Livre
                {
                    IdLivre = reader.GetInt32(0),
                    Titre = reader.GetString(1),
                    Auteur = reader.GetString(2),
                    Annee = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    ISBN = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Categorie = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Quantite = reader.GetInt32(6)
                });
            }

            return result;
        }
    }
}
