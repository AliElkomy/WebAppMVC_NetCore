using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;

namespace WebAppMVC.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int FoodCategoryId  { get; set; }
        public FoodItem() { }
        public FoodItem(string FName, string FDesc, int CatID)
        {
            Name = FName;
            Description = FDesc;
            FoodCategoryId = CatID;
        }
        //public FoodItem(string FName, string FDesc, Category FoodCategory)
        //{
        //    Name = FName; Description = FDesc;

        //}

    }
}
