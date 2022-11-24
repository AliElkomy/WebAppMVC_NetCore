
using System.Collections;
using System.Collections.Generic;

namespace WebAppMVC.Models
{
    public interface iRepoFood
    {
        FoodItem GetFoodItem(int id);
        void UpdateFoodItem(FoodItem foodItem);
        void DeleteFoodItem(int id);
        void AddFoodItem(FoodItem item);
        List<FoodItem> GetAllFoodItems();
        List<FoodItem> GetFoodItems(FoodItem searchItem);
    }
}
