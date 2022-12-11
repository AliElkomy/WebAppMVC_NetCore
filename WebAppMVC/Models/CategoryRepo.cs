using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebAppMVC.Models;

namespace WebAppMVC.Models
{
    public class CategoryRepo : iCategory
    {
        WebAppMVC.Models.FoodContext DB;
        public CategoryRepo(WebAppMVC.Models.FoodContext _DB)
        {
            DB = _DB;
        }
        public void DeleteCategory(int id)
        {
            DB.Categories.Remove(GetCategory(id));
            DB.SaveChanges();
           // throw new System.NotImplementedException();
        }

        public List<Category> GetCategories()
        {
           return(DB.Categories.ToList() != null) ? DB.Categories.ToList() :null;
            //throw new System.NotImplementedException();
        }

        public Category GetCategory(int id)
        {
            return ((Category)DB.Categories.Where(c => c.CatId == id) != null) ? (Category)DB.Categories.Where(c => c.CatId == id) : null;
           
            //throw new System.NotImplementedException();
        }

        public void InsertCategory(Category category)
        {
            DB.Categories.Add(category);
            DB.SaveChanges();
            //throw new System.NotImplementedException();
        }

        public void UpdateCategory(int id, Category category)
        {
            DB.Update(category);
            DB.SaveChanges();
            throw new System.NotImplementedException();
        }

        public List<Category> Find(string txt)
        {
            return (txt != "") ? DB.Categories.Where(s => s.CatName.Contains(txt) || s.Description.Contains(txt)).ToList() : null;
        }
    }

    internal struct NewStruct
    {
        public object Item1;
        public object Item2;

        public NewStruct(object item1, object item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj)
        {
            return obj is NewStruct other &&
                   EqualityComparer<object>.Default.Equals(Item1, other.Item1) &&
                   EqualityComparer<object>.Default.Equals(Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Item1, Item2);
        }

        public void Deconstruct(out object item1, out object item2)
        {
            item1 = Item1;
            item2 = Item2;
        }

        public static implicit operator (object, object)(NewStruct value)
        {
            return (value.Item1, value.Item2);
        }

        public static implicit operator NewStruct((object, object) value)
        {
            return new NewStruct(value.Item1, value.Item2);
        }
    }
}
