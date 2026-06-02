using Microsoft.AspNetCore.Mvc;

namespace VSystem.Internal.API.Time;

[ApiController]
[Route("api/time")]
public class ServerApiTime : ControllerBase
{
    [HttpGet("serverTime")]
    public IActionResult Time()
    {
        return Ok(DateTime.UtcNow);
    }
}