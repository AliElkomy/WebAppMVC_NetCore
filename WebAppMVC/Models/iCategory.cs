using System.Collections.Generic;

namespace WebAppMVC.Models
{
    public interface iCategory
    {
        List<Category> GetCategories();
        Category GetCategory(int id);
        void DeleteCategory(int id);
        void UpdateCategory(int id, Category category);
        void InsertCategory(Category category);
    }
}
