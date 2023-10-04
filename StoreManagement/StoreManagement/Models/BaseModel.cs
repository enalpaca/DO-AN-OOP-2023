using StoreManagement.IOFile;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StoreManagement.Models
{
    public class BaseModel
    {
        [Display(Name = "Mã:")]
        public string Id { get; set; }

        [Display(Name = "Tên:")]
        public virtual string Name { get; set; } = "";
        public DateTime CreateAt { get; set; }
        public BaseModel()
        {
            Id = Utils.GenerateString();
        }
    }
}
