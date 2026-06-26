using Microsoft.AspNetCore.Mvc;
using ornek.Models;
using ornek.Data;
using Microsoft.EntityFrameworkCore;


namespace ornek.Controllers
{
    public class AccountController(AppDbContext context) : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == model.Username
                                                               && u.Password == model.Password);
            if(user == null)
            {
                ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور خطأ");
                return View(model);
            }
            if(user.Role == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            }
            return RedirectToAction("Index", "Home");
        }
    }
}