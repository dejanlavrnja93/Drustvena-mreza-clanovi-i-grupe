using DrustvenaMreza.Models;
using System.Data.Common;
using System.Globalization;

namespace DrustvenaMreza.Repositories;

public class KorisnikRepository
{
    private readonly string _putanja;
    public KorisnikRepository(IWebHostEnvironment environment)
    {
        _putanja = Path.Combine(environment.ContentRootPath, "Data", "korisnici.csv");
        //Relativna putanja _putanja = "Data/korisnici.csv";
    }

    //1.Dobavi podatke o svim korisnicima
    public List<Korisnik> DobaviSve()
    {
        List<Korisnik> korisnici = new List<Korisnik>();

        foreach (string red in File.ReadAllLines(_putanja))
        {
            if (string.IsNullOrWhiteSpace(red))
            {
                continue;
            }

            string[] podaci = red.Split(',');
            Korisnik korisnik = new Korisnik
            {
                Id = int.Parse(podaci[0]),
                KorisnickoIme = podaci[1],
                Ime = podaci[2],
                Prezime = podaci[3],
                DatumRodjenja = DateOnly.ParseExact(podaci[4], "yyyy-MM-dd", CultureInfo.InvariantCulture)
            };
            korisnici.Add(korisnik);
        }
        return korisnici;
    }
    //2.Dobavi podatke o jednom odabranom korisniku
    public Korisnik? DobaviPoId(int id)
    {
        List<Korisnik> korisnici = DobaviSve();
        foreach (Korisnik korisnik in korisnici)
        {
            if(korisnik.Id == id)
            {
                return korisnik;
            }
        }
        return null;
    }
    //Pomocna metoda za cuvanje korisnika
    private void SacuvajSve(List<Korisnik> korisnici)
    {
        List<string> redovi = new List<string>();
        foreach (Korisnik korisnik in korisnici)
        {
            string red = string.Join(",",
                korisnik.Id.ToString(CultureInfo.InvariantCulture),
                korisnik.KorisnickoIme,
                korisnik.Ime,
                korisnik.Prezime,
                korisnik.DatumRodjenja.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture));
            redovi.Add(red);
        }
        File.WriteAllLines(_putanja, redovi);
    }
    //3.Kreirati novog korisnika
    public Korisnik Kreiraj (Korisnik korisnik)
    {
        List<Korisnik> korisnici = DobaviSve();
        int najveciId = 0;
        foreach (Korisnik postojeciKorisnik in korisnici)
        {
            if (postojeciKorisnik.Id > najveciId)
            {
                najveciId = postojeciKorisnik.Id;
            }
        }
        korisnik.Id = najveciId + 1;
        korisnici.Add (korisnik);
        SacuvajSve(korisnici);

        return korisnik;
    }
    //4.Azurirati postojecih korisnika
    public bool Azuriraj(int id, Korisnik noviPodaci)
    {
        List<Korisnik> korisnici = DobaviSve();
        foreach (Korisnik korisnik in korisnici)
        {
            if(korisnik.Id == id)
            {
                korisnik.KorisnickoIme = noviPodaci.KorisnickoIme;
                korisnik.Ime = noviPodaci.Ime;
                korisnik.Prezime = noviPodaci.Prezime;
                korisnik.DatumRodjenja = noviPodaci.DatumRodjenja;

                SacuvajSve(korisnici);

                return true;
            }
        }

        return false;
    }
}