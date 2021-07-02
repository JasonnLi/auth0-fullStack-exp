using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Core.Exp.Entities
{
    public partial class AuthConnection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicationId { get; set; }

        [Required]
        public string EnvironmentType { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public string ConnectionName { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public Customer Customer { get; set; }
    }
}