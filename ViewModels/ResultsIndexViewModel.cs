using AiDetector.Models;
using X.PagedList;

namespace AiDetector.ViewModels
{
    public class ResultsIndexViewModel
    {
        public IPagedList<ApiResponse>? PagedResults { get; set; }
        public bool ShowZeroFakeOnly { get; set; }
    }
}
