using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        // Product
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        // ShoppingCart
        [ForeignKey(nameof(ShoppingCart))]
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = default!;
    }
}
