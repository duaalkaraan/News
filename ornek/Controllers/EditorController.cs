using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ornek.Dtos.News;
using ornek.IServices;
using ornek.Services;
using ornek.ViewModels;



namespace ornek.Controllers
{
    [Authorize(Roles = "Editor")]
    public class EditorController : Controller
    {
        private readonly NewsService _newsService;
        private readonly ICategoryService _categoryService;

        public EditorController(NewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }


       

        public IActionResult Create()
        {
            var categories = _categoryService.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateNewsDto dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = _categoryService.GetAllCategories();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(dto);
            }

            var createdById = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
            _newsService.CreateFromDto(dto, createdById);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var edtorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var news = _newsService.GetEditorNews(edtorId);
            return View(news);
        }


        public IActionResult Edit(int id)
        {
            var news = _newsService.GetById(id);
            var viewModel = new NewsViewModel
            {
                News = news!,
                Categories = _categoryService.GetAllCategories()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(NewsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoryService.GetAllCategories();
                return View(viewModel);
            }
            viewModel.News.Status = "Pending"; 
            _newsService.Update(viewModel.News, viewModel.Images);
            return RedirectToAction("Index");
        }




        public IActionResult Delete(int id)
        {
            var news = _newsService.GetById(id);
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _newsService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
