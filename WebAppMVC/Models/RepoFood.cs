using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAppMVC.Controllers;

namespace WebAppMVC.Models
{
    public class RepoFood : iRepoFood
    {

        //private readonly ILogger _logger;
        private readonly FoodContext DB;

        public RepoFood(FoodContext _DB) 
        {
            DB = _DB;
        }


        public void AddFoodItem( FoodItem item)
        {
            //throw new System.NotImplementedException();
            DB.FoodItems.Add(item);
            DB.SaveChanges();
        }

        public void DeleteFoodItem(int id)
        {
            DB.FoodItems.Remove(GetFoodItem(id));
            DB.SaveChanges();
            //throw new System.NotImplementedException();
        }

        public List<FoodItem> GetAllFoodItems() => DB.FoodItems.ToList() ?? null;//throw new System.NotImplementedException();

        public FoodItem GetFoodItem(int id)
        {
            return (FoodItem)DB.FoodItems.Where(s => s.FoodId == id).FirstOrDefault();
            //throw new System.NotImplementedException();
        }

        public void UpdateFoodItem(FoodItem foodItem)
        {
            //int CatId = (from ct in DB.Categories where ct.CatName == foodItem.Category select ct.CatId).FirstOrDefault();
            int R = DB.Database.ExecuteSqlRaw("Update FoodItems " +
                                                    "set Name =@FName, Description =@Descr ,FoodCategoryId=@catId where FoodId=@id", 
                                                    new SqlParameter("@id", foodItem.FoodId),
                                                    new SqlParameter("@FName",foodItem.Name),
                                                    new SqlParameter("@Descr", foodItem.Description),
                                                    new SqlParameter("@catId", foodItem.FoodCategoryId));
            DB.SaveChanges ();
            //throw new System.NotImplementedException();
        }

        public  List<FoodItem> Find(string foodOrDesc) 
        {
           return (foodOrDesc!= "") ?  DB.FoodItems.Where(s=> s.Name.Contains(foodOrDesc)).ToList() : null;
              
        }

        public List<FoodItem> GetFoodItems(FoodItem searchItem)
        {
            return DB.FoodItems.Where(d => d.Name.Contains(searchItem.Name) || d.Description.Contains(searchItem.Description) ).ToList();
        }
        //public vwFoodItemCat GetFoodItemCat(int id)
        //{
        //    var c = (vwFoodItemCat)DB.vwFoodItemCat.Where(s => s.FoodId == id).FirstOrDefault();
        //    return c;


        //}

    }
}
