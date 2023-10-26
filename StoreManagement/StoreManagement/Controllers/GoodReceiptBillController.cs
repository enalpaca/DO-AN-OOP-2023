using Microsoft.AspNetCore.Mvc;
using StoreManagement.IOFile;
using StoreManagement.Models;

namespace StoreManagement.Controllers
{
    public class GoodReceiptBillController : BaseController
    {
        // GET: GoodReceiptBillController
        public ActionResult Index(string searchText, int? page)
        {
            List<GoodsReceiptBill> goodsReceiptBills = IOFile.IOFile.ReadGoodsReceiptBill();
            GoodsReceiptBillViewModel goodsReceiptBillViewModel = new GoodsReceiptBillViewModel();
            if (searchText != null && searchText != "")
            {
                goodsReceiptBillViewModel.SearchText = searchText;
                goodsReceiptBills = goodsReceiptBills.FindAll(p => Utils.StringLike(p.Id, searchText) || Utils.StringLike(p.ProductItem.Name, searchText) || Utils.StringLike(p.ProductItem.Provider, searchText));
            }

            int currentPage = page ?? 1;
            int totalRows = goodsReceiptBills.Count;

            goodsReceiptBills = goodsReceiptBills.Skip((currentPage - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE).ToList();

            ViewBag.totalPage = IOFile.Utils.CalculateNumberOfPage(totalRows, PAGE_SIZE); ;
            ViewBag.currentPage = currentPage;
            ViewBag.totalRow = totalRows;

            goodsReceiptBillViewModel.TotalRows = totalRows;
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

            List<GoodsReceiptBill> ReadListGoodsReceiptBill = IOFile.IOFile.ReadGoodsReceiptBill();
            List<Product> ReadListProduct = IOFile.IOFile.ReadProduct();

            int selectedProductIndex = ReadListProduct.FindIndex(p => p.Id == newGoodsReceiptBill.ProductItem.Id);

            if (selectedProductIndex >= 0)
            {
                ReadListProduct[selectedProductIndex].Quantity += newGoodsReceiptBill.ProductItem.Quantity;
            }

            ReadListGoodsReceiptBill.Add(newGoodsReceiptBill);

            IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);
            IOFile.IOFile.SaveProducts(ReadListProduct);

            SetAlertInfo(ErrorMessage.CREATED_SUCCESS);
            return Redirect("Index");

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
                        // item.ProductItem.Name = goodsReceiptBillUpdated.ProductItem.Name;
                        item.ProductItem.Price = goodsReceiptBillUpdated.ProductItem.Price;
                        item.Deliver = goodsReceiptBillUpdated.Deliver;
                        // item.ProductItem.Quantity = goodsReceiptBillUpdated.ProductItem.Quantity;
                        item.ProductItem.Provider = goodsReceiptBillUpdated.ProductItem.Provider;
                    }
                }
                IOFile.IOFile.SaveGoodsReceiptBills(ReadListGoodsReceiptBill);

                SetAlertInfo(ErrorMessage.UPDATED_SUCCESS);
                return Redirect(id);
            }
            catch
            {
                return View();
            }
        }

        // POST: GoodReceiptBillController/Delete/5
        // chưa hỗ trợ vì xóa cần phải trừ đi sp tồn kho
        /*[HttpPost("GoodReceiptBill/Delete/{id}")]
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
        }*/
    }
}
