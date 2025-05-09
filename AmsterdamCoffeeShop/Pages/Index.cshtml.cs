using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AmsterdamCoffeeShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmsterdamCoffeeShop.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _dbContext;

    [BindProperty]
    public string Email { get; set; }
    
    [BindProperty]
    public bool MarketingConsent { get; set; }
    
    public List<Product> FeaturedProducts { get; set; } = new List<Product>();

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task OnGetAsync()
    {
        FeaturedProducts = await _dbContext.Products
            .Where(p => p.IsAvailable)
            .Take(3)
            .ToListAsync();
    }
    
    public IActionResult OnPostSubscribe()
    {
        if (string.IsNullOrEmpty(Email))
        {
            ModelState.AddModelError("Email", "Email is required");
            return Page();
        }
        
        // In a real app, you would save the email to the database
        // and handle marketing consent
        
        TempData["SuccessMessage"] = "Thank you for subscribing!";
        return RedirectToPage();
    }
}
