using AssetManagement.Models;
using AssetManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AssetManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Employee> _employeeRepo;

        public HomeController(ILogger<HomeController> logger, IRepository<Employee> employeeRepo)
        {
            _logger = logger;
            _employeeRepo = employeeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _employeeRepo.GetAll();
            return View();

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