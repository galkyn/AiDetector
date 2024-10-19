using AiDetector.Models;
using MongoDB.Driver;

namespace AiDetector.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<ApiResponse> ApiResponses { get; }
    }
}
