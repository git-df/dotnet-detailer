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

    public class OrderInListModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal Price { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDone { get; set; }
        public List<OrderProductInListModel>? orderProducts { get; set; }
    }
}
