using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using JournalSys.Data;
using JournalSys.Models;
using JournalSys.Services;

namespace JournalSys.Controllers;

[ApiController]
[Route("patients")]
// [Authorize]
public class PatientsController : ControllerBase
{
    private readonly JournalDbContext _db;

    public PatientsController(JournalDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<List<Patient>> HentAlle()
    {
        return await _db.Patients.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> HentEn(int id)
    {
        var pasient = await _db.Patients.FindAsync(id);
        if (pasient == null) return NotFound();
        return pasient;
    }

    [HttpPost]
    public async Task<Patient> LagNy(Patient pasient)
    {
        _db.Patients.Add(pasient);
        await _db.SaveChangesAsync();
        return pasient;
    }

    [HttpPost("generer")]
    public async Task<Patient> Generer()
    {
        var pasient = PatientGenerator.Lag();
        _db.Patients.Add(pasient);
        await _db.SaveChangesAsync();
        return pasient;
    }
}
