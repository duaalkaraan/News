using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ornek.Data;
using ornek.Dtos;
using ornek.Models;


namespace ornek.Controllers
{
    public class HomeController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await context.Categories
                .Include(c => c.NewsList
                .Where(n => n.Status == "Published")
                .OrderByDescending(n => n.CreatedAt))
                .ThenInclude(n => n.Images)
                .ToListAsync();

            return View(categories);
        }
    }
}