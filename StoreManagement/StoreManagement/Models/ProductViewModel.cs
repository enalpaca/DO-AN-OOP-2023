namespace StoreManagement.Models
{
    public class ProductViewModel
    {
        public List<Product> ProductList { get; set; } = new List<Product>();
    }

    public class ProductViewModelCreateOrEdit: Product
    {
        public List<Category> ListCategory { get; set; } = new List<Category>();
    }
}