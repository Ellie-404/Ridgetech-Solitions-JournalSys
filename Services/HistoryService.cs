using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Services;

public class HistoryService
{
    private readonly JournalDbContext _db;

    public HistoryService(JournalDbContext db)
    {
        _db = db;
    }

    public async Task Logg(int patientId, int? doctorId, string handling, string beskrivelse)
    {
        _db.HistoryEntries.Add(new HistoryEntry
        {
            PatientId = patientId,
            DoctorId = doctorId,
            Handling = handling,
            Beskrivelse = beskrivelse
        });
        await _db.SaveChangesAsync();
    }
}
