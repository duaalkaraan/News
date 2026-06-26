using ornek.Models;
using System.ComponentModel.DataAnnotations;

namespace ornek.Dtos.News
{
    public class GetAllNewsDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

      
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public List<string>? Images { get; set; }
        public string CategoryName { get; set; } = null!;
        
    }
}
