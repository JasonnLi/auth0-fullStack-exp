using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Core.Exp.Entities
{
    public partial class Role
    {
        /*public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
            UserRoles = new HashSet<UserRole>();
        }*/

        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public Customer Customer { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}