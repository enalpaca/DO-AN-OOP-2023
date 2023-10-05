using Microsoft.AspNetCore.Mvc;
using StoreManagement.IOFile;
using StoreManagement.Models;

namespace StoreManagement.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: InvoiceController
        public ActionResult Index(string searchText)
        {
            List<Invoice> invoices = IOFile.IOFile.ReadInvoice();
            InvoiceViewModel invoiceViewModel = new InvoiceViewModel();
            if (searchText != null && searchText != "")
            {
                invoiceViewModel.SearchText = searchText;
                invoices = invoices.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.CustomerName, searchText) || Utils.StringLike(p.CustomerPhone, searchText));
            }
            invoiceViewModel.InvoiceList = invoices;

            return View("Index", invoiceViewModel);
        }

        // GET: InvoiceController/Details/5
        [HttpGet("Invoice/Details/{id}")]
        public ActionResult Details(string id)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            Invoice? invoice = ReadListInvoice.Find(x => x.Id == id);

            return View("Details", invoice);
        }

        // GET: InvoiceController/Create
        public ActionResult Create()
        {
            return View("CreateInvoice");
        }

        // POST: InvoiceController/Create
        [HttpPost("Invoice/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Invoice newInvoice)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                ReadListInvoice.Add(newInvoice);
                IOFile.IOFile.SaveInvoices(ReadListInvoice);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InvoiceController/Edit/5
        [HttpGet("Invoice/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

            Invoice? invoice = ReadListInvoice.Find(x => x.Id == id);

            ViewBag.ProductList = ReadListProduct.ToArray();
            return View("EditInvoice", invoice);
        }

        // POST: InvoiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InvoiceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InvoiceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: InvoiceController/Create/5/ProductItem
        [HttpPost("Invoice/Create/{invoiceID}/ProductItem")]
        [ValidateAntiForgeryToken]
        public ActionResult AddInvoiceProductItem(string invoiceID, InvoiceProduct newInvoiceProductItem)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == invoiceID);

                if (invoiceIndex >= 0)
                {
                    int productItemsIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(y => y.Id == newInvoiceProductItem.Id);
                    if (productItemsIndex >= 0)
                    {
                        ReadListInvoice[invoiceIndex].ProductItems[productItemsIndex].Quantity += newInvoiceProductItem.Quantity;
                    }
                    else
                    {
                        ReadListInvoice[invoiceIndex].ProductItems.Add(newInvoiceProductItem);
                    }
                    IOFile.IOFile.SaveInvoices(ReadListInvoice);

                }

                return Redirect("/Invoice/Edit/" + invoiceID);
            }
            catch
            {
                return View();
            }
        }

        // POST: InvoiceController/Delete/5/ProductItem
        [HttpPost("Invoice/Delete/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("DeleteInvoiceProductItem")]
        public ActionResult DeleteInvoiceProductItem(string InvoiceCode, string ProductCode)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == InvoiceCode);

                if (invoiceIndex >= 0)
                {
                    int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.Id == ProductCode);
                    if (invoiceProducItemIndex >= 0)
                    {
                        ReadListInvoice[invoiceIndex].ProductItems.RemoveAt(invoiceProducItemIndex);
                        IOFile.IOFile.SaveInvoices(ReadListInvoice);
                    }
                }

                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }
            catch
            {
                return View();
            }
        }

        // POST: InvoiceController/Update/5/ProductItem
        [HttpPost("Invoice/Update/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("UpdateInvoiceProductItem")]
        public ActionResult UpdateInvoiceProductItem(string InvoiceCode, string ProductCode, InvoiceProduct updatedInvoiceProduct)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();

                int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == InvoiceCode);

                if (invoiceIndex >= 0)
                {
                    int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.Id == ProductCode);
                    if (invoiceProducItemIndex >= 0)
                    {
                        ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].Quantity = updatedInvoiceProduct.Quantity;
                        IOFile.IOFile.SaveInvoices(ReadListInvoice);
                    }
                }

                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }
            catch
            {
                return View();
            }
        }
    }
}
