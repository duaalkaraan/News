using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ornek.Data;

namespace ornek.Controllers
{
    public class HomeController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var categories = await context.Categories
                .Include(c => c.NewsList)
                .ThenInclude(n => n.Images)
                .ToListAsync();

            return View(categories);
        }
    }
}