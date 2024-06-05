using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBDKMH.Models
{
    public class LoginViewModel
    {
        [Required]
        public string email { get; set; }
        public string password { get; set; }
    }
}