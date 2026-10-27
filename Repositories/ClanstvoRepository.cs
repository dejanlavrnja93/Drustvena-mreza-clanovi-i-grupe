//Repozitorijum za citanje clanstava
using DrustvenaMreza.Models;

namespace DrustvenaMreza.Repositories;

public class ClanstvoRepository
{

    private const string filePath = "Data/clanstva.csv";

    public static List<Clanstvo> Data = new List<Clanstvo>();

    public ClanstvoRepository()
    {
        if (Data.Count == 0)
        {
            Load();
        }
    }

    private void Load()
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            int korisnikId = int.Parse(parts[0]);
            int grupaId = int.Parse(parts[1]);
            Clanstvo clanstvo = new Clanstvo { KorisnikId = korisnikId, GrupaId = grupaId };
            Data.Add(clanstvo);

        }
    }

    public void Save()
    {
        List<string> lines = new List<string>();
        foreach (Clanstvo c in Data)
        {
            lines.Add($"{c.KorisnikId},{c.GrupaId}");
        }
        File.WriteAllLines(filePath, lines);
    }
}