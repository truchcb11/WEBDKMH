using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Lesson
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string TieuDe { get; set; }
        [StringLength(100)]
        public string Video { get; set; }
        [StringLength(1000)]
        public string Describe { get; set; }
        [Column("IDSub")]
        public int? Idsub { get; set; }

        [ForeignKey(nameof(Idsub))]
        [InverseProperty(nameof(Subjects.Lesson))]
        public virtual Subjects IdsubNavigation { get; set; }
    }
}
