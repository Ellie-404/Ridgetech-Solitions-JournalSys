namespace JournalSys.Models;

public class User
{
    public int Id { get; set; }
    public string Brukernavn { get; set; } = "";
    public string PassordHash { get; set; } = "";
    public string Rolle { get; set; } = "Sekretaer";
}