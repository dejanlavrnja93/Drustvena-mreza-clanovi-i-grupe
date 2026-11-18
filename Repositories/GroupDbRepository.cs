using DrustvenaMreza.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
namespace DrustvenaMreza.Repositories
{
    public class GroupDbRepository
    {
        private const string connectionString = "Data Source=Data/mydatabase.db";

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
    }
}
