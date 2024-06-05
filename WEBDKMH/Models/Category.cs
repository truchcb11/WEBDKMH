﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Category
    {
        public Category()
        {
            Subjects = new HashSet<Subjects>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Catename { get; set; }
        [StringLength(100)]
        public string Images { get; set; }

        [InverseProperty("IdcateNavigation")]
        public virtual ICollection<Subjects> Subjects { get; set; }
    }
}
