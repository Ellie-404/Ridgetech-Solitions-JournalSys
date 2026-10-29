using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;
using JournalSys.Services;

namespace JournalSys.Controllers;

[ApiController]
[Route("appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly JournalDbContext _db;
    private readonly HistoryService _historikk;

    public AppointmentsController(JournalDbContext db, HistoryService historikk)
    {
        _db = db;
        _historikk = historikk;
    }

    [HttpGet]
    public async Task<List<Appointment>> HentAlle()
    {
        return await _db.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> LagNy(Appointment time)
    {
        var pasientFinnes = await _db.Patients.AnyAsync(p => p.Id == time.PatientId);
        var legeFinnes = await _db.Doctors.AnyAsync(d => d.Id == time.DoctorId);

        if (!pasientFinnes || !legeFinnes)
            return BadRequest("Pasient eller lege finnes ikke.");

        var opptatt = await _db.Appointments.AnyAsync(a =>
            a.DoctorId == time.DoctorId && a.Tidspunkt == time.Tidspunkt);

        if (opptatt)
            return BadRequest("Legen har allerede en time på dette tidspunktet.");

        _db.Appointments.Add(time);
        await _db.SaveChangesAsync();

        await _historikk.Logg(time.PatientId, time.DoctorId, "Time bestilt", $"Time {time.Tidspunkt:dd.MM.yyyy HH:mm}: {time.Arsak}");
        return time;
    }
}
