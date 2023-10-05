namespace StoreManagement.Models
{
    public class CategoryViewModel
    {
        public List<Category> CategoryList { get; set; } = new List<Category>();
        public string SearchText { get; set; } = "";
    }
}
