namespace JournalSys.Models;

public class LabResult
{
    public int Id { get; set; }
    public string Svar { get; set; } = "";
    public DateTime SvarTidspunkt { get; set; } = DateTime.Now;

    public int LabTestId { get; set; }
}
