using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ornek.Dtos.News;
using ornek.Services;
using ornek.IServices;
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




        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Create()
        {
            var viewModel = new NewsViewModel
            {
                Categories = _categoryService.GetAllCategories()
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Create(NewsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoryService.GetAllCategories();
                return View(viewModel);
            }
            _newsService.Create(viewModel.News, viewModel.Images, "Pending");
            return RedirectToAction("Index");
        }


        public IActionResult Edit()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Edit()
        //{

        //}

        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(int id)
        //{

        //}
    }
}
