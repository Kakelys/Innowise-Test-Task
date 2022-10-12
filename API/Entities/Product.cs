using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class Product
    {
        public Product()
        {
            FridgeProducts = new HashSet<FridgeProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }

        public virtual Image Image { get; set; }
        public virtual ICollection<FridgeProduct> FridgeProducts { get; set; }
    }
}
