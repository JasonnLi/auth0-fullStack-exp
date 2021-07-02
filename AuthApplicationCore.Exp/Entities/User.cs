using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Core.Exp.Entities
{
    public enum UserType
    {
        Standard = 1,
        ALM = 2
    }

    public enum UserSource
    {
        // User was created via the dashboard
        Internal = 1,

        // User record was created via Identity Provider
        IdentityProvider = 2
    }

    public partial class User
    {
        [Key]
        public int Id { get; set; }

        // CustomerId represent tenantId
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        // each user in Auth0 user store has a auth0 ID
        [Required]
        public string Auth0Id { get; set; }

        public UserType Type { get; set; }

        public UserSource Source { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public Customer Customer { get; set; }

        // ICollection implements IEnumerable interface, providing ability to edit data
        // One user can upload many books or zero books
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Books> Books { get; set; }

    }
}