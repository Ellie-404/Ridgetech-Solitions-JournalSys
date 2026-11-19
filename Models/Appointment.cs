using System.ComponentModel.DataAnnotations.Schema;

namespace JournalSys.Models;

public class Appointment
{
    [NotMapped]
    public List<int> ProveTypeIds { get; set; } = new();

    public int Id { get; set; }
    public DateTime Tidspunkt { get; set; }
    public string Arsak { get; set; } = "";

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public string Farge { get; set; } = "Gronn";
}
