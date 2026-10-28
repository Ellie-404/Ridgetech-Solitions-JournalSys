using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JournalSys.Pages.Felles
{
    public class InnboksModel : PageModel
{
    public List<Melding> Meldinger { get; set; } = new();

    public void OnGet()
    {
        Meldinger = new List<Melding>
        {
            new() { Id = 1, Avsender = "Dr. Hansen", Emne = "Prøvesvar klare", Sendt = DateTime.Now.AddHours(-2), Lest = false },
            new() { Id = 2, Avsender = "Timebestilling", Emne = "Bekreftelse på time", Sendt = DateTime.Now.AddDays(-1), Lest = true }
        };
    }
}
}
