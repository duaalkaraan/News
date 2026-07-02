using System.ComponentModel.DataAnnotations;

namespace ornek.Dtos.News
{
    public class CreateNewsDto
    {
        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "المحتوى مطلوب")]
        public string Content { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "التصنيف مطلوب")]
        public int CategoryId { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}