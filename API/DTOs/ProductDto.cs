using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.DTOs
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int? DefaultQuantity { get; set; }
        [Required]
        public byte[] Picture { get; set; }
        [Required]
        public string PictureExntension { get; set; }
    }
}