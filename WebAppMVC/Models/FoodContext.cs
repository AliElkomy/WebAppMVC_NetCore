using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.Models
{
    public class FoodContext : DbContext 
    {
        public FoodContext(DbContextOptions options) : base(options) { }
        public DbSet<FoodItem> FoodItems { get; set; }  
        public DbSet<Category> Categories { get; set; }
        public DbSet<vwFoodItemCat> vwFoodItemCats { get; set; }
        
    }
}
