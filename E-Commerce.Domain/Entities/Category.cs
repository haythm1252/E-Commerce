using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public bool? IsTopCategory { get; set; } 
        public bool? IsSection { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        // relationships
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
