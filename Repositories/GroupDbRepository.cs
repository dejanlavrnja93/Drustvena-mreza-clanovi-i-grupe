using DrustvenaMreza.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
namespace DrustvenaMreza.Repositories
{
    public class GroupDbRepository
    {
        private readonly string connectionString;

        public GroupDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Grupa> GetAll()
        {
            List<Grupa> grupe = new List<Grupa>();



            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = "SELECT Id, Name, CreationDate FROM Groups";
            using SqliteCommand command = new SqliteCommand(query, connection);
            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["Id"]);
                string name = reader["Name"].ToString();
                DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);

                Grupa grupa = new Grupa(id, name, creationDate);
                grupe.Add(grupa);

            }
            connection.Close();
            return grupe;



        }

        public Grupa? GetById(int id) 
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = "SELECT Id, Name, CreationDate FROM Groups WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                int groupId = Convert.ToInt32(reader["Id"]);
                string name = reader["Name"].ToString();
                DateTime creationDate = Convert.ToDateTime(reader["CreationDate"]);

                return new Grupa(groupId, name, creationDate);
            }

            connection.Close();
            return null;
        }

        public Grupa Create(Grupa grupa)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = @"INSERT INTO Groups (Name, CreationDate) VALUES (@Name, @CreationDate); SELECT last_insert_rowid();";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", grupa.ImeGrupe);
            command.Parameters.AddWithValue("@CreationDate", grupa.DatumOsnivanja);
            int newId = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            grupa.Id = newId;
            return new Grupa(newId, grupa.ImeGrupe, grupa.DatumOsnivanja);
        }

        public void Delete(int id)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = "DELETE FROM Groups WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            connection.Close();
            
        }

        public void Update(Grupa grupa)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            string query = "UPDATE Groups SET Name = @Name, CreationDate = @CreationDate WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", grupa.ImeGrupe);
            command.Parameters.AddWithValue("@CreationDate", grupa.DatumOsnivanja);
            command.Parameters.AddWithValue("@Id", grupa.Id);
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
