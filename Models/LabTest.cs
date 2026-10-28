namespace JournalSys.Models;

public class LabTest
{
    public int Id { get; set; }
    public string Type { get; set; } = "";
    public DateTime BestiltTidspunkt { get; set; } = DateTime.Now;

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public LabResult? Resultat { get; set; }
}
