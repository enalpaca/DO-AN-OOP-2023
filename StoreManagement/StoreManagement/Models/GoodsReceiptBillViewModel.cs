namespace StoreManagement.Models
{
    public class GoodsReceiptBillViewModel
    {
        public List<GoodsReceiptBill> GoodsReceiptBillList { get; set; } = new List<GoodsReceiptBill>();
        public string SearchText { get; set; } = "";
    }
}
