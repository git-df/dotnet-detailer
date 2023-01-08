using Data.Entity;

namespace MVC.Models
{
    public class OrderProductWithCategoryListModel
    {
        public List<CategoryWithProductsOrderModel>? CategoryWithProducts { get; set; }
        public string Code { get; set; } = String.Empty;
    }

    public class OrderSummaryModel
    {
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public List<OrderProductModel>? OrderProducts { get; set; }
    }
}
