using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using client.Models.ProductModels;

namespace client.Models.FridgeProductModels
{
    public class AddFridgeProduct
    {
        public List<Product> Products { get; set; }
        public int ProductId { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int? Quantity { get; set; }
    }
}