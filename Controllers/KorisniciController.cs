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
        try
        {
            Korisnik kreiraniKorisnik = _dbRepository.Create(korisnik);
            return CreatedAtAction(nameof(DobaviPoId),
                                    new {id = kreiraniKorisnik.Id},
                                    kreiraniKorisnik);
        }
        catch (SqliteException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem(
                statusCode:500,
                detail:"Greška prilikom čuvanja korisnika u bazi."
            );
        }
        catch (OverflowException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem(statusCode:500,
                            detail:"Identifikator korisnika je izvan podržanog opsega.");
        }
    }
    //Azuriranje korisnika
    [HttpPut("{id:int}")]
    public IActionResult Azurija(int id, Korisnik korisnik)
    {
       if(id != korisnik.Id)
        {
            return BadRequest( "ID u adresi mora odgovarati ID-ju korisnika.");
        }
        try
        {
            bool uspesno = _dbRepository.Update(korisnik);

            if(!uspesno)
            {
                return NotFound("Korisnik nije pronađen.");
            }
            
            return NoContent();
        }
        catch(SqliteException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem(statusCode:500,
            detail: "Greška prilikom izmene korisnika u bazi.");
        }
    }
    [HttpDelete("{id:int}")]
    public IActionResult Obrisi(int id)
    {
        try
        {
            bool uspesno = _dbRepository.Delete(id);
            if(!uspesno)
            {
                return NotFound("Korisnik nije pronađen.");
            }
            return NoContent();
        }
        catch (SqliteException ex)
        {
            Console.Error.WriteLine(ex);

            return Problem(
                statusCode:500,
                detail:"Greška prilikom brisanja korisnika iz baze."
            );
        }
    }
}