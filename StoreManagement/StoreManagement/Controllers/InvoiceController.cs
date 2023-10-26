using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreManagement.IOFile;
using StoreManagement.Models;
using System.Drawing.Printing;

namespace StoreManagement.Controllers
{
    public class InvoiceController : BaseController
    {
        // GET: InvoiceController
        public ActionResult Index(string searchText, int? page)
        {
            List<Invoice> invoices = IOFile.IOFile.ReadInvoice();
            InvoiceViewModel invoiceViewModel = new InvoiceViewModel();
            if (searchText != null && searchText != "")
            {
                invoiceViewModel.SearchText = searchText;
                invoices = invoices.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.CustomerName, searchText) || Utils.StringLike(p.CustomerPhone, searchText));
            }

            int currentPage = page ?? 1;
            int totalRows = invoices.Count;

            invoices = invoices.Skip((currentPage - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE).ToList();

            ViewBag.totalPage = IOFile.Utils.CalculateNumberOfPage(totalRows, PAGE_SIZE); ;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = totalRows;

            invoiceViewModel.InvoiceList = invoices;
            invoiceViewModel.TotalRows = totalRows;

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
                SetAlertInfo(ErrorMessage.CREATED_SUCCESS);
                return Redirect("Edit/" + newInvoice.Id);
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
        [HttpPost("Invoice/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Invoice invoiceUpdated)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
                foreach (Invoice item in ReadListInvoice)
                {
                    if (item.Id == invoiceUpdated.Id)
                    {
                        item.CustomerAddress = invoiceUpdated.CustomerAddress;
                        item.CustomerName = invoiceUpdated.CustomerName;
                        item.CustomerPhone = invoiceUpdated.CustomerPhone;
                    }
                }
                IOFile.IOFile.SaveInvoices(ReadListInvoice);
                return Redirect(id);
            }
            catch
            {
                return View();
            }
        }

        // POST: InvoiceController/Delete/5
        [HttpPost("Invoice/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
                int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == id);

                if (invoiceIndex >= 0)
                {
                    if (ReadListInvoice[invoiceIndex].ProductItems.Count == 0)
                    {
                        ReadListInvoice.RemoveAt(invoiceIndex);
                        IOFile.IOFile.SaveInvoices(ReadListInvoice);

                        SetAlertInfo(ErrorMessage.DELETED_SUCCESS);
                    }
                    else
                    {
                        SetAlertError(ErrorMessage.INVOICE_HAS_PRODUCT);
                    }
                }

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
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            int currentProductIndex = ReadListProduct.FindIndex(p => p.Id == newInvoiceProductItem.Id);
            if (currentProductIndex < 0)
            {
                SetAlertError(ErrorMessage.PRODUCT_NOT_FOUND);
                return Redirect("/Invoice/Edit/" + invoiceID);
            }

            if (ReadListProduct[currentProductIndex].Quantity < newInvoiceProductItem.Quantity)
            {
                SetAlertError(ErrorMessage.EXCEED_QUANTITY);
                return Redirect("/Invoice/Edit/" + invoiceID);
            }

            int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == invoiceID);

            if (invoiceIndex < 0)
            {
                SetAlertError(ErrorMessage.INVOICE_NOT_FOUND);
                return Redirect("/Invoice/Edit/" + invoiceID);
            }

            InvoiceProduct? invoiceProduct = ReadListInvoice[invoiceIndex].ProductItems.Find(p => p.Id == newInvoiceProductItem.Id);
            if (invoiceProduct != null)
            {
                SetAlertError(ErrorMessage.INVOICE_PRODUCT_FOUND);
                return Redirect("/Invoice/Edit/" + invoiceID);
            }

            ReadListInvoice[invoiceIndex].ProductItems.Add(newInvoiceProductItem);
            ReadListProduct[currentProductIndex].Quantity -= newInvoiceProductItem.Quantity;

            IOFile.IOFile.SaveInvoices(ReadListInvoice);
            IOFile.IOFile.SaveProducts(ReadListProduct);

            SetAlertInfo(ErrorMessage.CREATED_SUCCESS);
            return Redirect("/Invoice/Edit/" + invoiceID);
        }

