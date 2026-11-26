using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using JournalSys.Data;
using JournalSys.Models;

var builder = WebApplication.CreateBuilder(args);

// TODO: skru på AuthorizeFolder igjen når innlogging er klar osv

builder.Services.AddRazorPages(options =>
{   
    /*
    options.Conventions.AuthorizeFolder("/Pasient");
    options.Conventions.AuthorizeFolder("/Lege");
    options.Conventions.AuthorizeFolder("/Admin");
    options.Conventions.AllowAnonymousToPage("/Login");
    */
});

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Index";

        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/patients") ||
                context.Request.Path.StartsWithSegments("/doctors") ||
                context.Request.Path.StartsWithSegments("/appointments") ||
                context.Request.Path.StartsWithSegments("/labtests") ||
                context.Request.Path.StartsWithSegments("/notes") ||
                context.Request.Path.StartsWithSegments("/history"))
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

builder.Services.AddControllers();
builder.Services.AddDbContext<JournalDbContext>(options => 
    options.UseSqlite("Data source=journal.db"));

builder.Services.AddScoped<JournalSys.Services.HistoryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JournalDbContext>();
    db.Database.EnsureCreated();

    if (!db.LabTestTypes.Any())
    {
        db.LabTestTypes.AddRange(
            new JournalSys.Models.LabTestType { Navn = "Blodprøve", Beskrivelse = "Generell blodprøve" },
            new JournalSys.Models.LabTestType { Navn = "Urinprøve", Beskrivelse = "Urinanalyse" },
            new JournalSys.Models.LabTestType { Navn = "EKG", Beskrivelse = "Hjerterytme" }
        );
        db.SaveChanges();
    }

    if (!db.Users.Any())
    {
        var hasher = new PasswordHasher<User>();

        void LagBruker(string navn, string passord, string rolle)
        {
            var u = new User { Brukernavn = navn, Rolle = rolle };
            u.PassordHash = hasher.HashPassword(u, passord);
            db.Users.Add(u);
        }

        if (app.Environment.IsDevelopment())
        {
            LagBruker("admin1", "admin2", "admin3");
            LagBruker("lege1", "lege2", "lege3");
            LagBruker("sekretaer1", "sekretaer2", "sekretaer3");
        }
        else
        {
            var adminPassord = Environment.GetEnvironmentVariable("JOURNAL_ADMIN_PASSWORD");
            if (!string.IsNullOrEmpty(adminPassord))
            {
                LagBruker("admin", adminPassord, "Admin");
            }
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapControllers();

app.Run();