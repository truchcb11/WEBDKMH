using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBDKMH.Areas.Admin.Model
{
    public class LesViewModel
    {
        public int Id { get; set; }
        public string Tieude { get; set; }
        public string Video { get; set; }
        public string Describe { get; set; }
        public int? Idsub { get; set; }
    }
}