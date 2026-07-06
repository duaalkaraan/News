using Microsoft.AspNetCore.Mvc;
using ornek.IServices;
using ornek.Services;

namespace ornek.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly NewsService _newsService;
        private readonly ICategoryService categoryService;

        public DashboardController(NewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            categoryService = categoryService;
        }

        public IActionResult Index()
        {
            ViewBag.NewsCount = _newsService.GetAllNews().PublishedNewsCount;
                ;
            ViewBag.CategoryCount = 0; //(categoryService.GetAllCategories()).Count;
            return View();
        }
    }
}