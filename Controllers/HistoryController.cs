using Microsoft.AspNetCore.Mvc;

namespace JournalSys.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
