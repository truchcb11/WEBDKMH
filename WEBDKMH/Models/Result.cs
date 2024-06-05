using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Result
    {
        [Key]
        [Column("IDuser")]
        public int Iduser { get; set; }
        [Key]
        [Column("IDexam")]
        public int Idexam { get; set; }
        [Key]
        [Column("IDques")]
        public int Idques { get; set; }
        [Column("answer")]
        [StringLength(250)]
        public string Answer { get; set; }

        [ForeignKey(nameof(Idexam))]
        [InverseProperty(nameof(Exam.Result))]
        public virtual Exam IdexamNavigation { get; set; }
        [ForeignKey(nameof(Idques))]
        [InverseProperty(nameof(Question.Result))]
        public virtual Question IdquesNavigation { get; set; }
        [ForeignKey(nameof(Iduser))]
        [InverseProperty(nameof(Users.Result))]
        public virtual Users IduserNavigation { get; set; }
    }
}
