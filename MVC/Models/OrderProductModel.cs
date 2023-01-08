﻿namespace MVC.Models
{
    public class OrderProductModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
