namespace StoreManagement.Models
{
    public class GoodsReceiptBillProduct:BaseModel
    {
        public string Unit { get; set; } = "";
        public string Price { get; set; } = "";
        public string Company { get; set; } = "";
    }
    public class GoodsReceiptBill:BaseModel
    {
        public string Deliver { get; set; } = "";
        public List<GoodsReceiptBillProduct> ProductItems { get; set; } = new List<GoodsReceiptBillProduct>();

    }
}
