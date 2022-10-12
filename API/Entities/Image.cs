using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public virtual Product IdNavigation { get; set; }
    }
}