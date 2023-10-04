namespace StoreManagement.Models
{
    public class InvoiceProduct:BaseModel
    {
        public long Price { get; set; } = 0;
    }
    public class Invoice:BaseModel
    {
        public string CustomerName { get; set; } = "";
        public string CustomerAddress { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public List<InvoiceProduct> ProductItems { get; set; } = new List<InvoiceProduct>();

    }
}