        // POST: InvoiceController/Delete/5/ProductItem
        [HttpPost("Invoice/Delete/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("DeleteInvoiceProductItem")]
        public ActionResult DeleteInvoiceProductItem(string InvoiceCode, string ProductCode)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == InvoiceCode);

            if (invoiceIndex >= 0)
            {
                int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.Id == ProductCode);

                if (invoiceProducItemIndex >= 0)
                {
                    int productIndex = ReadListProduct.FindIndex(p => p.Id == ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].Id);

                    if (productIndex >= 0)
                    {
                        ReadListProduct[productIndex].Quantity += ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].Quantity;
                    }

                    ReadListInvoice[invoiceIndex].ProductItems.RemoveAt(invoiceProducItemIndex);

                    IOFile.IOFile.SaveProducts(ReadListProduct);
                    IOFile.IOFile.SaveInvoices(ReadListInvoice);
                    SetAlertInfo(ErrorMessage.DELETED_SUCCESS);
                }
            }

            return Redirect("/Invoice/Edit/" + InvoiceCode);

        }

        // POST: InvoiceController/Update/5/ProductItem
        [HttpPost("Invoice/Update/{InvoiceCode}/ProductItem/{ProductCode}")]
        [ActionName("UpdateInvoiceProductItem")]
        public ActionResult UpdateInvoiceProductItem(string InvoiceCode, string ProductCode, InvoiceProduct updatedInvoiceProduct)
        {
            List<Invoice> ReadListInvoice = IOFile.IOFile.ReadInvoice();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            int currentProductIndex = ReadListProduct.FindIndex(p => p.Id == ProductCode);
            int invoiceIndex = ReadListInvoice.FindIndex(x => x.Id == InvoiceCode);

            if (currentProductIndex < 0)
            {
                SetAlertError(ErrorMessage.PRODUCT_NOT_FOUND);
                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }
            if (invoiceIndex < 0)
            {
                SetAlertError(ErrorMessage.INVOICE_NOT_FOUND);
                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }


            int invoiceProducItemIndex = ReadListInvoice[invoiceIndex].ProductItems.FindIndex(x => x.Id == ProductCode);

            if (invoiceProducItemIndex < 0)
            {
                SetAlertError(ErrorMessage.INVOICE_PRODUCT_NOT_FOUND);
                return Redirect("/Invoice/Edit/" + InvoiceCode);
            }

            int delta = updatedInvoiceProduct.Quantity - ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].Quantity;

            ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].Quantity = updatedInvoiceProduct.Quantity;

            // user tăng sl trong hóa đơn
            if (delta > 0)
            {
                if (delta > ReadListProduct[currentProductIndex].Quantity)
                {
                    SetAlertError(ErrorMessage.EXCEED_QUANTITY);
                    return Redirect("/Invoice/Edit/" + InvoiceCode);
                }

                // giảm sl tồn kho
                ReadListProduct[currentProductIndex].Quantity -= delta;
            }

            // user giảm sl trong hóa đơn
            if (delta < 0)
            {
                // tăng sl tồn kho
                ReadListProduct[currentProductIndex].Quantity += Math.Abs(delta);
            }

            // user đã giảm sl tới 0, ta sẽ xóa sp khỏi hóa đơn
            if (ReadListInvoice[invoiceIndex].ProductItems[invoiceProducItemIndex].Quantity == 0)
            {
                ReadListInvoice[invoiceIndex].ProductItems.RemoveAt(invoiceProducItemIndex);
            }

            IOFile.IOFile.SaveProducts(ReadListProduct);
            IOFile.IOFile.SaveInvoices(ReadListInvoice);

            SetAlertInfo(ErrorMessage.UPDATED_SUCCESS);
            return Redirect("/Invoice/Edit/" + InvoiceCode);
        }
    }
}
