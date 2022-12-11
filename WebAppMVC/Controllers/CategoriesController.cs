using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly FoodContext _context;

        public CategoriesController(FoodContext context)
        {
            _context = context;
        }
        public int CurPage = 1;

        // GET: Categories
        public IActionResult Index() // async Task<IActionResult> Index()
        {
            TempData["PgNm"] = CurPage;
            var dd = _context.Categories.FromSqlRaw("exec pgOfTbl 'Categories','CatId',5," + CurPage.ToString()).ToList();
            if (dd.Any())
            {
                return View(dd);
            }
            else
                return View(null); //return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatName,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,CatName,Description")] Category category)
        {
            if (id != category.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TempData["ErrMsg"] = "F"; 
            var category = await _context.Categories.FindAsync(id);
            bool chekHasRelated = (_context.FoodItems.Where(i => i.FoodCategoryId == id).ToList().Count > 0) ? true : false;
            if (chekHasRelated != true )
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMsg"] = "Can't Delete Cagtegry has related food items Data";
                TempData["ErrMsg"] = "T";
                return RedirectToAction(nameof(Delete));
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CatId == id);
        }
        public IActionResult Find(string txtFind)
        {
            var s = txtFind;
            if (s == null)
            {
                ViewBag.err = true;
                return RedirectToAction("index");
            }
            CategoryRepo repoCat = new CategoryRepo(_context);
            List<Category> result = repoCat.Find(s);
            return View(result);
        }
        public IActionResult NextPage()
        {
            bool canNext = (_context.Categories.Count() > 5 * int.Parse(TempData["PgNm"].ToString())) ? true : false;
            if (canNext)
            { TempData["PgNm"] = int.Parse(TempData["PgNm"].ToString()) + 1; }

            CurPage = int.Parse(TempData["PgNm"].ToString());
            var dd = _context.Categories.FromSqlRaw("exec pgOfTbl 'Categories','CatId',5," + CurPage.ToString()).ToList();
            return View("index", dd);
        }
        public IActionResult PreviousPage()
        {
            if (int.Parse(TempData["PgNm"].ToString()) > 1)
            { TempData["PgNm"] = int.Parse(TempData["PgNm"].ToString()) - 1; }

            CurPage = int.Parse(TempData["PgNm"].ToString());
            var dd = _context.Categories.FromSqlRaw("exec pgOfTbl 'Categories','CatId',5," + CurPage.ToString()).ToList();
            return View("index", dd);
        }
        public IActionResult CatItems(int? id)
        {
            Category cat = (Category)_context.Categories.Where(c => c.CatId == id).FirstOrDefault();
            ViewBag.Title = cat.CatName;
            var dd = _context.FoodItems.Where(d => d.FoodCategoryId == id).ToList();
            return View(dd);
        }
    }
}
