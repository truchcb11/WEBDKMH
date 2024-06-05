using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WEBDKMH.Models;

namespace WEBDKMH.Models
{
    public class EditProfile
    {
        public EditProfile()
        {
            IDRoll = 1;
            Verify = 1;
        }

        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public int IDRoll { get; set; }

        public int Verify { get; set; }
    }
}