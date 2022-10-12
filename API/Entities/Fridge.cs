using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class Fridge
    {
        public Fridge()
        {
            FridgeProducts = new HashSet<FridgeProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public int? Model { get; set; }

        public virtual FridgeModel ModelNavigation { get; set; }
        public virtual ICollection<FridgeProduct> FridgeProducts { get; set; }
    }
}
