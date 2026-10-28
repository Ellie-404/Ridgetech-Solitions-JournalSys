namespace JournalSys.Models;

public class HistoryEntry
{
    public int Id { get; set; }
    public string Handling { get; set; }
    public string Beskrivelse { get; set; }
    public DateTime Tidspunkt { get; set; } = DateTime.Now;

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
}
