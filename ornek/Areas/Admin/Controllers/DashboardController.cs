using Microsoft.AspNetCore.Mvc;
using ornek.Services;

namespace ornek.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly NewsService _newsService;
        private readonly CategoryService _categoryService;

        public DashboardController(NewsService newsService, CategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            ViewBag.NewsCount = _newsService.GetAllNews().Count;
            ViewBag.CategoryCount = _categoryService.GetAllCategories().Count;
            return View();
        }
    }
}