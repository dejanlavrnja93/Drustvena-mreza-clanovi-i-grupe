using System;
namespace DrustvenaMreza.Models;

public class Grupa
{
    public int Id { get; set; }
    public string ImeGrupe { get; set; }
    public DateTime DatumOsnivanja { get; set; }

    

    public Grupa(int id, string imeGrupe, DateTime datumOsnivanja)
    {
        Id = id;
        ImeGrupe = imeGrupe;
        DatumOsnivanja = datumOsnivanja;
    }

}
