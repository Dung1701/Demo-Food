using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
namespace Fast_Food1.Models
{
    public class FoodCategoryView
    {
        public List<Food>? Foods { get; set; }
        public SelectList? Category {  get; set; }
        public string? FoodCategory { get; set; }
         
        public string?  SearchString { get; set; }
        public List<Food> HotFoods { get; set; }
    }
}
