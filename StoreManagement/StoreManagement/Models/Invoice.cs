using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StoreManagement.Models
{
    public class InvoiceProduct : BaseModel
    {
        [Display(Name = "Giá:")]
        public long Price { get; set; } = 0;

        [Display(Name = "Số lượng:")]
        public int Quantity { get; set; } = 0;
    }
    public class Invoice : BaseModel
    {
        [Display(Name = "Tên khách hàng:")]
        public string CustomerName { get; set; } = "";

        [Display(Name = "Địa chỉ:")]
        public string CustomerAddress { get; set; } = "";

        [Display(Name = "Số điện thoại:")]
        public string CustomerPhone { get; set; } = "";
        public List<InvoiceProduct> ProductItems { get; set; } = new List<InvoiceProduct>();
    }
}
