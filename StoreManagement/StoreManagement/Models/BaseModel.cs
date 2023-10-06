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
        [Display(Name = "Ngày tạo:")]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public BaseModel()
        {
            Id = Utils.GenerateString();
        }
    }
}
