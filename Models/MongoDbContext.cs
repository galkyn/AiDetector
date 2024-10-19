using AiDetector.Configurations;
using AiDetector.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AiDetector.Models
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<ApiResponse> ApiResponses =>
            _database.GetCollection<ApiResponse>("ApiResponses");
    }
}
