using System.Linq.Expressions;
using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers;

[ApiController]
[Route("api/korisnici")]
public class KorisniciController : ControllerBase
{   private readonly UserDbRepository _dbRepository;

    public KorisniciController(UserDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }
    [HttpGet]
    public ActionResult<List<Korisnik>>DobaviSve(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if(page < 1 || pageSize < 1)
        {
            return BadRequest("Broj stranice i veličina stranice moraju biti veći od nule.");
        }

        try
        {
            List<Korisnik> korisnici = _dbRepository.GetAll(page,pageSize);
            long totalCount = _dbRepository.CountAll();

            return Ok(new
            {
                Data = korisnici,totalCount = totalCount
            });
        }
        catch(SqliteException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem(statusCode: 500, detail: "Greška prilikom pristupanja bazi.");
        }
        catch(FormatException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem(statusCode: 500, detail: "Podaci nisu u dobrom formatu.");
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine(ex);
            return Problem(statusCode: 500, detail: "Došlo je do greške pri obradi korisnika.");
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