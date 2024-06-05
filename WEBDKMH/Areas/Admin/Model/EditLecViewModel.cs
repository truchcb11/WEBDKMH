using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBDKMH.Areas.Admin.Model
{
    public class EditLecViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}