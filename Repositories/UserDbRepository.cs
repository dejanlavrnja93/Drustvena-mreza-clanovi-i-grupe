using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace DrustvenaMreza.Repositories;

public class UserDbRepository
{
    private readonly string _connectionString;

    public UserDbRepository(IWebHostEnvironment environment)
    {
        string putanjaBaze = Path.Combine(environment.ContentRootPath, "Data", "mydatabase.db");
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource=putanjaBaze,
            Mode= SqliteOpenMode.ReadWrite
        }.ToString();
    }
    public List<Korisnik> GetAll()
    {
        List<Korisnik>korisnici = new List<Korisnik>();
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, Username, Name, Surname, Birthday
            FROM Users
            ORDER BY Id;
            """;
        using SqliteDataReader reader = command.ExecuteReader();
        while(reader.Read())
        {
            korisnici.Add(new Korisnik
            {
                Id = reader.GetInt32(0),
                KorisnickoIme = reader.GetString(1),
                Ime = reader.GetString(2),
                Prezime = reader.GetString(3),
                DatumRodjenja = DateOnly.ParseExact(
                    reader.GetString(4),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture
                )
            });
        }
        return korisnici;
    }

    public Korisnik? GetById(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        //WHERE Id = @id izdvaja trazenog korisnika, AddWithValue prosledjuje ID kao SQL parametar !!!
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = """
        SELECT Id, Username, Name, Surname, Birthday
        FROM Users
        WHERE Id = @id 
        """;
        command.Parameters.AddWithValue("@id",id);

        using SqliteDataReader reader = command.ExecuteReader();
        if(!reader.Read())
        {
            return null;
        }
        return new Korisnik()
        {
            Id = reader.GetInt32(0),
            KorisnickoIme = reader.GetString(1),
            Ime = reader.GetString(2),
            Prezime = reader.GetString(3),
            DatumRodjenja = DateOnly.ParseExact(
                reader.GetString(4),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture
            )
        };
    }
}