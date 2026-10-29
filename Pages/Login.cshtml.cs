using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JournalSys.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testbruker"),
                new Claim(ClaimTypes.Role, "Lege"),
            };
            var identity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            return Redirect("/");
        }
    }
}
