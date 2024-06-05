using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Exam
    {
        public Exam()
        {
            Result = new HashSet<Result>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("number of questions")]
        public int? NumberOfQuestions { get; set; }

        [InverseProperty("IdexamNavigation")]
        public virtual ICollection<Result> Result { get; set; }
    }
}
