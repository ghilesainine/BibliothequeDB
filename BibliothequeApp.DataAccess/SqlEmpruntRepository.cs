using BibliothequeApp.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.DataAccess
{
    public class SqlEmpruntRepository : IEmpruntRepository
    {
        public void Add(Emprunt entity)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"INSERT INTO Emprunts
(DateEmprunt, DateRetourPrevue, IdLivre, IdUsager)
VALUES (@DateEmprunt, @DateRetourPrevue, @IdLivre, @IdUsager);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DateEmprunt", entity.DateEmprunt);
            cmd.Parameters.AddWithValue("@DateRetourPrevue", entity.DateRetourPrevue);
            cmd.Parameters.AddWithValue("@IdLivre", entity.IdLivre);
            cmd.Parameters.AddWithValue("@IdUsager", entity.IdUsager);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"DELETE FROM Emprunts WHERE IdEmprunt = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public List<Emprunt> GetAll()
        {
            var list = new List<Emprunt>();

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdEmprunt, DateEmprunt, DateRetourPrevue, IdLivre, IdUsager
FROM Emprunts;";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Emprunt
                {
                    IdEmprunt = reader.GetInt32(0),
                    DateEmprunt = reader.GetDateTime(1),
                    DateRetourPrevue = reader.GetDateTime(2),
                    IdLivre = reader.GetInt32(3),
                    IdUsager = reader.GetInt32(4)
                });
            }

            return list;
        }

        public Emprunt? GetById(int id)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdEmprunt, DateEmprunt, DateRetourPrevue, IdLivre, IdUsager
FROM Emprunts WHERE IdEmprunt = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Emprunt
                {
                    IdEmprunt = reader.GetInt32(0),
                    DateEmprunt = reader.GetDateTime(1),
                    DateRetourPrevue = reader.GetDateTime(2),
                    IdLivre = reader.GetInt32(3),
                    IdUsager = reader.GetInt32(4)
                };
            }

            return null;
        }

        public void Update(Emprunt entity)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"UPDATE Emprunts
SET DateEmprunt = @DateEmprunt,
DateRetourPrevue = @DateRetourPrevue,
IdLivre = @IdLivre,
IdUsager = @IdUsager
WHERE IdEmprunt = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.IdEmprunt);
            cmd.Parameters.AddWithValue("@DateEmprunt", entity.DateEmprunt);
            cmd.Parameters.AddWithValue("@DateRetourPrevue", entity.DateRetourPrevue);
            cmd.Parameters.AddWithValue("@IdLivre", entity.IdLivre);
            cmd.Parameters.AddWithValue("@IdUsager", entity.IdUsager);

            cmd.ExecuteNonQuery();
        }

        public List<Emprunt> GetByUsager(int idUsager)
        {
            var list = new List<Emprunt>();

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdEmprunt, DateEmprunt, DateRetourPrevue, IdLivre, IdUsager
FROM Emprunts
WHERE IdUsager = @IdUsager;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdUsager", idUsager);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Emprunt
                {
                    IdEmprunt = reader.GetInt32(0),
                    DateEmprunt = reader.GetDateTime(1),
                    DateRetourPrevue = reader.GetDateTime(2),
                    IdLivre = reader.GetInt32(3),
                    IdUsager = reader.GetInt32(4)
                });
            }

            return list;
        }
    }
}
