using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Core.Exp.Entities
{
    public partial class Permission
    {
        /*public Permission()
        {
            RolePermission = new HashSet<RolePermission>();
        }*/

        [Key]
        public int Id { get; set; }

        [Required]
        public string Action { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}