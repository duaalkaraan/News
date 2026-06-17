using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ornek.Data;
using ornek.Models;

namespace ornek.Controllers
{
    public class NewsController(AppDbContext context) : Controller
    {
        

        // عرض كل الأخبار
        public async Task<IActionResult> Index()
        {
            var newsList =await context.News.ToListAsync();
            return View(newsList);
        }

        // فتح فورم إضافة خبر
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // استقبال الفورم وحفظ الخبر
        [HttpPost]
        public IActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                news.CreatedAt = DateTime.Now;
                context.News.Add(news);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // حذف خبر
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var news = context.News.Find(id);
            if (news != null)
            {
                context.News.Remove(news);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // تعديل خبر
        [HttpPost]
        public IActionResult Edit(int id, string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "العنوان والمحتوى مطلوبان!";
                return RedirectToAction("Index");
            }

            var news = context.News.Find(id);
            if (news != null)
            {
                news.Title = title;
                news.Content = content;
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}