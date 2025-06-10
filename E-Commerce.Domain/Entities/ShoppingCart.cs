using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string? CartId { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>(); 
        
        [NotMapped]
        public decimal TotalPrice => ShoppingCartItems.Sum(item => item.TotalPrice);
    }
}
