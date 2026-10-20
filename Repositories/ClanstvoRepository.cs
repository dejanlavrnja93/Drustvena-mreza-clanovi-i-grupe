//Repozitorijum za citanje clanstava
using DrustvenaMreza.Models;

namespace DrustvenaMreza.Repositories;

public class ClanstvoRepository
{
    private readonly string _putanja;

    public ClanstvoRepository(IWebHostEnvironment environment)
    {
        _putanja = Path.Combine(environment.ContentRootPath,"Data", "clanstva.csv");
    }
    public List<Clanstvo> DobaviSve()
    {
        List<Clanstvo> clanstva = new List<Clanstvo>();
        foreach (string red in File.ReadAllLines(_putanja))
        {
            if(string.IsNullOrWhiteSpace(red))
            {
                continue;
            }
            string[] podaci = red.Split(',');
            Clanstvo clanstvo = new Clanstvo
            {
                KorisnikId = int.Parse(podaci[0]),
                GrupaId = int.Parse(podaci[1])
            };
            clanstva.Add(clanstvo);
        };
        return clanstva;
    }
    //Metoda za izdvajanje clanstva jedne grupe
    public List<Clanstvo> DobaviPoGrupi(int grupaId)
    {
        List<Clanstvo> clanstva = DobaviSve();
        List<Clanstvo> clanstvaGrupe = new List<Clanstvo>();

        foreach (Clanstvo clanstvo in clanstva)
        {
            if(clanstvo.GrupaId == grupaId)
            {
                clanstvaGrupe.Add(clanstvo);
            }
        }
        return clanstvaGrupe;
    }
}