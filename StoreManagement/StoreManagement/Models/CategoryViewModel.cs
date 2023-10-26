namespace StoreManagement.Models
{
    public class CategoryViewModel
    {
        public List<Category> CategoryRows { get; set; } = new List<Category>();
        public string SearchText { get; set; } = "";
        public int TotalRows { get; set; } = 0;
    }
}
