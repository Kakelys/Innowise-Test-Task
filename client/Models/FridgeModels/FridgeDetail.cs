using System;
using System.ComponentModel.DataAnnotations;

namespace client.Models.FridgeModels
{
    public class FridgeDetail
    {
        public Fridge FridgeInDetail { get; set; }
        public int ProductId {get;set;}
        [Required]
        [Range(0, int.MaxValue)]
        public int? Quantity {get;set;}
    }
}