using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Model
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; } // MongoDB ObjectId stored as a string

        public int CustomerId { get; set; } // Foreign key reference to Customer
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
