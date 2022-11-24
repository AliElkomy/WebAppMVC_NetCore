using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string Description { get; set; }
    
        public Category() { }
        public Category( string name)
        {
            CatName = name;
        }
        public Category( string name, string description ) 
        {
            CatName = name;
            Description = description;
        }

        public Category(int id, string name, string description)
        {
            CatId = id;
            CatName = name;
            Description = description;

        }
    }
    
}
