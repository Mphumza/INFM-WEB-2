using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INFM_WEB_2.Controllers
{
    public class DashController : Controller
    {
        private readonly ApplicationDbContext _context; // Replace YourDbContext with your actual DbContext

        public DashController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Dash()
        {
            // Retrieve products from the database or any data source
            List<Product> products = _context.Products.ToList(); // Fetch all products from the database

            int numberOfProducts = products.Count; // Get the number of products in the list

            // Pass the count of products to the view
            ViewBag.NumberOfProducts = numberOfProducts; // Using ViewBag, an easy way to pass data to the view

            return View();
        }
    }
}
