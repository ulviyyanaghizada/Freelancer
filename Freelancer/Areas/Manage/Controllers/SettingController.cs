using Freelancer.DAL;
using Freelancer.Models;
using Freelancer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelancer.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        readonly AppDbContext _context;
        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Settings);
        }
        public IActionResult Update(int? id)
        {
            if (id == null || id==0) return BadRequest();
            Setting exist= _context.Settings.FirstOrDefault(p=>p.Id == id);
            if (exist is null) return NotFound();
            UpdateSettingVM updateSetting = new UpdateSettingVM
            {
                Value = exist.Value,
                Key=exist.Key
            };
            return View(updateSetting);
        }
        [HttpPost]
        public IActionResult Update(int? id,UpdateSettingVM updateSetting)
        {
            if (id is null || id == 0) return BadRequest();
            if(!ModelState.IsValid) return View();
            Setting exist= _context.Settings.FirstOrDefault(p=>p.Id == id);
            if (exist is null) return NotFound();
            exist.Value=updateSetting.Value;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
