using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class FridgeProduct
    {
        public int Id { get; set; }
        public int? Product { get; set; }
        public int? Fridge { get; set; }
        public int? Quantity { get; set; }

        public virtual Fridge FridgeNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
