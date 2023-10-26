using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreManagement.IOFile;
using StoreManagement.Models;
using System.Drawing.Printing;

namespace StoreManagement.Controllers
{
    public class ProductController : BaseController
    {
        // GET: ProductController
        public ActionResult Statistics(string StatisticsType, int? page)
        {
            List<Product> ProductList = IOFile.IOFile.ReadProduct();
            ProductViewModel productViewModel = new ProductViewModel();

            if (StatisticsType == "inventory")
            {
                productViewModel.ProductList = ProductList.FindAll(x => x.Quantity > 0);
                productViewModel.PageTitle = "Danh sách sản phẩm tồn kho";
            }

            if (StatisticsType == "expired")
            {
                productViewModel.ProductList = ProductList.FindAll(x => x.ExpiredAt < DateTime.Now);
                productViewModel.PageTitle = "Danh sách sản phẩm hết hạn sử dụng";
            }

            int currentPage = page ?? 1;
            int totalRows = productViewModel.ProductList.Count;

            productViewModel.ProductList = productViewModel.ProductList.Skip((currentPage - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE).ToList();

            ViewBag.totalPage = IOFile.Utils.CalculateNumberOfPage(totalRows, PAGE_SIZE); ;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = totalRows;

            return View("Statistics", productViewModel);
        }

        // GET: ProductController
        public ActionResult Index(string searchText, int? page)
        {
            List<Product> ProductList = IOFile.IOFile.ReadProduct();
            ProductViewModel productViewModel = new ProductViewModel();
            if (searchText != null && searchText != "")
            {
                productViewModel.SearchText = searchText;
                ProductList = ProductList.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.Name, searchText) || Utils.StringLike(p.Company, searchText));
            }

            int currentPage = page ?? 1;
            int totalRows = ProductList.Count;

            ProductList = ProductList.Skip((currentPage - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE).ToList();

            ViewBag.totalPage = IOFile.Utils.CalculateNumberOfPage(totalRows, PAGE_SIZE); ;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = totalRows;

            productViewModel.ProductList = ProductList;
            productViewModel.TotalRows = totalRows;

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

            ViewBag.Categories = ListCategory.ToArray();

            return View("Create", new Product());
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
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            List<Invoice> ReadInvoice = IOFile.IOFile.ReadInvoice();

            int productIndex = ReadListProduct.FindIndex(x => x.Id == id);

            if (productIndex < 0)
            {
                SetAlertError(ErrorMessage.PRODUCT_NOT_FOUND);
                return Redirect("/Product");
            }

            bool flag = false;

            foreach (Invoice invoice in ReadInvoice)
            {
                foreach (InvoiceProduct invoiceProduct in invoice.ProductItems)
                {
                    if (invoiceProduct.Id == ReadListProduct[productIndex].Id)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            if (flag == true)
            {
                SetAlertError(ErrorMessage.PRODUCT_EXISTING_IN_INVOICE);
            }
            else
            {
                ReadListProduct.RemoveAt(productIndex);
                IOFile.IOFile.SaveProducts(ReadListProduct);

                SetAlertError(ErrorMessage.DELETED_SUCCESS);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
