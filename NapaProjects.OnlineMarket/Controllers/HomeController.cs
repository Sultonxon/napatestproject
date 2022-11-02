using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NapaProjects.OnlineMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider serviceProvider;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            if (AppRoles.IsFirstRunning)
            {
                await AppDbContext.InitializeOnMigration(serviceProvider);
                AppRoles.IsFirstRunning = false;
            }
            
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Migration()
        {
            if (AppRoles.IsFirstRunning)
            {
                await AppDbContext.InitializeOnMigration(serviceProvider);
                AppRoles.IsFirstRunning = false;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SecondPage() => View();
    }
}