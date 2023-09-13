using InventarioApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventarioApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult InventoryCreate()
        {
            return View();
        }

        public IActionResult InventoryEdit()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            var inventoryList = new List<InventoryEntry>();

            
            var inventoryEntry = new InventoryEntry
            {
                Id = 1,
                Name = "Laptop",
                Type = "Hardware",
                Description = "Laptop HP 15",
                Notes = "Ninguna",
                Quantity = 2
            };
            inventoryList.Add(inventoryEntry);

            inventoryEntry = new InventoryEntry
            {
                Id = 2,
                Name = "Monitor",
                Type = "Hardware",
                Description = "Monitor HP 24",
                Notes = "Ninguna",
                Quantity = 1
            };
            inventoryList.Add(inventoryEntry);

            return View(inventoryList);
        }

        public IActionResult Login()
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