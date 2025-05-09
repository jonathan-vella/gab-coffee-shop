using System.ComponentModel.DataAnnotations;

namespace AmsterdamCoffeeShop.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Delivery address is required")]
        public string PreferredDeliveryAddress { get; set; }
        
        public bool MarketingPreference { get; set; }
    }
}
