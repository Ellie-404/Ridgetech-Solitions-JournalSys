namespace JournalSys.Models;

public class Submission
{
    public int Id { get; set; }
    public string Elev { get; set; } = "";
    public string Svar { get; set; } = "";
    public DateTime Levert { get; set; } = DateTime.Now;

    // Fylles ut av læreren
    public int? Poeng { get; set; }
    public string? Tilbakemelding { get; set; }
    public DateTime? Rettet { get; set; }

    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
}