using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using client.Models.FridgeProductModels;

namespace client.Models.FridgeModels
{
    public class Fridge
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public int? Model { get; set; }

        public virtual FridgeModel ModelNavigation { get; set; }
        public virtual ICollection<FridgeProduct> FridgeProducts { get; set; }
    }
}