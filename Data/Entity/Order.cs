using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public decimal Price { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDone { get; set; }
    }
}
