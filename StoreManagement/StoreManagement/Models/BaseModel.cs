using StoreManagement.IOFile;

namespace StoreManagement.Models
{
    public class BaseModel
    {
        public string Id { get; set; }
        public virtual string Name { get; set; } = "";
        public DateTime CreateAt { get; set; }
        public BaseModel()
        {
            Id = Utils.GenerateString();
        }
    }
}
