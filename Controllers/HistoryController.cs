using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Controllers;

[ApiController]
[Route("history")]
public class HistoryController : ControllerBase
{
    private readonly JournalDbContext _db;

    public HistoryController(JournalDbContext db)
    {
        _db = db;
    }

    [HttpGet("patient/{patientId}")]
    public async Task<List<HistoryEntry>> HentForPasient(int patientId)
    {
        return await _db.HistoryEntries
            .Where(h => h.PatientId == patientId)
            .Include(h => h.Doctor)
            .OrderByDescending(h => h.Tidspunkt)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<HistoryEntry>> LagNy(HistoryEntry oppforing)
    {
        var pasientFinnes = await _db.Patients.AnyAsync(p => p.Id == oppforing.PatientId);
        if (!pasientFinnes)
            BadRequest("Pasient finnes ikke.");

        _db.HistoryEntries.Add(oppforing);
        await _db.SaveChangesAsync();
        return oppforing;
    }
}
