namespace StoreManagement.Models
{
    public class ProductViewModel
    {
        public List<Product> ProductList { get; set; } = new List<Product>();
        public string SearchText { get; set; } = "";
        public string PageTitle { get; set; } = "";
    }
}