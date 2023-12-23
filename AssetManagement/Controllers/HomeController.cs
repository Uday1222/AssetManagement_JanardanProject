using AssetManagement.Models;
using AssetManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AssetManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.Get("User");
            if(user != null)
            {
                if(user.ToString() == "")
                {
                    return RedirectToAction("Login", "Auth");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
            //var result = await _employeeRepo.GetAll();
            

        }

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