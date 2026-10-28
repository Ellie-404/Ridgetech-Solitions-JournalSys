using Microsoft.EntityFrameworkCore;
using JournalSys.Models;


namespace JournalSys.Data;

public class JournalDbContext : DbContext
{
    public JournalDbContext(DbContextOptions<JournalDbContext> options)
        : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<LabTest> LabTests { get; set; }
    public DbSet<LabResult> LabResults { get; set;}
    public DbSet<DoctorNote> DoctorNotes { get; set; }
    public DbSet<HistoryEntry> HistoryEntries { get; set; }
}