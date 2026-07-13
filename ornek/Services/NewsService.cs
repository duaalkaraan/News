using Microsoft.EntityFrameworkCore;
using ornek.Areas.Admin.Dtos.News;
using ornek.Data;
using ornek.Dtos.News;
using ornek.Helpers;
using ornek.Models;


namespace ornek.Services
{

    public class NewsService
    {
        private readonly AppDbContext _context;
       
        public NewsService(AppDbContext context)        
        {
            _context = context;
        }
         
       public async Task<GetAlllNewsWithStatistics> GetAllNews(string q, int pageIndex = 1)
        {
            int pageSize = 10;


            var query = _context.News
                .Where(x => x.Status == "Published")
                .Include(n => n.Category).Include(n => n.Images)
                .AsQueryable()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{q}%"));
            }

            var dtoQuery = query.Select(x => new GetAllNewsDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
                Images = x.Images!.Select(x => x.ImagePath).ToList(),
                Status = x.Status ?? "Draft"
            });
            var paginatedValues = await PaginatedList<GetAllNewsDto>.CreateAsync(dtoQuery, pageIndex, pageSize);


            //var values =await query.Select(x=> new GetAllNewsDto
            //    {

            //        Id = x.Id,
            //        Title = x.Title,
            //        Content = x.Content,
            //        CreatedAt = x.CreatedAt,
            //        CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
            //        Images = x.Images!.Select(x=> x.ImagePath).ToList(),
            //        Status = x.Status ?? "Draft"
            ////    })
            //    .ToListAsync();

            var rejectedNews = _context.News.Where(x => x.Status == "Rejected").Count();
            var pendingNews = _context.News.Where(x => x.Status == "Pending").Count();
            var publishedNews = _context.News.Where(x => x.Status == "Published").Count();
            var categoryCount = _context.Categories.Count();
            var totalCount = _context.News.Count();

            return new()
            {
                PublishedNewsList = paginatedValues,
                RejectedNewsCount = rejectedNews,
                PendingNewsCount = pendingNews,
                PublishedNewsCount = publishedNews,
                CategoryCount = categoryCount,
                TotalCount = totalCount
            };
        }

        public List<GetAllNewsDto> GetPendingNews()
        {
            return _context.News
                .Where(x => x.Status == "Pending")
                .Include(n => n.Category)
                .Include(n => n.Images)
                .Select(x => new GetAllNewsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
                    Images = x.Images!.Select(i => i.ImagePath).ToList(),
                    Status = x.Status
                })
                .ToList();
        }

        public List<GetAllNewsDto> GetEditorNews(string editorId)
        {
            return _context.News
                .Where(x => x.CreatedById == editorId)
                .Include(n => n.Category)
                .Include(n => n.Images)
                .Select(x => new GetAllNewsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
                    Images = x.Images!.Select(i => i.ImagePath).ToList(),
                    Status = x.Status
                })
                .ToList();
        }


        public void UpdateStatus(int id, string status)
        {
            var news = GetById(id);
            if (news != null)
            {
                news.Status = status;
                _context.SaveChanges();
            }
        }
        public News? GetById(int id)
        {
            return  _context.News.Include(n=> n.Category).Include(n=> n.Images).FirstOrDefault(n=> n.Id == id);

        }

        public async Task<List<GetAllNewsDto>> Search(string q)
        {
            var query = _context.News
                .Where(x => x.Status == "Published")
                .Include(n => n.Category)
                .Include(n => n.Images).AsQueryable().AsNoTracking();
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x=>EF.Functions.Like(x.Title,$"%{q}%") || EF.Functions.Like(x.Content, $"%{q}%"));
            }
            else
            {
                return new();
            }
            var values = await query
                .Select(x => new GetAllNewsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
                    Images = x.Images!.Select(i => i.ImagePath).ToList(),
                    Status = x.Status
                })
                .ToListAsync();
            return values;
        }

        public void Delete(int id)
        {
            var news = GetById(id);
            if(news != null)
            {
                _context.News.Remove(news);
                _context.SaveChanges();
            }
        }



        public void Create(News news, List<IFormFile>? images, string status = "Published")
        {
            news.CreatedAt = DateTime.Now;
            news.Status = status;
            _context.News.Add(news);
            _context.SaveChanges();

            if(images != null)
            {
                 foreach(var image in images)
                 {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var path = Path.Combine("wwwroot/images/news", fileName);
                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    _context.NewsImages.Add(new NewsImage
                    {
                        ImagePath = "/images/news/" + fileName,
                        NewsId = news.Id
                    });
                 }
                _context.SaveChanges();
            }
        }

        public void Update(News news, List<IFormFile>? images)
        {
            var existing = GetById(news.Id)!;
            existing.Title = news.Title;
            existing.Content = news.Content;
            existing.CategoryId = news.CategoryId;
            _context.SaveChanges();

            if (images != null)
            {
                foreach (var image in images)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var path = Path.Combine("wwwroot/images/news", fileName);
                    using (var stream = new FileStream(path,FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    _context.NewsImages.Add(new NewsImage
                    {
                        ImagePath = "/images/news/" + fileName,
                        NewsId = news.Id
                    });
                }

                _context.SaveChanges();


            }
        }

        public void CreateFromDto(CreateNewsDto dto, string createdById)
        {
            var news = new News
            {
                Title = dto.Title,
                Content = dto.Content,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.Now,
                Status = "Pending",
                CreatedById = createdById
            };

            _context.News.Add(news);
            _context.SaveChanges();

            if (dto.Images != null)
            {
                foreach (var image in dto.Images)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var path = Path.Combine("wwwroot/images/news", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                    _context.NewsImages.Add(new NewsImage
                    {
                        ImagePath = "/images/news/" + fileName,
                        NewsId = news.Id
                    });
                }
                _context.SaveChanges();
            }
        }
        public List<GetAllNewsDto> SearchAdmin(string q)
        {
            return _context.News
                .Where(x => x.Title.Contains(q) || x.Content.Contains(q))
                .Include(n => n.Category)
                .Include(n => n.Images)
                .Select(x => new GetAllNewsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
                    Images = x.Images!.Select(i => i.ImagePath).ToList(),
                    Status = x.Status
                })
                .ToList();
        }

        public async Task<List<GetAllNewsDto>> GetLatestNews(int count)
        {
            var query = _context.News
                .Where(x => x.Status == "Published")
                .Include(n => n.Category)
                .Include(n => n.Images)
                .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .AsQueryable()
                .AsNoTracking();

            var values = await query
                .Select(x => new GetAllNewsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    CategoryName = x.Category != null ? x.Category.Name : "بدون تصنيف",
                    Images = x.Images!.Select(i => i.ImagePath).ToList(),
                    Status = x.Status
                })
                .ToListAsync();

            return values;
        }

    }
}
