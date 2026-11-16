using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace DrustvenaMreza.Controllers;

[ApiController]
[Route("api/korisnici")]
public class KorisniciController : ControllerBase
{
    private readonly KorisnikRepository _repository;
    private readonly string _connectionString;

    private List<Korisnik> GetAllFromDatabase()
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

    public KorisniciController(KorisnikRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;

        string putanjaBaze = Path.Combine(environment.ContentRootPath,"database","mydatabase.db");
    
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource=putanjaBaze
        }.ToString();
    }
    [HttpGet]
    public ActionResult<List<Korisnik>>DobaviSve()
    {
        return Ok(GetAllFromDatabase());
    }
    //Dobavljanje korisnika po ID-u
    [HttpGet("{id:int}")]
    public ActionResult<Korisnik> DobaviPoId(int id)
    {
        Korisnik? korisnik = _repository.DobaviPoId(id);
        if(korisnik == null)
        {
            return NotFound();
        }
        return Ok(korisnik);
    }
    //Kreiranje korisnika
    [HttpPost]
    public ActionResult<Korisnik>Kreiraj(Korisnik korisnik)
    {
        Korisnik kreiraniKorisnik = _repository.Kreiraj(korisnik);
        return CreatedAtAction(
            nameof(DobaviPoId),
            new {id = kreiraniKorisnik.Id},
            kreiraniKorisnik
        );
    }
    //Azuriranje korisnika
    [HttpPut("{id:int}")]
    public IActionResult Azurija(int id, Korisnik korisnik)
    {
        bool uspesno = _repository.Azuriraj(id, korisnik);
        if(!uspesno)
        {
            return NotFound();
        }
        return NoContent();
    }
}