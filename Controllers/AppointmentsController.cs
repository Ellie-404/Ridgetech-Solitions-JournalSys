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

        foreach (var typeId in time.ProveTypeIds)
        {
            var type = await _db.LabTestTypes.FindAsync(typeId);
            if (type == null) continue;

            _db.LabTests.Add(new LabTest
            {
                Type = type.Navn,
                LabTestTypeId = type.Id,
                AppointmentId = time.Id,
                PatientId = time.PatientId,
                DoctorId = time.DoctorId
            });
        }
        await _db.SaveChangesAsync();

        await _historikk.Logg(time.PatientId, time.DoctorId, "Time bestilt", $"Time {time.Tidspunkt:dd.MM.yyyy HH:mm}: {time.Arsak}");
        return time;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Appointment>> Rediger(int id, Appointment oppdatert)
    {
        var time = await _db.Appointments.FindAsync(id);
        if (time == null) return NotFound();

        var legeFinnes = await _db.Doctors.AnyAsync(d => d.Id == oppdatert.DoctorId);
        if (!legeFinnes)
            return BadRequest("Lege finnes ikke.");

        var opptatt = await _db.Appointments.AnyAsync(a => 
        a.Id != id &&
        a.DoctorId == oppdatert.DoctorId &&
        a.Tidspunkt == oppdatert.Tidspunkt);

        if (opptatt)
            return BadRequest("Legen har allerede en time på dette tidspunktet.");

        var gammeltTidspunkt = time.Tidspunkt;

        time.Tidspunkt = oppdatert.Tidspunkt;
        time.Arsak = oppdatert.Arsak;
        time.Farge = oppdatert.Farge;
        time.DoctorId = oppdatert.DoctorId;

        await _db.SaveChangesAsync();

        await _historikk.Logg(time.PatientId, time.DoctorId, "Time endret",
            $"Tidspunkt endret fra {gammeltTidspunkt:dd.MM.yyyy HH:mm} til {time.Tidspunkt:dd.MM.yyyy HH:mm}: {time.Arsak}");
        
        return time;
    }
}
