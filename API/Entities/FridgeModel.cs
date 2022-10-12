using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class FridgeModel
    {
        public FridgeModel()
        {
            Fridges = new HashSet<Fridge>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Fridge> Fridges { get; set; }
    }
}
