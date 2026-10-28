using Microsoft.AspNetCore.SignalR;

namespace JournalSys.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Fornavn { get; set; } = "";
    public string Etternavn { get; set; } = "";
    public string Spesialitet { get; set; } = "";
}
