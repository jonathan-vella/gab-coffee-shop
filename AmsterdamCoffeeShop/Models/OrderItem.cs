using System.ComponentModel.DataAnnotations;

namespace AmsterdamCoffeeShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
