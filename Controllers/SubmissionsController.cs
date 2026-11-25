using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Controllers;

[ApiController]
[Route("submissions")]
public class SubmissionsController : ControllerBase
{
    private readonly JournalDbContext _db;

    public SubmissionsController(JournalDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Submission>> Lever(Submission svar)
    {
        var oppgaveFinnes = await _db.Assignments.AnyAsync(a => a.Id == svar.AssignmentId);
        if (!oppgaveFinnes)
            return BadRequest($"Oppgaven finnes ikke");

        // Elev skal ikke kunne sette poeng selv
        svar.Poeng = null;
        svar.Tilbakemelding = null;
        svar.Rettet = null;

        _db.Submissions.Add(svar);
        await _db.SaveChangesAsync();
        return svar;
    }

    // Læreren retter
    [HttpPut("{id}/rett")]
    public async Task<ActionResult<Submission>> Rett(int id, Submission retting)
    {
        var svar = await _db.Submissions
            .Include(s => s.Assignment)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (svar == null) return NotFound();

        if (retting.Poeng < 0 || retting.Poeng > svar.Assignment!.MaksPoeng)
            return BadRequest($"Poeng må være mellom 0 og {svar.Assignment.MaksPoeng}.");

        svar.Poeng = retting.Poeng;
        svar.Tilbakemelding = retting.Tilbakemelding;
        svar.Rettet = DateTime.Now;

        await _db.SaveChangesAsync();
        return svar;
    }

}