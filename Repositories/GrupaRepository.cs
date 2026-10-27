using System;
using System.IO;
using DrustvenaMreza.Models;
using System.Data.Common;
using System.Globalization;

namespace DrustvenaMreza.Repositories;

public class GrupaRepository
{
    private const string filePath = "Data/grupe.csv";
    public static Dictionary<int, Grupa> Data;


    public GrupaRepository()
    {
        if (Data == null)
        {
            Load();
        }
    }

    private void Load()
    {
        Data = new Dictionary<int, Grupa>();
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            
            string[] parts = line.Split(',');
            int id = int.Parse(parts[0]);
            string imeGrupe = parts[1];
            DateTime datumOsnivanja = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Grupa grupa = new Grupa(id, imeGrupe, datumOsnivanja);
            Data[id] = grupa;
        }
    }

    public void Save()
    {
        List<string> lines = new List<string>();
        foreach (Grupa g in Data.Values)
        {
            lines.Add($"{g.Id},{g.ImeGrupe},{g.DatumOsnivanja.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
        }
        File.WriteAllLines(filePath, lines);
    }
}