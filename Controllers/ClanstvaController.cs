using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers;

[ApiController]
[Route("api/clanstva")]

public class ClanstvaContoller : ControllerBase
{
    private ClanstvoRepository clanstvoRepository = new ClanstvoRepository();
    private GrupaRepository grupaRepository = new GrupaRepository();
    private KorisnikRepository korisnikRepository;

    public ClanstvaContoller(IWebHostEnvironment environment)
    {
        korisnikRepository = new KorisnikRepository(environment);
    }

    [HttpGet("{grupaId:int}")]
    public ActionResult<List<Korisnik>> DobaviKorisnikeGrupe(int grupaId)
    {
        List<Clanstvo> clanstva = ClanstvoRepository.Data;

        List<Korisnik> korisnici = new List<Korisnik>();

        if(!GrupaRepository.Data.ContainsKey(grupaId))
        {
            return NotFound();
        }
        List<Korisnik> sviKorisnici = korisnikRepository.DobaviSve();
        

        foreach (Clanstvo clanstvo in clanstva)
        {
           if(clanstvo.GrupaId == grupaId)
            {
               foreach(Korisnik korisnik in sviKorisnici)
                {
                    if (korisnik.Id == clanstvo.KorisnikId)
                    {
                        korisnici.Add(korisnik);
                        break;
                    }
                }
            }
        }
        return Ok(korisnici);
    }
    [HttpPut("{grupaId:int}/korisnici/{korisnikId:int}")]
    public ActionResult DodajKorisnikaUGrupu(int grupaId, int korisnikId)
    {
        if (!GrupaRepository.Data.ContainsKey(grupaId))
        {
            return NotFound();
        }
        Korisnik? korisnik = korisnikRepository.DobaviPoId(korisnikId);

        if (korisnik == null)
        {
            return NotFound();
        }

        foreach(Clanstvo clanstvo in ClanstvoRepository.Data)
        {
            if (clanstvo.GrupaId == grupaId && clanstvo.KorisnikId == korisnikId)
            {
                return Conflict("Korisnik je već član grupe.");
            }
        }

        Clanstvo novoClanstvo = new Clanstvo
        {
            GrupaId = grupaId,
            KorisnikId = korisnikId
        };
        ClanstvoRepository.Data.Add(novoClanstvo);
        clanstvoRepository.Save();
        return Ok();
    }

    [HttpDelete("{grupaId:int}/korisnici/{korisnikId:int}")]
    public ActionResult UkloniKorisnikaIzGrupe(int grupaId, int korisnikId)
    {
        if (!GrupaRepository.Data.ContainsKey(grupaId))
        {
            return NotFound();
        }

        Korisnik? korisnik = korisnikRepository.DobaviPoId(korisnikId);
        if (korisnik == null)
        {
            return NotFound();
        }

        Clanstvo? clanstvoZaBrisanje = null;

        foreach (Clanstvo clanstvo in ClanstvoRepository.Data)
        {
            if (clanstvo.GrupaId == grupaId && clanstvo.KorisnikId == korisnikId)
            {
                clanstvoZaBrisanje = clanstvo;
                break;
            }
        }


        if (clanstvoZaBrisanje == null)
        {
            return NotFound();
        }
        ClanstvoRepository.Data.Remove(clanstvoZaBrisanje);
        clanstvoRepository.Save();
        return Ok();
    }
}
