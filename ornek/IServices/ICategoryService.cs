using ornek.Models;

namespace ornek.IServices
{
    public interface ICategoryService
    {
        public List<Category> GetAllCategories();
        public Category? GetById(int id);
        public void Create(Category category);
        public void Update(Category category);
        public void Delete(int id);
    }
}
