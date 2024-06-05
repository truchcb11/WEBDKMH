using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Course
    {
        [Key]
        [Column("IDlecturers")]
        public int Idlecturers { get; set; }
        [Key]
        [Column("IDSubjects")]
        public int Idsubjects { get; set; }
        [StringLength(50)]
        public string Coursename { get; set; }
        public TimeSpan? Starttime { get; set; }
        public TimeSpan? Endtime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Startdate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Enddate { get; set; }
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey(nameof(Idlecturers))]
        [InverseProperty(nameof(Lecturers.Course))]
        public virtual Lecturers IdlecturersNavigation { get; set; }
        [ForeignKey(nameof(Idsubjects))]
        [InverseProperty(nameof(Subjects.Course))]
        public virtual Subjects IdsubjectsNavigation { get; set; }
    }
}
