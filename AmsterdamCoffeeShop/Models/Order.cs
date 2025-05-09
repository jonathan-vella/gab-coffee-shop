using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmsterdamCoffeeShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public string DeliveryAddress { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
