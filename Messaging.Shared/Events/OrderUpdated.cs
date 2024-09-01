using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Shared.Events
{
    public class OrderUpdated
    {
        public string OrderId { get; set; } // OrderId as int
        public int CustomerId { get; set; } // Foreign key reference to Customer
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
