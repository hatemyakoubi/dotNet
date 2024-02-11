using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class ProduitModel : PageModel
    {
        private readonly DemoContext? demoContext;
        public ProduitModel(DemoContext? demoContext)
        {
            this.demoContext = demoContext;
        }
        public List<Product> Products { get; set; }
        public void OnGet(int? productId)
        {
            IQueryable<Product> Querry = demoContext.Products
                 .Include(p => p.Category)
                 .Include(p => p.Brand)
                 .Include(p => p.Stocks)
                 .Include(p => p.OrderItems)
                 .Select(p => new Product
                 {
                     ProductId = p.ProductId,
                     ProductName = p.ProductName,
                     Brand = p.Brand,
                     Category = p.Category,
                     Quantity = p.Stocks.Sum(s => s.Quantity)
                 });
            if (productId.HasValue)
            {
                Querry = Querry.Where(p => p.ProductId == productId);

            }
            Products = Querry.ToList();

        }
    }
}
