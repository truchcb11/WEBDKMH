using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEBDKMH.Areas.Admin.Model
{
    public class SubViewModel
    { 
        public int Id { get; set; }
        public string Masubhect { get; set; }
        public string SubjectTitle { get; set; }
        public string Describe { get; set; }
        public string DKtienquyet { get; set; }
        public int? IDcate { get; set; }
        public string Images { get; set; }
        public double? Price { get; set; }
    }
}