using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using client.Models.ProductModels;

namespace client.Models.FridgeModels
{
    public class CreateFridge
    {
        public List<FridgeModel> Models { get; set; }
        public List<List<Product>> ProductGroups { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string OwnerName {get;set;}
        [Required]
        public int Model {get;set;}
        public List<Product> Products { get; set; }
    }
}