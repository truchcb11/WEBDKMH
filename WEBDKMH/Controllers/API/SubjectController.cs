using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WEBDKMH.Models.Payment;

namespace WEBDKMH.Controllers
{
    public class SubjectController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/Sub/ds")]
        public List<Subject> ds()
        {
            return db.Subjects.ToList();
        }

        [HttpGet]
        [Route("api/Sub/dstheoten")]
        public List<Subject> dstheoten(string ten)
        {
            return db.Subjects.Where(us => us.SubjectTitle.Contains(ten)).ToList();
        }

        [HttpDelete]
        [Route("api/Sub/xoa")]
        public bool Xoa(int idus)
        {
            Subject us = db.Subjects.FirstOrDefault(x => x.ID == idus);
            if (us != null)
            {
                db.Subjects.DeleteOnSubmit(us);
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("api/Sub/them")]
        public bool them([FromBody] Subject u)
        {
            try
            {
                db.Subjects.InsertOnSubmit(u);
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
        [Route("api/Sub/sua/{Id}")]
        public IHttpActionResult GetById(int Id)
        {
            var user = db.Subjects.FirstOrDefault(u => u.ID == Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("api/Sub/sua/{Id}")]
        public IHttpActionResult Update(int Id, [FromBody] Subject updated)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Subjects.FirstOrDefault(u => u.ID == Id);
            if (user == null)
            {
                return NotFound();
            }
            user.MaSubhect = updated.MaSubhect;
            user.SubjectTitle = updated.SubjectTitle;
            user.Describe = updated.Describe;
            user.DKtienquyet = updated.DKtienquyet;
            user.IDcate = updated.IDcate;
            user.Images = updated.Images;
            user.Price = updated.Price;
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


        [HttpGet]
        [Route("api/Sub/cate/{Id}")]
        public IHttpActionResult GetByIdCate(int Id)
        {
            var users = db.Subjects.Where(u => u.IDcate == Id).ToList();
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }

        
    }
}
