using Microsoft.AspNetCore.RateLimiting;
using ornek.Dtos.News;

namespace ornek.Areas.Admin.Dtos.News
{
    public class GetAlllNewsWithStatistics
    {
        public List<GetAllNewsDto> PublishedNewsList{ get; set; }
        public int CategoryCount { get; set; }
        public int PendingNewsCount { get; set; }
        public int PublishedNewsCount { get; set; }
        public int RejectedNewsCount { get; set; }
        public int TotalCount { get; set; }
    }
}
