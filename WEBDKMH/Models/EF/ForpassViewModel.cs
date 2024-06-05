using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBDKMH.Models
{
    public class ForpassViewModel
    {
        [Required]
        public string email { get; set; }
    }
}