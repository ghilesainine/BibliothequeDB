using BibliothequeApp.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliothequeApp.DataAccess
{
    public class SqlUsageRepository : IUsagerRepository
    {
        public void Add(Usage entity)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"INSERT INTO Usagers (Nom, Email, Telephone)
VALUES (@Nom, @Email, @Telephone);";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nom", entity.Nom);
            cmd.Parameters.AddWithValue("@Email", entity.Email);
            cmd.Parameters.AddWithValue("@Telephone", (object?)entity.Telephone ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"DELETE FROM Emprunts WHERE IdUsager = @Id;
DELETE FROM Usagers WHERE IdUsager = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public List<Usage> GetAll()
        {
            var list = new List<Usage>();
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdUsager, Nom, Email, Telephone FROM Usagers;";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Usage
                {
                    IdUsager = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Email = reader.GetString(2),
                    Telephone = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }

            return list;
        }

        public Usage? GetById(int id)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdUsager, Nom, Email, Telephone
FROM Usagers WHERE IdUsager = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usage
                {
                    IdUsager = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Email = reader.GetString(2),
                    Telephone = reader.IsDBNull(3) ? null : reader.GetString(3)
                };
            }

            return null;
        }

        public void Update(Usage entity)
        {
            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"UPDATE Usagers
SET Nom = @Nom,
Email = @Email,
Telephone = @Telephone
WHERE IdUsager = @Id;";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", entity.IdUsager);
            cmd.Parameters.AddWithValue("@Nom", entity.Nom);
            cmd.Parameters.AddWithValue("@Email", entity.Email);
            cmd.Parameters.AddWithValue("@Telephone", (object?)entity.Telephone ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public List<Usage> SearchByName(string name)
        {
            var list = new List<Usage>();

            using var conn = ConnectionFactory.CreateConnection();
            conn.Open();

            string sql = @"SELECT IdUsager, Nom, Email, Telephone
FROM Usagers
WHERE Nom LIKE '%' + @Name + '%';";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", name);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Usage
                {
                    IdUsager = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Email = reader.GetString(2),
                    Telephone = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }

            return list;
        }
    }
}
