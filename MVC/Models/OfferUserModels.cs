namespace MVC.Models
{
    public class OfferUserListModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }

    public class OfferUserAddModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }
}
