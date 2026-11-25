namespace JournalSys.Models;

public class Assignment
{
    public int Id { get; set; }
    public string Tittel { get; set; } = "";
    public string OppgaveTekst { get; set; } = "";
    public int MaksPoeng { get; set; } = 10;
    public DateTime Opprettet { get; set; } = DateTime.Now;
}