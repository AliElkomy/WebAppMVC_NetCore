using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAppMVC.Models;


namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FoodContext _foodContext;

        public int CurPage = 1 ;
        
        public static bool ContainsAllItems(IList<FoodItem> listA, IList<FoodItem> listB)
        {
            return !listB.Except(listA).Any();
        }
        public HomeController(ILogger<HomeController> logger, FoodContext foodContext)
        {
            _logger = logger;
            _foodContext = foodContext;
        }
        public void Seed_Category()
        {
            var c = _foodContext.Categories.ToList().Count;
            if (c == 0)
            {
                IList<Category> categories = new List<Category>
            {
                new Category("Sandwich"),
                new Category("Dish"),
                new Category("Appetizer"),
                new Category("Drink")
            };
                _foodContext.Categories.AddRange(categories);
                _foodContext.SaveChanges();
            }
        }
        public void Seed_Food()
        {
            IList<FoodItem> foods = new List<FoodItem>
            {
                new FoodItem("Foul", "Fooul", 1),
                new FoodItem("Falafel", "Falafel", 1),
                new FoodItem("Potatos", "Potatos", 1),
                new FoodItem("Egg", "Omlit", 2),
                new FoodItem("Salad", "Green Salad", 3),
                new FoodItem("Tea", "Red Tea", 4)
            };

            var c = _foodContext.FoodItems.ToList().Count;
            if (c == 0)
            {
                _foodContext.FoodItems.AddRange(foods);
                _foodContext.SaveChanges();
            }
        }
        
        public void FillCategoryDD()
        {
            var Options = _foodContext.Categories.Select(a => new SelectListItem { Value = a.CatId.ToString(), Text = a.CatName }).ToList();
            ViewBag.Options = Options;
        }

        public List<vwFoodItemCat> GetAllItems()
        {
            
            var query = from c in _foodContext.Categories
                        join i in _foodContext.FoodItems on c.CatId equals i.FoodCategoryId
                        let value = new { id = i.FoodId, i.Name, Description = i.Description, Category = c.CatName }
                        select value;
            var d = query.ToList();

            List<vwFoodItemCat> dd = new List<vwFoodItemCat>();

            foreach (var c in d)
            {
                dd.Add(new vwFoodItemCat { Id = c.id, Name = c.Name, Description = c.Description, Category = c.Category });
            }
            _logger.Log(LogLevel.Information, "Get All Items "); 
            return dd;
        }
        public IActionResult Index(string SortCol)
        {
            CurPage = 1;
            //TempData["txtFind"] = "";
            TempData["PgNm"] = 1;
            Seed_Category();
            Seed_Food();
            if (SortCol == "" || SortCol == null)
            {
                SortCol = "Name";
            }
            ViewBag.SortCol = SortCol; // "Name";
            //var dd = GetAllItems();
            //var dd = from d in  _foodContext.FoodItems select d;

            var dd = _foodContext.FoodItems.FromSqlRaw ("exec pgOfTbl 'FoodItems','" +  SortCol + "',5,"+ CurPage.ToString()).ToList();
            //dd = dd.OrderBy(b => b.Name);
            //FoodItem FD = new FoodItem();
            //FD.CategoryList = new SelectList(_foodContext.Categories.ToList(), "CatId", "CatName");
            FillCategoryDD();
            _logger.Log(LogLevel.Information, "Get Items Page " , CurPage.ToString());
            return View(dd);
            
        }
        public IActionResult NextPage() 
        {
            bool canNext = ( _foodContext.FoodItems.Count() > 5 * int.Parse(TempData["PgNm"].ToString())) ?true:false;
            if(canNext)
            { TempData["PgNm"] = int.Parse(TempData["PgNm"].ToString()) + 1; }
           
            CurPage = int.Parse(TempData["PgNm"].ToString());
            var dd = _foodContext.FoodItems.FromSqlRaw("exec pgOfTbl 'FoodItems','Name',5," + CurPage.ToString()).ToList();
            FillCategoryDD();
            return View("index",dd);
        }
        public IActionResult PreviousPage()
        {
            if(int.Parse(TempData["PgNm"].ToString()) > 1)
            { TempData["PgNm"] = int.Parse(TempData["PgNm"].ToString()) - 1; }
            
            CurPage = int.Parse(TempData["PgNm"].ToString());
            var dd = _foodContext.FoodItems.FromSqlRaw("exec pgOfTbl 'FoodItems','Name',5," + CurPage.ToString()).ToList();
            FillCategoryDD();
            return View("index", dd);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddFoodItem()
        {
            FillCategoryDD();
            FoodItem food = new FoodItem();
            return View(food);
        }

        [HttpPost]
        public IActionResult SaveFood(FoodItem food)
        {
            TempData["ErrMsg"] = "F";

            if (food.Name == null) 
            {
                TempData["ErrMsg"] = "T";

                FillCategoryDD();
                TempData["ErrorMsg"] = "Enter Missing data";
                return View("AddFoodItem");
            }

            RepoFood repoFood = new RepoFood(_foodContext);
            repoFood.AddFoodItem(food);
            return View();

        }
        
        public IActionResult Details (int id)
        {
            if (id == 0)
            {
                id = Int32.Parse( TempData["Id"].ToString());
            }
          
            ViewBag.Id = id;

            vwFoodItemCat vwFoodItemCat = (vwFoodItemCat)_foodContext.vwFoodItemCats.Where(d => d.Id == id).FirstOrDefault();

            return View(vwFoodItemCat);
        }

        public IActionResult Edit (int id)
        {
            FillCategoryDD();

            FoodItem Edit_Food = (FoodItem)_foodContext.FoodItems.Where(f => f.FoodId == id).FirstOrDefault();
            TempData["Id"] = Edit_Food.FoodId;
            TempData["Name"] = Edit_Food.Name;
            TempData["Description"] = Edit_Food.Description;
            TempData["FoodCategoryId"] = Edit_Food.FoodCategoryId;
            
            return View(Edit_Food);
        }

        [HttpPost]
        public IActionResult UpdateFood(FoodItem model)
        {
            TempData["Id"] = model.FoodId;
            ViewBag.ErrMsg = false;
           
            if (model.Name == null || model.Description == null)
            {
                ViewBag.ErrMsg = true;

                FillCategoryDD();
                TempData["SuccessMsg"] = "Correct Missing Data";
                return View("Edit", model);
            }
            RepoFood repoFood = new RepoFood(_foodContext);
            repoFood.UpdateFoodItem(model);
            return RedirectToAction("Details",  TempData["Id"]);
        }

        public IActionResult Delete (int id)
        {
            RepoFood repoFood = new RepoFood(_foodContext);
            repoFood.DeleteFoodItem(id);
            //RedirectToAction("Index",new { Model = GetAllItems() });
            var dd = _foodContext.FoodItems.ToList();
            return View("index",dd);
        }

        [HttpPost]
        public IActionResult Find(string txtFind)
        {
            var s = txtFind;
            if (s == null) 
            {
                ViewBag.err=true; 
                return RedirectToAction ("index");
            }
            FillCategoryDD();
            //string txt = TempData["txtFind"].ToString();
            RepoFood repoFood = new RepoFood(_foodContext);
            List<FoodItem> result = repoFood.Find(s);
            return View(result); 
        }

    }
}
