using Microsoft.AspNetCore.Mvc;

namespace JournalSys.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExaminationsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
