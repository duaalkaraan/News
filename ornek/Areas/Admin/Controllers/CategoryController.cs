using Microsoft.AspNetCore.Mvc;
using ornek.Services;
using ornek.Models;
using ornek.IServices;

namespace ornek.Areas.Admin.Controllers
{
    [Area ("Admin")]
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        
        public IActionResult Index()
        {
            var categories = categoryService.GetAllCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            categoryService.Create(category);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = categoryService.GetById(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            categoryService.Update(category);
            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(int id)
        {
           var category = categoryService.GetById(id);
           return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            categoryService.Delete(id);
            return RedirectToAction("Index");
        }


    }

}
