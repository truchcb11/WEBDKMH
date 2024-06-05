using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEBDKMH.Controllers
{
    public class LessonController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/Les/ds")]
        public List<Lesson> ds()
        {
            return db.Lessons.ToList();
        }

        [HttpGet]
        [Route("api/Les/dstheoten")]
        public List<Lesson> dstheoten(string ten)
        {
            return db.Lessons.Where(us => us.TieuDe.Contains(ten)).ToList();
        }

        [HttpDelete]
        [Route("api/Les/xoa")]
        public bool Xoa(int id)
        {
            Lesson us = db.Lessons.FirstOrDefault(x => x.ID == id);
            if (us != null)
            {
                db.Lessons.DeleteOnSubmit(us);
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        [HttpGet]
        [Route("api/Les/sua/{Id}")]
        public IHttpActionResult GetById(int Id)
        {
            var user = db.Lessons.Where(u => u.IDSub == Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("api/Less/sua/{Id}")]
        public IHttpActionResult GetId(int Id)
        {
            var user = db.Lessons.FirstOrDefault(u => u.ID == Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("api/Les/them")]
        public bool them([FromBody] Lesson u)
        {
            try
            {
                db.Lessons.InsertOnSubmit(u);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi: " + ex.Message);
                return false;
            }
        }


        [HttpPut]
        [Route("api/Less/sua/{Id}")]
        public IHttpActionResult Update(int Id, [FromBody] Lesson updated)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Lessons.FirstOrDefault(u => u.ID == Id);
            if (user == null)
            {
                return NotFound();
            }
            user.TieuDe = updated.TieuDe;
            user.Video = updated.Video;
            user.Describe = updated.Describe;
            user.IDSub = updated.IDSub;
            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }
    }
}
