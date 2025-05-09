using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AmsterdamCoffeeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsterdamCoffeeShop.Pages
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderModel> _logger;
        
        public List<Product> Products { get; set; } = new List<Product>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal CartTotal => CartItems.Sum(item => item.Product.Price * item.Quantity);

        public OrderModel(ApplicationDbContext dbContext, ILogger<OrderModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task OnGetAsync(int? productId = null)
        {
            Products = await _dbContext.Products
                .Where(p => p.IsAvailable)
                .OrderBy(p => p.Category)
                .ToListAsync();
                
            LoadCartFromSession();
            
            // Handle direct product addition if productId is provided
            if (productId.HasValue)
            {
                await AddProductToCart(productId.Value, 1);
            }
        }
        
        public async Task<IActionResult> OnPostAddToCartAsync(int productId, int quantity)
        {
            if (quantity <= 0) quantity = 1;
            
            await AddProductToCart(productId, quantity);
            
            return RedirectToPage();
        }
        
        public IActionResult OnPostRemoveFromCart(int productId)
        {
            LoadCartFromSession();
            
            var item = CartItems.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                CartItems.Remove(item);
                SaveCartToSession();
            }
            
            return RedirectToPage();
        }
        
        private async Task AddProductToCart(int productId, int quantity)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return;
            
            LoadCartFromSession();
            
            var existingItem = CartItems.FirstOrDefault(i => i.Product.Id == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            
            SaveCartToSession();
        }
        
        private void LoadCartFromSession()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                CartItems = cart;
            }
        }
        
        private void SaveCartToSession()
        {
            HttpContext.Session.Set("Cart", CartItems);
        }
    }
    
    // Session extension methods
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
