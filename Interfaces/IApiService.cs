using AiDetector.Models;

namespace AiDetector.Interfaces
{
    public interface IApiService
    {
        Task<ApiResponse> DetectTextAsync(string inputText);
    }
}
