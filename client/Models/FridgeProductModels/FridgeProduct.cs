using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using client.Models.FridgeModels;
using client.Models.ProductModels;

namespace client.Models.FridgeProductModels
{
    public class FridgeProduct
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public int? Fridge { get; set; }
        public int Quantity { get; set; }

        public virtual Fridge FridgeNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}