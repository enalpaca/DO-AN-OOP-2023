using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.IOFile;
using StoreManagement.Models;

namespace StoreManagement.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController
        [HttpGet("Category")]
        public ActionResult Index(string searchText)
        {
            List<Category> categories = IOFile.IOFile.ReadCategory();
            CategoryViewModel catViewModel = new CategoryViewModel();

            if (searchText != null && searchText != "")
            {
                catViewModel.SearchText = searchText;
                categories = categories.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.Name, searchText));
            }

            catViewModel.CategoryList = categories;

            return View("Index", catViewModel);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View("CreateCategory");
        }

        // POST: CategoryController/Create
        [HttpPost("Category/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category newCat)
        {
            try
            {
                List<Category> ReadListCat = IOFile.IOFile.ReadCategory();

                ReadListCat.Add(newCat);
                IOFile.IOFile.SaveCategories(ReadListCat);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/AFWE5
        [HttpGet("Category/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            List<Category> ReadListCat = IOFile.IOFile.ReadCategory();
            Category? cat = ReadListCat.Find(x => x.Id == id);

            return View("EditCategory", cat);
        }

        // POST: CategoryController/Edit/5
        [HttpPost("Category/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Category updatedCat)
        {
            try
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

                int categoryIndex = ReadListCategory.FindIndex(x => x.Id == updatedCat.Id);

                if (categoryIndex >= 0)
                {
                    ReadListCategory.RemoveAt(categoryIndex);
                    ReadListCategory.Insert(categoryIndex, updatedCat);
                    IOFile.IOFile.SaveCategories(ReadListCategory);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost("Category/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();
                int categoryIndex = ReadListCategory.FindIndex(x => x.Id == id);

                if (categoryIndex >= 0)
                {
                    ReadListCategory.RemoveAt(categoryIndex);
                    IOFile.IOFile.SaveCategories(ReadListCategory);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
