using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEBDKMH.Controllers
{
    public class CourseController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/Cou/sua/{Id}")]
        public IHttpActionResult GetById(int Id)
        {
            var users = db.Courses.Where(u => u.IDSubjects == Id).ToList();
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }
    }
}
