using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.IOFile;
using StoreManagement.Models;

namespace StoreManagement.Controllers
{
    public class GoodReceiptBillController : Controller
    {
        // GET: GoodReceiptBillController
        public ActionResult Index(string searchText)
        {
            List<GoodsReceiptBill> goodsReceiptBills = IOFile.IOFile.ReadGoodsReceiptBill();
            GoodsReceiptBillViewModel goodsReceiptBillViewModel = new GoodsReceiptBillViewModel();
            if (searchText != null && searchText != "")
            {
                goodsReceiptBillViewModel.SearchText = searchText;
                goodsReceiptBills = goodsReceiptBills.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.ProductItem.Name, searchText) || Utils.StringLike(p.ProductItem.Company, searchText));
            }
            goodsReceiptBillViewModel.GoodsReceiptBillList = goodsReceiptBills;
            return View("Index", goodsReceiptBillViewModel);
        }

        // GET: GoodReceiptBillController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GoodReceiptBillController/Create
        public ActionResult Create()
        {
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            ViewBag.ListProduct = ReadListProduct;
            return View("Create");
        }

        // POST: GoodReceiptBillController/Create
        [HttpPost("GoodReceiptBill/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GoodsReceiptBill newGoodsReceiptBill)
        {
            try
            {
                List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
                List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

                foreach (Product productItem in ReadListProduct)
                {
                    if (productItem.Id == newGoodsReceiptBill.ProductItem.Id)
                    {
                        productItem.Quantity += newGoodsReceiptBill.ProductItem.Quantity;
                    }
                }

                ReadListGoodsReceiptBill.Add(newGoodsReceiptBill);

                IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
                IOFile.IOFile.SaveProducts(ReadListProduct);

                return Redirect("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GoodReceiptBillController/Edit/5
        [HttpGet("GoodReceiptBill/Edit/{id}")]
        public ActionResult Edit(string id)
        {
            List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();

            GoodsReceiptBill? goodsReceiptBill = ReadListGoodsReceiptBill.Find(x => x.Id == id);

            return View("Edit", goodsReceiptBill);
        }

        // POST: GoodReceiptBillController/Edit/5
        [HttpPost("GoodReceiptBill/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, GoodsReceiptBill goodsReceiptBillUpdated)
        {
            try
            {
                List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
                foreach (GoodsReceiptBill item in ReadListGoodsReceiptBill)
                {
                    if (item.Id == goodsReceiptBillUpdated.Id)
                    {
                        item.ProductItem.Name = goodsReceiptBillUpdated.ProductItem.Name;
                        item.ProductItem.Price = goodsReceiptBillUpdated.ProductItem.Price;
                        item.Deliver = goodsReceiptBillUpdated.Deliver;
                        item.ProductItem.Unit = goodsReceiptBillUpdated.ProductItem.Unit;
                        item.ProductItem.Quantity = goodsReceiptBillUpdated.ProductItem.Quantity;
                        item.ProductItem.Company = goodsReceiptBillUpdated.ProductItem.Company;
                    }
                }
                IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
                return Redirect(id);
            }
            catch
            {
                return View();
            }
        }

        // POST: GoodReceiptBillController/Delete/5
        [HttpPost("GoodReceiptBill/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
                int goodReceiptBillIndex = ReadListGoodsReceiptBill.FindIndex(x => x.Id == id);

                if (goodReceiptBillIndex >= 0)
                {
                    ReadListGoodsReceiptBill.RemoveAt(goodReceiptBillIndex);
                    IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
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
