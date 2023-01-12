namespace MVC.Models
{
    public class PromocodeInListModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductInPromocodeListModel? Product { get; set; }
    }

    public class PromocodeAddModel
    {
        public int ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
