using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEBDKMH.Controllers
{
    public class LecturersController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/Lec/ds")]
        public List<Lecturer> dsLecturer()
        {
            return db.Lecturers.ToList();
        }

        [HttpGet]
        [Route("api/Lec/dstheoten")]
        public List<Lecturer> dsLectheoten(string ten)
        {
            return db.Lecturers.Where(us => us.Fullname.Contains(ten)).ToList();
        }

        [HttpDelete]
        [Route("api/Lec/xoa")]
        public bool Xoa(int idus)
        {
            Lecturer us = db.Lecturers.FirstOrDefault(x => x.ID == idus);
            if (us != null)
            {
                db.Lecturers.DeleteOnSubmit(us);
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("api/Lec/them")]
        public bool them([FromBody] Lecturer u)
        {
            try
            {
                db.Lecturers.InsertOnSubmit(u);
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
        [Route("api/Lec/sua/{Id}")]
        public IHttpActionResult GetById(int Id)
        {
            var user = db.Lecturers.FirstOrDefault(u => u.ID == Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("api/Lec/sua/{Id}")]
        public IHttpActionResult Update(int Id, [FromBody] Lecturer updated)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Lecturers.FirstOrDefault(u => u.ID == Id);
            if (user == null)
            {
                return NotFound();
            }
            // Cập nhật thông tin người dùng
            user.email = updated.email;
            user.Fullname = updated.Fullname;
            user.Address = updated.Address;
            user.phone = updated.phone;
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
