using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Question
    {
        public Question()
        {
            Result = new HashSet<Result>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("Question")]
        [StringLength(250)]
        public string Question1 { get; set; }
        [Column("answer_a")]
        [StringLength(250)]
        public string AnswerA { get; set; }
        [Column("answer_b")]
        [StringLength(250)]
        public string AnswerB { get; set; }
        [Column("answer_c")]
        [StringLength(250)]
        public string AnswerC { get; set; }
        [Column("answer_d")]
        [StringLength(250)]
        public string AnswerD { get; set; }
        [Column("answer")]
        [StringLength(250)]
        public string Answer { get; set; }
        [Column("IDsub")]
        public int? Idsub { get; set; }

        [ForeignKey(nameof(Idsub))]
        [InverseProperty(nameof(Subjects.Question))]
        public virtual Subjects IdsubNavigation { get; set; }
        [InverseProperty("IdquesNavigation")]
        public virtual ICollection<Result> Result { get; set; }
    }
}
