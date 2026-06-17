using System.ComponentModel.DataAnnotations;

namespace ornek.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<News>? NewsList { get; set; }
    }
}