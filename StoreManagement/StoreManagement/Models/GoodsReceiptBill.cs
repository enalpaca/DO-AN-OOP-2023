namespace StoreManagement.Models
{
    public class GoodsReceiptBillProduct : BaseModel
    {
        public string Unit { get; set; } = "";
        public long Price { get; set; } = 0;
        public string Company { get; set; } = "";
        public int Quantity { get; set; } = 0;
    }
    public class GoodsReceiptBill : BaseModel
    {
        public string Deliver { get; set; } = "";
        public List<GoodsReceiptBillProduct> ProductItems { get; set; } = new List<GoodsReceiptBillProduct>();

    }
}
