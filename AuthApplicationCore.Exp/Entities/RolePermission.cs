using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Core.Exp.Entities
{
    public partial class RolePermission
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}