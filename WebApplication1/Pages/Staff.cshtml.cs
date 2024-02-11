using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class StaffModel : PageModel
    {
        private readonly DemoContext? demoContext;
        public StaffModel(DemoContext? demoContext)
        {
            this.demoContext = demoContext;
        }
        public List<Staff> Staffs { get; set; }
        public void OnGet()
        {
            Staffs = demoContext.Staffs
                .Include(s=>s.Orders)
                .Include(s=>s.InverseManager)
                .Include(s=>s.Manager)
                .ToList();
            Console.WriteLine(Staffs);
        }

    }
}
