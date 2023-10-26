using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.Controllers
{
    public class BaseController : Controller
    {

        protected int PAGE_SIZE = 5;

        // 1-success, 2-warning, 3-danger, 4-info
        // https://mianliencoding.com/chi-tiet-bai-viet-tao-alert-trong-asp-net-mvc-su-dung-bootstrap-28

        protected void SetAlertInfo(string message)
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = "alert-info";
        }

        protected void SetAlertError(string message)
        {
            TempData["AlertMessage"] = message;
            TempData["AlertType"] = "alert-danger";
        }
        protected void SetAlert(string message, int type)
        {
            TempData["AlertMessage"] = message;
            if (type == 1)
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == 2)
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == 3)
            {
                TempData["AlertType"] = "alert-danger";
            }
            else
            {
                TempData["AlertType"] = "alert-info";
            }
        }
    }
}
