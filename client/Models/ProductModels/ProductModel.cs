using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace client.Models.ProductModels
{
    public class ProductModel
    {
        public Product Product { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int? DefaultQuantity { get; set; }
        [Required]
        public IFormFile Picture { get; set; }
    }
}