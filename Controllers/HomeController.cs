using AiDetector.Interfaces;
using AiDetector.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AiDetector.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Submit(HomeViewModel model)
        {
            if (string.IsNullOrEmpty(model.InputText))
            {
                ModelState.AddModelError("", "Введите текст.");
                return View("Index", model);
            }

            var apiResponse = await _apiService.DetectTextAsync(model.InputText);

            var resultViewModel = new ResultViewModel
            {
                InputText = apiResponse.InputText,
                Data = apiResponse.Data
            };

            return View("Result", resultViewModel);
        }
    }
}
