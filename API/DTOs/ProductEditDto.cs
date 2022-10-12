using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProductEditDto
    {
        [Required]
        public string Name { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int? DefaultQuantity { get; set; }
        public byte[] Picture { get; set; }
        public string PictureExntension { get; set; }
    }
}