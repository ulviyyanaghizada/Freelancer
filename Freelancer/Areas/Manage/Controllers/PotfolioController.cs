using Freelancer.DAL;
using Freelancer.Models;
using Freelancer.Utilities.Extension;
using Freelancer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freelancer.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles="Admin")]
    public class PotfolioController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public PotfolioController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page=1)
        {
            ViewBag.MaxPageCount = Math.Ceiling((decimal)_context.Portfolios.Count() / 5);
            ViewBag.CurrentPage = page;
            return View(_context.Portfolios.Skip((page-1)*5).Take(5).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreatePortfolioVM portfolioVM)
        {
            var coverImage = portfolioVM.CoverImage;
            string result = coverImage.CheckValidate("image/", 800);
            if (result.Length>0)
            {
                ModelState.AddModelError("CoverImage", result);
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Portfolio portfolio = new Portfolio
            {
                ImageUrl=coverImage.SaveFile(Path.Combine(_env.WebRootPath,"assets","img"))
            };
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0) return BadRequest();
            Portfolio exist = _context.Portfolios.FirstOrDefault(p=>p.Id== id);
            if (exist is null)
            {
                return NotFound();
            }
            exist.ImageUrl.DeleteFile(_env.WebRootPath, "assets/img");
            _context.Portfolios.Remove(exist);
            _context.SaveChanges();
            return RedirectToAction("Index");
           
        }
        public IActionResult Update(int? id)
        {
            if (id is null || id==0)
            {
                return BadRequest();
            }
            Portfolio portfolio = _context.Portfolios.FirstOrDefault(p => p.Id == id);
            if (portfolio is null) return NotFound();
            UpdatePortfolioVM updatePortfolio = new UpdatePortfolioVM
            {
                ImageUrl=portfolio.ImageUrl
            };
            ViewBag.Image = portfolio.ImageUrl;
            return View(updatePortfolio);

        }
        [HttpPost]
        public IActionResult Update(int? id, UpdatePortfolioVM updatePortfolio)
        {

            if (id is null || id == 0)
            {
                return BadRequest();
            }
            var coverImage = updatePortfolio.CoverImage;
            string result = coverImage.CheckValidate("image/", 800);
            if (result.Length>0)
            {
                ModelState.AddModelError("CoverImage", result);

            }
            if (!ModelState.IsValid)
            {
                ViewBag.Image=_context.Portfolios.FirstOrDefault(p=>p.Id == id);
                return View();
            }
            Portfolio portfolio = _context.Portfolios.FirstOrDefault(p => p.Id == id);
            if (portfolio is null) return NotFound();
            if (coverImage !=null)
            {
                portfolio.ImageUrl.DeleteFile(_env.WebRootPath, "assets/img");
                updatePortfolio.ImageUrl = coverImage.SaveFile(Path.Combine(_env.WebRootPath, "assets", "img"));

            }
            else {
                updatePortfolio.ImageUrl = portfolio.ImageUrl;            
            }
            portfolio.ImageUrl = updatePortfolio.ImageUrl;
            _context.SaveChanges();
            return RedirectToAction("Index");
            
        }
    }
}
