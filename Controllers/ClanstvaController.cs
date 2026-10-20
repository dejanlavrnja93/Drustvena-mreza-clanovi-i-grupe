using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers;

[ApiController]
[Route("api/clanstvo")]

public class ClanstvaContoller : ControllerBase
{
    private readonly ClanstvoRepository _clanstvoRepository;
    private readonly KorisnikRepository _korisnikRepository;

    public ClanstvaContoller(
        ClanstvoRepository clanstvoRepository,
        KorisnikRepository korisnikRepository)
    {
        _clanstvoRepository = clanstvoRepository;
        _korisnikRepository = korisnikRepository;
    }
    [HttpGet("{grupaId:int}/korisnici")]
    public ActionResult <List<Korisnik>> DobaviKorisnikeGrupe(int grupaId)
    {
        List <Clanstvo> clanstva =
        _clanstvoRepository.DobaviPoGrupi(grupaId);

        List<Korisnik> korisnici = new List<Korisnik>();
        foreach(Clanstvo clanstvo in clanstva)
        {
            Korisnik? korisnik = _korisnikRepository.DobaviPoId(clanstvo.KorisnikId);
            if (korisnik!=null)
            {
                korisnici.Add(korisnik);
            }
        }
        return Ok(korisnici);
    }
}