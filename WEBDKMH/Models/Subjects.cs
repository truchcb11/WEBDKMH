using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Subjects
    {
        public Subjects()
        {
            Course = new HashSet<Course>();
            History = new HashSet<History>();
            Lesson = new HashSet<Lesson>();
            Question = new HashSet<Question>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string MaSubhect { get; set; }
        [StringLength(50)]
        public string SubjectTitle { get; set; }
        [StringLength(1000)]
        public string Describe { get; set; }
        [Column("DKtienquyet")]
        [StringLength(50)]
        public string Dktienquyet { get; set; }
        [Column("IDcate")]
        public int? Idcate { get; set; }
        [StringLength(100)]
        public string Images { get; set; }
        public double? Price { get; set; }

        [ForeignKey(nameof(Idcate))]
        [InverseProperty(nameof(Category.Subjects))]
        public virtual Category IdcateNavigation { get; set; }
        [InverseProperty("IdsubjectsNavigation")]
        public virtual ICollection<Course> Course { get; set; }
        [InverseProperty("IdsubNavigation")]
        public virtual ICollection<History> History { get; set; }
        [InverseProperty("IdsubNavigation")]
        public virtual ICollection<Lesson> Lesson { get; set; }
        [InverseProperty("IdsubNavigation")]
        public virtual ICollection<Question> Question { get; set; }
    }
}
