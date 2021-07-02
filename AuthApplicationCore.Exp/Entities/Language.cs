using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthApp.Core.Exp.Entities
{
    public class Language
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // for one to many, one language can be used by many books. Thus using collection here to represent language PK is in the Books table as FK
        public ICollection<Books> Books { get; set; }

    }
}
