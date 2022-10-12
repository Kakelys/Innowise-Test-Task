using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace client.Models.FridgeModels
{
    public class EditFridge
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public int? Model { get; set; }
        public Fridge Fridge { get; set; }
        public List<FridgeModel> Models { get; set; }
    }
}