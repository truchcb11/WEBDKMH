using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEBDKMH.Controllers
{
    public class CategoryController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/Cate/ds")]
        public List<Category> ds()
        {
            return db.Categories.ToList();
        }

        [HttpGet]
        [Route("api/Cate/sua/{id}")]
        public IHttpActionResult GetById(int id)
        {
            var user = db.Categories.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
