using Microsoft.AspNetCore.Mvc;
using AbiWebsite.Data;
using AbiWebsite.Models;

namespace AbiWebsite.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PushController(AbiDbContext db) : ControllerBase {
        private readonly AbiDbContext _db = db;

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] PushSubscription sub) {
            _db.PushSubscriptions.Add(sub);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}