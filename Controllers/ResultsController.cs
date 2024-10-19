using AiDetector.Interfaces;
using AiDetector.Models;
using AiDetector.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using X.PagedList;
using MongoDB.Bson;

namespace AiDetector.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IMongoDbContext _dbContext;

        public ResultsController(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page = 1, bool showZeroFakeOnly = false)
        {
            const int pageSize = 10;
            var pageNumber = Math.Max(1, page ?? 1);

            var filter = showZeroFakeOnly
                ? Builders<ApiResponse>.Filter.Eq("Data.FakePercentage", 0)
                : Builders<ApiResponse>.Filter.Empty;

            var results = await _dbContext.ApiResponses
                .Find(filter)
                .Sort(Builders<ApiResponse>.Sort.Descending(x => x.CreatedAt))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalCount = await _dbContext.ApiResponses.CountDocumentsAsync(filter);

            var pagedResults = new StaticPagedList<ApiResponse>(results, pageNumber, pageSize, (int)totalCount);
            
            var viewModel = new ResultsIndexViewModel
            {
                PagedResults = pagedResults,
                ShowZeroFakeOnly = showZeroFakeOnly
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID");
            }

            var filter = Builders<ApiResponse>.Filter.Eq(x => x.Id, objectId);
            var result = await _dbContext.ApiResponses.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound("Item not found");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
