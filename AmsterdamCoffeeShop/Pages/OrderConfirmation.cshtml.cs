using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AmsterdamCoffeeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsterdamCoffeeShop.Pages
{
    public class OrderConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderConfirmationModel> _logger;

        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Customer Customer { get; set; }
        public decimal SubTotal => OrderItems.Sum(i => i.TotalPrice);
        public decimal DeliveryFee => 2.50m;

        public OrderConfirmationModel(ApplicationDbContext dbContext, ILogger<OrderConfirmationModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            OrderItems = await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == id)
                .ToListAsync();

            Customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == Order.CustomerId);

            return Page();
        }
    }
}
