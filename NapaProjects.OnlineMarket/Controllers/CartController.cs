using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Cryptography.X509Certificates;

namespace NapaProjects.OnlineMarket.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartRepository _cartRepository;
        private IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;

        public CartController(ICartRepository cartRepository, UserManager<AppUser> userManager, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> CartView(string  userName, string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(userName??User.Identity.Name);
            ViewBag.ReturnUrl = returnUrl;
            return View(new CartModel(_cartRepository.GetByUser(user.Id)
                , _cartRepository.Orders(_cartRepository.GetByUser(user.Id).Id)));
        }

        public IActionResult AddToCart(int productId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ProductId = productId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string returnUrl, int quantity)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _cartRepository.AddToCart(new Order { ProductId = productId, Quantity = quantity }, user.Id);
            return RedirectToAction(nameof(CartView), new { userName = User.Identity.Name, returnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult DeleteOrder(int orderId, string returnUrl)
        {
            _cartRepository.DeleteFromCart(orderId);
            return Redirect(returnUrl);
        }
    }
}
