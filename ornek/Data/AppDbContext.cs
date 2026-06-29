using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ornek.Models;

namespace ornek.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "سياسة" },
                new Category { Id = 2, Name = "رياضة" },
                new Category { Id = 3, Name = "تقنية" },
                new Category { Id = 4, Name = "اقتصاد" },
                new Category { Id = 5, Name = "ترفيه" }
            );
        }
    }
}