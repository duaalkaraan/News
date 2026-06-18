using ornek.Models;

public class NewsImage
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public int NewsId { get; set; }
    public News? News { get; set; }
}