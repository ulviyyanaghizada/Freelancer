using Freelancer.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Freelancer.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        readonly AppDbContext _context;
        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_context.Settings.ToDictionary(s => s.Key, s => s.Value));
        }
    }
}
