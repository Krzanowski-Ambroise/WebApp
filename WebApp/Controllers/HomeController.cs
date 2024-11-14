using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Areas.Identity.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;  // Injection de UserManager

        // Constructeur avec injection de UserManager
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;  // Assigner UserManager
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewData["UserId"] = user.Id;

                // R�cup�rer les r�les de l'utilisateur connect�
                var roles = await _userManager.GetRolesAsync(user);
                ViewData["Roles"] = string.Join(", ", roles); // Convertir en cha�ne si plusieurs r�les
            }
            else
            {
                ViewData["UserId"] = "User not found";
                ViewData["Roles"] = "No roles";
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
