using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;
using JournalSys.Services;

namespace JournalSys.Controllers;

[ApiController]
[Route("labtests")]
public class LabTestsController : ControllerBase
{
    private readonly JournalDbContext _db;
    private readonly HistoryService _historikk;

    public LabTestsController(JournalDbContext db, HistoryService historikk)
    {
        _db = db;
        _historikk = historikk;
    }

    [HttpGet]
    public async Task<List<LabTest>> HentAlle()
    {
        return await _db.LabTests
            .Include(t => t.Patient)
            .Include(t => t.Doctor)
            .Include(t => t.Resultat)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<LabTest>> Bestill(LabTest prove)
    {
        var pasientFinnes = await _db.Patients.AnyAsync(p => p.Id == prove.PatientId);
        var legeFinnes = await _db.Doctors.AnyAsync(d => d.Id == prove.DoctorId);

        if(!pasientFinnes || !legeFinnes)
            return BadRequest($"PatientId={prove.PatientId}, DoctorId={prove.DoctorId}, pasientFinnes={pasientFinnes}, legeFinnes={legeFinnes}");

        _db.LabTests.Add(prove);
        await _db.SaveChangesAsync();
        return prove;
    }

    [HttpPost("{id}/resultat")]
    public async Task<ActionResult<LabResult>> RegistrerSvar(int id, LabResult resultat)
    {
        var prove = await _db.LabTests
            .Include(t => t.Resultat)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (prove == null) return NotFound();
        if (prove.Resultat !=null)
            return BadRequest("Denne prøven har allerede fått svar.");

        resultat.LabTestId = id;
        _db.LabResults.Add(resultat);
        await _db.SaveChangesAsync();

        await _historikk.Logg(prove.PatientId, prove.DoctorId, "Prøve bestilt", prove.Type);

        return resultat;
    }
}
