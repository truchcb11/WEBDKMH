using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBDKMH.Models
{
    public class ExamResultViewModel
    {
        public List<Question> Questions { get; set; }
        public List<Result> Results { get; set; }
    }
}
