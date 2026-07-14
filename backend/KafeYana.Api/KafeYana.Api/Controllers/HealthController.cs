using KafeYana.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KafeYana.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(AppDbContext _db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _db.Database.ExecuteSqlRawAsync("SELECT 1");
            return Ok(new { status = "ok", time = DateTime.UtcNow });
        }
    }
}
