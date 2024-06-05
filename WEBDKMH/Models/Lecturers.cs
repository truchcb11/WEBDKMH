using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Lecturers
    {
        public Lecturers()
        {
            Course = new HashSet<Course>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string Fullname { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }
        [Column("phone")]
        public int? Phone { get; set; }

        [InverseProperty("IdlecturersNavigation")]
        public virtual ICollection<Course> Course { get; set; }
    }
}
