using Microsoft.EntityFrameworkCore.Storage;

namespace WebAppMVC.Models
{
    public class vwFoodItemCat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public int CategoryId { get; set; }
        public vwFoodItemCat() { }
        public vwFoodItemCat(int id, string name, string discription, string category, int categoryId)
        {
            Id = id;
            Name = name;
            Description = discription;
            Category = category;
            CategoryId = categoryId;    
        }

        //public vwFoodItemCat GetFoodItemCatByID (int id) 
        //{  
        //    return new FoodContext.vwFoodItemCat.Where(food => food.id = id).FirstOrDefault();
        //}
    }
}
