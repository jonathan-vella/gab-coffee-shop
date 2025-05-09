using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AmsterdamCoffeeShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsterdamCoffeeShop.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CheckoutModel> _logger;

        [BindProperty]
        public Customer Customer { get; set; } = new Customer();

        [BindProperty]
        public string DeliveryAddress { get; set; }

        [BindProperty]
        public bool SaveAddress { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal CartTotal => CartItems.Sum(item => item.Product.Price * item.Quantity);
        public decimal DeliveryFee => 2.50m;

        public CheckoutModel(ApplicationDbContext dbContext, ILogger<CheckoutModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            LoadCartFromSession();

            if (!CartItems.Any())
            {
                return RedirectToPage("/Order");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadCartFromSession();
                return Page();
            }

            LoadCartFromSession();

            if (!CartItems.Any())
            {
                return RedirectToPage("/Order");
            }

            // Save customer to database
            var existingCustomer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Email == Customer.Email);

            if (existingCustomer == null)
            {
                // New customer
                if (SaveAddress)
                {
                    Customer.PreferredDeliveryAddress = DeliveryAddress;
                }

                _dbContext.Customers.Add(Customer);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Existing customer
                Customer = existingCustomer;

                if (SaveAddress)
                {
                    existingCustomer.PreferredDeliveryAddress = DeliveryAddress;
                    await _dbContext.SaveChangesAsync();
                }
            }

            // Create order
            var order = new Order
            {
                CustomerId = Customer.Id,
                DeliveryAddress = DeliveryAddress,
                OrderDate = DateTime.Now,
                TotalAmount = CartTotal + DeliveryFee,
                Status = "Pending"
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            // Create order items
            foreach (var cartItem in CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.Product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Product.Price
                };

                _dbContext.OrderItems.Add(orderItem);
            }

            await _dbContext.SaveChangesAsync();

            // Clear cart
            HttpContext.Session.Remove("Cart");

            return RedirectToPage("/OrderConfirmation", new { id = order.Id });
        }

        private void LoadCartFromSession()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                CartItems = cart;
            }
        }
    }
}
