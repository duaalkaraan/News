using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ornek.Data;
using ornek.Models;

namespace ornek.Controllers
{
    public class NewsController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> NewsList()
        {
            var newsList = await context.News
                .Include(n => n.Category)
                .Include(n => n.Images)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            ViewBag.Categories = new SelectList(
                await context.Categories.ToListAsync(), "Id", "Name");

            return View(newsList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(News news, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "يرجى تعبئة جميع الحقول المطلوبة";
                return RedirectToAction("Index");
            }

            news.CreatedAt = DateTime.Now;
            context.News.Add(news);
            await context.SaveChangesAsync();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var path = Path.Combine("wwwroot/images/news", fileName);
                    Directory.CreateDirectory("wwwroot/images/news");

                    using var stream = new FileStream(path, FileMode.Create);
                    await image.CopyToAsync(stream);

                    await context.NewsImages.AddAsync(new NewsImage
                    {
                        NewsId = news.Id,
                        ImagePath = "/images/news/" + fileName
                    });
                }
            }

            await context.SaveChangesAsync();
            TempData["Success"] = "تم نشر الخبر بنجاح ✓";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string title,
     string content, int categoryId, List<IFormFile>? newImages)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "يرجى تعبئة جميع الحقول المطلوبة";
                return RedirectToAction("Index");
            }

            var news = await context.News.FindAsync(id);
            if (news != null)
            {
                news.Title = title;
                news.Content = content;
                news.CategoryId = categoryId;
                await context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل الخبر بنجاح ✓";
            }

            if (newImages != null)
            {
                foreach (var image in newImages)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                        var path = Path.Combine("wwwroot/images/news", fileName);
                        Directory.CreateDirectory("wwwroot/images/news");

                        using var stream = new FileStream(path, FileMode.Create);
                        await image.CopyToAsync(stream);

                        context.NewsImages.Add(new NewsImage
                        {
                            NewsId = id,
                            ImagePath = "/images/news/" + fileName
                        });
                    }
                }
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await context.News.FindAsync(id);
            if (news != null)
            {
                context.News.Remove(news);
                await context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الخبر بنجاح ✓";
            }
            return RedirectToAction("Index");
        }
        // عرض تفاصيل خبر واحد مع كل صوره
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var news = await context.News
                .Include(n => n.Category)
                .Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (news == null) return NotFound();

            return View(news);
        }
    }
}