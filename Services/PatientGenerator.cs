using JournalSys.Models;

namespace JournalSys.Services;

public static class PatientGenerator
{
    private static readonly string[] Fornavn = 
    {"Ola", "Kari", "Nils", "Ingrid", "Lars", "Sofie", "Emil", "Nora", "Jonas", "Maja"};
    private static readonly string[] Etternavn =
    {"Hansen", "Johansen", "Olsen", "Larsen", "Andersen", "Pedersen", "Nilsen", "Kristiansen"};
    private static readonly string[] Gater =
    {"Storgata", "Kirkeveien", "Skolegata", "Parkveien", "Fjellveien", "Strandgata"};
    private static readonly string[] Steder =
    {"3015 Drammen", "3616 Kongsberg", "3510 Hønefoss", "3045 Drammen"};

    public static Patient Lag()
    {
        var rnd = Random.Shared;
        var dato = DateTime.Today.AddDays(-rnd.Next(0, 90 * 365));
        var personnummer = $"{dato:dd}{dato:MM}{dato:yy}{rnd.Next(0, 100000):D5}";

        return new Patient
        {
            Fornavn = Fornavn[rnd.Next(Fornavn.Length)],
            Etternavn = Etternavn[rnd.Next(Etternavn.Length)],
            Fodselsdato = dato,
            Telefon = "4" + rnd.Next(1000000, 10000000),
            Personnummer = personnummer,
            Adresse = $"{Gater[rnd.Next(Gater.Length)]} {rnd.Next(1, 100)}, {Steder[rnd.Next(Steder.Length)]}"
        };
    }
}