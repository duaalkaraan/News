using Microsoft.EntityFrameworkCore;
using ornek.Models;

namespace ornek.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Data 
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