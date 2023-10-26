using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class GoodsReceiptBillProduct : BaseModel
    {
        [Display(Name = "Tên hàng hóa:")]
        public override string Name { get; set; } = "";

        [Display(Name = "Giá:")]
        public long Price { get; set; } = 0;

        [Display(Name = "Nhà cung cấp:")]
        public string Provider { get; set; } = "";

        [Display(Name = "Số lượng:")]
        public int Quantity { get; set; } = 0;
    }
    public class GoodsReceiptBill : BaseModel
    {
        [Display(Name = "Người giao hàng:")]
        public string Deliver { get; set; } = "";
        public GoodsReceiptBillProduct ProductItem { get; set; } = new GoodsReceiptBillProduct();

    }
}
