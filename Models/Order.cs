using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace productManagementSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}