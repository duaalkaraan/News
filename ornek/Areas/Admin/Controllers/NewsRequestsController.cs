using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ornek.IServices;
using ornek.Services;
using ornek.Dtos.News;



namespace ornek.Areas.Admin.Controllers
{


    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class NewsRequestsController : Controller
    {

        private readonly NewsService _newsService;


        private readonly ICategoryService _categoryService;

        public NewsRequestsController(NewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }
       


        public IActionResult Index()
        {
            var news = _newsService.GetPendingNews();
            return View(news);
        }

        public IActionResult Approve(int id)
        {
            _newsService.UpdateStatus(id, "Published");
            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
            _newsService.UpdateStatus(id, "Rejected");
            return RedirectToAction("Index");
        }
    }
}
