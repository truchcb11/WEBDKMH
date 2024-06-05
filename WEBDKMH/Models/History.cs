using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class History
    {
        [Key]
        [Column("IDUser")]
        public int Iduser { get; set; }
        [Key]
        [Column("IDSub")]
        public int Idsub { get; set; }
        [Column("IDCou")]
        public int? Idcou { get; set; }

        [ForeignKey(nameof(Idsub))]
        [InverseProperty(nameof(Subjects.History))]
        public virtual Subjects IdsubNavigation { get; set; }
        [ForeignKey(nameof(Iduser))]
        [InverseProperty(nameof(Users.History))]
        public virtual Users IduserNavigation { get; set; }
    }
}
