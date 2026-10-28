using Microsoft.AspNetCore.SignalR;

namespace JournalSys.Models;

public class DoctorNote
{
    public int Id { get; set; }
    public string Tekst { get; set; } = "";
    public DateTime Opprettet { get; set; } = DateTime.Now;

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
}
