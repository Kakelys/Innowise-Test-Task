using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class FridgeProductDto
    {
        [Required]
        public int? ProductId { get; set; }

        [Required]
        [Range(0,int.MaxValue)]
        public int? Quantity { get; set; }
    }   
}