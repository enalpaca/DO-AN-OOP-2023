using Microsoft.AspNetCore.Mvc;
using StoreManagement.IOFile;
using StoreManagement.Models;

namespace StoreManagement.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public ActionResult Index(string searchText)
        {
            List<Product> ProductList = IOFile.IOFile.ReadProduct();
            ProductViewModel productViewModel = new ProductViewModel();
            if (searchText != null && searchText != "")
            {
                productViewModel.SearchText = searchText;
                ProductList = ProductList.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.Name, searchText));
            }
            productViewModel.ProductList = ProductList;

            return View("Index", productViewModel);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            List<Category> ListCategory = IOFile.IOFile.ReadCategory();
            ProductViewModelCreateOrEdit productCreateViewModel = new ProductViewModelCreateOrEdit();
            productCreateViewModel.ListCategory = ListCategory;

            return View("Create", productCreateViewModel);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product newProduct)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                ReadListProduct.Add(newProduct);
                IOFile.IOFile.SaveProducts(ReadListProduct);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        [HttpGet("Product/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            List<Category> ReadListCategory = IOFile.IOFile.ReadCategory();

            Product? product = ReadListProduct.Find(x => x.Id == id);
          
            ViewBag.Categories = ReadListCategory.ToArray();
            return View("Edit", product);
        }

        // POST: ProductController/Edit/5
        [HttpPost("Product/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Product updateProduct)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                int productIndex = ReadListProduct.FindIndex(x => x.Id == updateProduct.Id);

                if (productIndex >= 0)
                {
                    ReadListProduct.RemoveAt(productIndex);
                    ReadListProduct.Insert(productIndex, updateProduct);
                    IOFile.IOFile.SaveProducts(ReadListProduct);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
                int productIndex = ReadListProduct.FindIndex(x => x.Id == id);

                if (productIndex >= 0)
                {
                    ReadListProduct.RemoveAt(productIndex);
                    IOFile.IOFile.SaveProducts(ReadListProduct);
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
