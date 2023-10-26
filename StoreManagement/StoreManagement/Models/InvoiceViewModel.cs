namespace StoreManagement.Models
{
    public class InvoiceViewModel
    {
        public List<Invoice> InvoiceList { get; set; } = new List<Invoice>();
        public string SearchText { get; set; } = "";
        public int TotalRows { get; set; } = 0;
    }
}
