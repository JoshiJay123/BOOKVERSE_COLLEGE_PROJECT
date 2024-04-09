using COLLEGE_PROJECT.Model;
using MongoDB.Driver;

namespace COLLEGE_PROJECT.Data
{
    public class CartContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("carts");

        public CartContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDbUrl"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("ASPDB");
        }
    }
}
