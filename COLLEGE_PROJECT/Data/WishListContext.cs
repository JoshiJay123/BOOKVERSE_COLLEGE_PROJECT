using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using COLLEGE_PROJECT.Model;

namespace COLLEGE_PROJECT.Data
{
    public class WishListContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<WishList> WishLists => _database.GetCollection<WishList>("wishlists");

        public WishListContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDbUrl"];
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("ASPDB");
        }   
    }
}
