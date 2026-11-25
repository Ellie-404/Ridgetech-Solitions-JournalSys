using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Controllers;

[ApiController]
[Route("assignments")]
public class AssignmentsController : ControllerBase
{
    private readonly JournalDbContext _db;

    public AssignmentsController(JournalDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<Assignment>> HentAlle()
    {
        return await _db.Assignments.OrderByDescending(a => a.Opprettet).ToListAsync();
    }

    [HttpPost]
    public async Task<Assignment> LagNy(Assignment oppgave)
    {
        _db.Assignments.Add(oppgave);
        await _db.SaveChangesAsync();
        return oppgave;
    }

    [HttpGet("{id}/submissions")]
    public async Task<List<Submission>> HentSvar(int id)
    {
        return await _db.Submissions
            .Where(s => s.AssignmentId == id)
            .OrderBy(s => s.Levert)
            .ToListAsync();
    }
}