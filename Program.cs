using Microsoft.EntityFrameworkCore;
using JournalSys.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Pasient");
    options.Conventions.AuthorizeFolder("/Lege");
    options.Conventions.AuthorizeFolder("/Admin");
    options.Conventions.AllowAnonymousToPage("/Login");
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