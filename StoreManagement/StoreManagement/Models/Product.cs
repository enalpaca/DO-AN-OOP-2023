using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class Product : BaseModel
    {
        [Display(Name = "Tên sản phẩm:")]
        public override string Name { get; set; } = "";

        [Display(Name = "Ngày hết hạn:")]
        public DateTime ExpiredAt { get; set; }

        [Display(Name = "Công ty sản xuất:")]
        public string Company { get; set; } = "";

        [Display(Name = "Loại hàng:")]
        public string CategoryId { get; set; } = "";

        [Display(Name = "Số lượng hàng:")]
        public int Quantity { get; set; } = 0;

        [Display(Name = "Ngày sản xuất:")]
        public DateTime ProductionDate { get; set; }

        [Display(Name = "Giá:")]
        public long Price { get; set; } = 0;

        [field: NonSerialized]
        public Category? Category { get; set; }
    }
}
