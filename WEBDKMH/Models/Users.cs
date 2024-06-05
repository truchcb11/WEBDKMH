using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEBDKMH.Models
{
    public partial class Users
    {
        public Users()
        {
            History = new HashSet<History>();
            Result = new HashSet<Result>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Fullname { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        public int? Phone { get; set; }
        [Column("IDRoll")]
        public int? Idroll { get; set; }
        public bool? Verify { get; set; }

        [ForeignKey(nameof(Idroll))]
        [InverseProperty(nameof(Roles.Users))]
        public virtual Roles IdrollNavigation { get; set; }
        [InverseProperty("IduserNavigation")]
        public virtual ICollection<History> History { get; set; }
        [InverseProperty("IduserNavigation")]
        public virtual ICollection<Result> Result { get; set; }
    }
}
