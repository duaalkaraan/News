using ornek.Models;

namespace ornek.ViewModels
{
    public class NewsViewModel
    {
        public News News { get; set; } = new News();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<IFormFile>? Images { get; set; }
    }
}
