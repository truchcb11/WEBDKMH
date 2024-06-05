using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEBDKMH.Controllers
{
    public class HistoryController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();


        [HttpGet]
        [Route("api/His/ds")]
        public List<History> ds()
        {
            return db.Histories.ToList();
        }

        [HttpPost]
        [Route("api/His/them")]
        public bool them([FromBody] History u)
        {
            try
            {               
                db.Histories.InsertOnSubmit(u);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi: " + ex.Message);
                return false;
            }
        }

        [HttpGet]
        [Route("api/His/sua/{Id}")]
        public IHttpActionResult GetById(int Id)
        {
            var users = db.Histories.Where(u => u.IDUser == Id).ToList();
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }
    }
}
