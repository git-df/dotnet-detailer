using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class Promocode
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
