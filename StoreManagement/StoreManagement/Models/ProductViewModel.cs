namespace StoreManagement.Models
{
   

    public class ProductViewModel
    {
        public List<Product> ProductList { get; set; } = new List<Product>();
    }

    public class ProductViewModelCreateOrEdit
    {
        public Product? Product { get; set; }
        public List<Category> CatList { get; set; } = new List<Category>();
    }
}
