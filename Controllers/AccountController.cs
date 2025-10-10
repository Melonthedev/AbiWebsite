using AbiWebsite.Data;
using AbiWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbiWebsite.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller {
        private readonly AbiDbContext _db;
        private readonly EmailService _emailService;

        public AccountController(AbiDbContext db, EmailService emailService) {
            _db = db;
            _emailService = emailService;
        }

        [HttpPost("resendlogincode")]
        public async Task<IActionResult> ResendLoginCode([FromForm] string name) {
            var student = await _db.Students.FirstOrDefaultAsync(s => s.FullName == name);
            if (student == null)
                return Redirect("/loginwithcode?name=" + Uri.EscapeDataString(name) + "&msg=notfound");

            var rnd = new Random();
            student.LoginCode = rnd.Next(100, 1000);
            await _db.SaveChangesAsync();

            var emailSent = _emailService.SendLoginCodeMail(student.Email, student.FullName, student.LoginCode.ToString("D3"), isResend: true);
            
            if (emailSent) return Redirect("/loginwithcode?name=" + Uri.EscapeDataString(name) + "&msg=resent");
            else return Redirect("/loginwithcode?name=" + Uri.EscapeDataString(name) + "&msg=notsent");
        }
    }
}
