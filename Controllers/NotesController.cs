using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;
using JournalSys.Services;

namespace JournalSys.Controllers;

[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
    private readonly JournalDbContext _db;
    private readonly HistoryService _historikk;

    public NotesController(JournalDbContext db, HistoryService historikk)
    {
        _db = db;
        _historikk = historikk;
    }

    [HttpGet]
    public async Task<List<DoctorNote>> HentAlle()
    {
        return await _db.DoctorNotes
            .Include(n => n.Patient)
            .Include(n => n.Doctor)
            .OrderByDescending(n => n.Opprettet)
            .ToListAsync();
    }

    [HttpGet("patient/{patientId}")]
    public async Task<List<DoctorNote>> HentForPasient(int patientId)
    {
        return await _db.DoctorNotes
            .Where(n => n.PatientId == patientId)
            .Include(n => n.Doctor)
            .OrderByDescending(n => n.Opprettet)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<DoctorNote>> LagNy(DoctorNote notat)
    {
        var pasientFinnes = await _db.Patients.AnyAsync(p => p.Id == notat.PatientId);
        var legeFinnes = await _db.Doctors.AnyAsync(d => d.Id == notat.DoctorId);

        if (!pasientFinnes || !legeFinnes)
            return BadRequest("Lege eller pasient finnes ikke.");

        _db.DoctorNotes.Add(notat);
        await _db.SaveChangesAsync();

        await _historikk.Logg(notat.PatientId, notat.DoctorId, "Notat skrevet", "Nytt legenotat opprettet");

        return notat;
    }
}
