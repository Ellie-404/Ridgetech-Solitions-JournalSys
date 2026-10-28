using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Controllers;

[ApiController]
[Route("doctors")]
public class DoctorsController : ControllerBase
{
    private readonly JournalDbContext _db;

    public DoctorsController(JournalDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<Doctor>> HentAlle()
    {
        return await _db.Doctors.ToListAsync();
    }

    [HttpPost]
    public async Task<Doctor> LagNy(Doctor lege)
    {
        _db.Doctors.Add(lege);
        await _db.SaveChangesAsync();
        return lege;
    }
}