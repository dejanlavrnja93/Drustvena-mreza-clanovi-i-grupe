namespace DrustvenaMreza.Models;

public class Korisnik
{
    public int Id{ get;set;}

    public string KorisnickoIme{get;set;} = string.Empty;
    public string Ime{get;set;} = string.Empty;
    public string Prezime{get;set;}= string.Empty;
    public DateOnly DatumRodjenja{get;set;}
}