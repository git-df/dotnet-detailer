namespace MVC.Models
{
    public class CategoryWithProductsPriceListModel
    {
        public string Name { get; set; } = string.Empty;
        public List<ProductInPriceListModel>? Products { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CategoryWithProductsOrderModel
    {
        public string Name { get; set; } = string.Empty;
        public List<ProductInOrderModel>? Products { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class CategoryListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class CategoryAddModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
