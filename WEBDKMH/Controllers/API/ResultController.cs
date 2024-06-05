using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WEBDKMH.Controllers.API
{
    public class ResultController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        // GET: Result

        [HttpPost]
        [Route("api/Result/them")]
        public bool themuser([FromBody] Result u)
        {
            try
            {
                db.Results.InsertOnSubmit(u);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi: " + ex.Message);
                return false;
            }
        }


        [Route("api/Result/getResultsByExamId/{Id}")]
        [HttpGet]
        public IHttpActionResult GetById(int Id)
        {
            var users = db.Results.Where(u => u.IDuser == Id).ToList();
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }

    }
}