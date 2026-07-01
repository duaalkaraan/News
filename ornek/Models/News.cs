using System.ComponentModel.DataAnnotations;

namespace ornek.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "المحتوى مطلوب")]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "التصنيف مطلوب")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<NewsImage>? Images { get; set; }
        public string Status { get; set; }
    }
}