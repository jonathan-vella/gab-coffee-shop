using System.ComponentModel.DataAnnotations;

namespace AmsterdamCoffeeShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        public string ImageUrl { get; set; }
        
        public bool IsAvailable { get; set; }
        
        public string Category { get; set; }
    }
}
