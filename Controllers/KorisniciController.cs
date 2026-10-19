using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers;

[ApiController]
[Route("api/korisnici")]
public class KorisniciController : ControllerBase
{
    private readonly KorisnikRepository _repository;

    public KorisniciController(KorisnikRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<List<Korisnik>>DobaviSve()
    {
        return Ok(_repository.DobaviSve());
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