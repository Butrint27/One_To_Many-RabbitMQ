using MongoDB.Driver;
using OrderService.Model;

namespace OrderService.Data
{
    public class OrderDbContext
    {
        private readonly IMongoDatabase _database;

        public OrderDbContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("OrderDb");
        }

        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
    }
}
