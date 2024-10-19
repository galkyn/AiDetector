using AiDetector.Configurations;
using AiDetector.Interfaces;
using AiDetector.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace AiDetector.Services
{
    public class ApiService : IApiService
    {
        private readonly IMongoDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public ApiService(IMongoDbContext dbContext, HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiSettings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));

            _httpClient.DefaultRequestHeaders.Add("ApiKey", _apiSettings.ApiKey);
        }

        public async Task<ApiResponse> DetectTextAsync(string inputText)
        {
            var requestData = new { input_text = inputText };

            var response = await _httpClient.PostAsJsonAsync(_apiSettings.Url, requestData);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize API response.");
            }

            var apiResponse = new ApiResponse
            {
                InputText = inputText,
                Data = new ApiData
                {
                    TextWords = result.Data?.TextWords ?? 0,
                    AiWords = result.Data?.AiWords ?? 0,
                    FakePercentage = result.Data?.FakePercentage ?? 0
                },
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.ApiResponses.InsertOneAsync(apiResponse);

            return apiResponse;
        }
    }
}
