using System;
using System.Collections.Generic;

#nullable disable

namespace API.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Role { get; set; } = 10;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual Role RoleNavigation { get; set; }
    }
}
