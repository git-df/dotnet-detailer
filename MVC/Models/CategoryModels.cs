namespace MVC.Models
{
    public class CategoryWithProductsPriceListModel
    {
        public string Name { get; set; } = string.Empty;
        public List<ProductInPriceListModel>? Products { get; set; }
    }
}
