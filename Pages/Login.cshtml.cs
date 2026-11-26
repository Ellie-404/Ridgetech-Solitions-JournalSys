using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JournalSys.Data;
using JournalSys.Models;

namespace JournalSys.Pages
{
    public class LoginModel : PageModel
    {
        private readonly JournalDbContext _db;

        public LoginModel(JournalDbContext db)
        {
            _db = db;
        }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";
        public string Feil { get; set; }

        public void OnGet() {}

        public async Task<IActionResult> OnPostAsync()
        {
            var bruker = await _db.Users.FirstOrDefaultAsync(u => u.Brukernavn == Username);
            var hasher = new PasswordHasher<User>();

            if (bruker == null ||
                hasher.VerifyHashedPassword(bruker, bruker.PassordHash, Password) == PasswordVerificationResult.Failed)
            {
                Feil = "Feil brukernavn eller passord.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, bruker.Brukernavn),
                new Claim(ClaimTypes.Role, bruker.Rolle)
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Redirect("/");
        }
    }
}
