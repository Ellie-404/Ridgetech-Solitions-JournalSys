using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Controllers;

[ApiController]
[Route("labtesttypes")]
public class LabTestTypesController : ControllerBase
{
    private readonly JournalDbContext _db;

    public LabTestTypesController(JournalDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<LabTestType>> HentAlle()
    {
        return await _db.LabTestTypes.OrderBy(t => t.Navn).ToListAsync();
    }

    [HttpPost]
    public async Task<LabTestType> LagNy(LabTestType type)
    {
        _db.LabTestTypes.Add(type);
        await _db.SaveChangesAsync();
        return type;
    }
}