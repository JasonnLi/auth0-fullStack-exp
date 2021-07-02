using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Core.Exp.Entities
{
    public partial class Customer
    {
        // One customer is one tenant as the customer/org is using your app that is using the same DB as other apps
        // There are mutiple applications using one DB, so application id should also be pointed out
        [Key]
        public int Id { get; set; }

        // tenantId
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid PublicId { get; set; }

        [Required]
        public string ApplicationId { get; set; }

        [Required]
        public string EnvironmentType { get; set; }

        [Required]
        public string OrgId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        //

        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<AuthConnection> AuthConnections { get; set; }
    }
}