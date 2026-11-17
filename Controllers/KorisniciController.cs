using System.Linq.Expressions;
using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers;

[ApiController]
[Route("api/korisnici")]
public class KorisniciController : ControllerBase
{
    private readonly KorisnikRepository _repository;
    private readonly UserDbRepository _dbRepository;

    public KorisniciController(KorisnikRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _dbRepository = new UserDbRepository(environment);
    }
    [HttpGet]
    public ActionResult<List<Korisnik>>DobaviSve()
    {
        try
        {
            return Ok(_dbRepository.GetAll());
        }
        catch(SqliteException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem (statusCode:500, detail:"Greska prilikom pristupanja bazi");
        }
        catch (FormatException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem (statusCode:500, detail:"Podaci nisu u dobrom formatu");
        }
    }
    //Dobavljanje korisnika po ID-u
    [HttpGet("{id:int}")]
    public ActionResult<Korisnik> DobaviPoId(int id)
    {
        try
        {
            Korisnik? korisnik = _dbRepository.GetById(id);
            if(korisnik == null)
            {
                return NotFound("Korisnik nije pronadjen");
            }
            return Ok(korisnik);
        }
        catch(SqliteException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem (statusCode:500, detail:"Greska prilikom pristupanja bazi");
        }
        catch(FormatException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem (statusCode:500, detail:"Podaci nisu u dobrom formatu");
        }
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