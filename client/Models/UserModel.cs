using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace client.Models
{
    public class UserModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}