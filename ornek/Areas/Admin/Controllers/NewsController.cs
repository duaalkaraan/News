using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ornek.IServices;
using ornek.Models;
using ornek.Services;
using ornek.ViewModels;

namespace ornek.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
 
    public class NewsController : Controller
    {

        private readonly NewsService _newsService;


        private readonly ICategoryService _categoryService;

        public NewsController(NewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }



        public IActionResult Index()
        {
            var news = _newsService.GetAllNews();
            return View(news);
            
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
            if(!ModelState.IsValid)
            {
                viewModel.Categories = _categoryService.GetAllCategories();
                return View(viewModel);
            }
            _newsService.Create(viewModel.News, viewModel.Images);
            return RedirectToAction("Index");
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
            _newsService.Update(viewModel.News, viewModel.Images);
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public IActionResult Edit(NewsViewModel viewModel)
        //{
        //    _newsService.Update(viewModel.News, viewModel.Images);
        //    return RedirectToAction("Index");
        //}





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
