namespace StoreManagement.Models
{
    public class ErrorMessage
    {
        public static string CREATED_SUCCESS = "Thêm thành công";
        public static string DELETED_SUCCESS = "Xóa thành công";
        public static string UPDATED_SUCCESS = "Cập nhật thành công";
        public static string PRODUCT_EXISTING_IN_INVOICE = "Sản phẩm tồn tại trong hóa đơn xuất. KHÔNG thể xóa!";
        public static string PRODUCT_NOT_FOUND = "Sản phẩm không tồn tại!";
        public static string EXCEED_QUANTITY = "Vượt quá số lượng tồn kho";
        public static string INVOICE_NOT_FOUND = "Hóa đơn không tồn tại!";
        public static string INVOICE_PRODUCT_FOUND = "Sản phẩm đã tồn tại. Vui lòng cập nhật số lượng bên dưới!";
        public static string INVOICE_PRODUCT_NOT_FOUND = "Sản phẩm không tồn tại.";
        public static string INVOICE_HAS_PRODUCT = "Hóa đơn có sản phẩm bên trong, vui lòng xóa sản phẩm trước";
    }
}
