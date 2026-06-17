using System.ComponentModel.DataAnnotations;

namespace ornek.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "content is required")]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}