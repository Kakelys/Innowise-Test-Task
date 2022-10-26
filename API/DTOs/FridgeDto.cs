using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class FridgeDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string OwnerName {get;set;}

        [Required]
        public int? Model {get;set;}

        public List<Product> Products { get; set; }
    }
}