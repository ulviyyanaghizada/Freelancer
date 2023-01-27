using Freelancer.DAL;
using Freelancer.Models;
using Freelancer.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace Freelancer.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM home = new HomeVM { Portfolios = _context.Portfolios };
            return View(home);
        }
    }
}
