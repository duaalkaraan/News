using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ornek.Data;
using ornek.Dtos;
using ornek.Models;
using ornek.Services;


namespace ornek.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly NewsService _newsService;

        public HomeController(AppDbContext context, NewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories
                .Include(c => c.NewsList
                .Where(n => n.Status == "Published")
                .OrderByDescending(n => n.CreatedAt))
                .ThenInclude(n => n.Images)
                .ToListAsync();


            var latestNews = await _newsService.GetLatestNews(9);
            ViewData["LatestNews"] = latestNews;


            return View(categories);
        }


        public async Task<IActionResult> Search(string q)
        {
               var results =await _newsService.Search(q);

            if (results.Count == 1)
            {
                return RedirectToAction("Details", "News", new { id = results.First().Id });
            }

            return View(results);
        }
    }
}