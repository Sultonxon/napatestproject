using Microsoft.AspNetCore.Mvc;

namespace NapaProjects.OnlineMarket.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public HistoryController(IHistoryRepository historyRepository, ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _historyRepository = historyRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View(_historyRepository.ProductHistory.Select(x =>
            {
                var y = (HistoryModel)x;
                y.RelatedCategoryName = _categoryRepository.Get(x.RelatedCategoryId.Value).Name;
                return y;
            }));
        }

        public IActionResult HistoryWithid(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ProductName = _productRepository.Get(id).Name;
            return View(_historyRepository.ProductHistory.Where(x => x.RelatedProductId == id).Select(x =>
            {
                var y = (HistoryModel)x;
                y.RelatedCategoryName = _categoryRepository.Get(x.RelatedCategoryId.Value).Name;
                return y;
            }));
        }
    }
}
