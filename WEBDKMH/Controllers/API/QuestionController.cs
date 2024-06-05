using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEBDKMH.Models;
using System.Web.Http;


namespace WEBDKMH.Controllers
{
    public class QuestionController : ApiController
    {
        // GET: Question
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/Que/ds")]
        public List<Question> ds()
        {
            return db.Questions.ToList();
        }

        [HttpGet]
        [Route("api/Que/dstheoten")]
        public List<Question> dstheoten(string ten)
        {
            return db.Questions.Where(us => us.Question1.Contains(ten)).ToList();
        }

        [HttpDelete]
        [Route("api/Que/xoa")]
        public bool Xoa(int idus)
        {
            Question us = db.Questions.FirstOrDefault(x => x.ID == idus);
            if (us != null)
            {
                db.Questions.DeleteOnSubmit(us);
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        [HttpGet]
        [Route("api/Que/sua/{Id}")]
        public IHttpActionResult GetById(int Id)
        {
            var user = db.Questions.Where(u => u.IDsub == Id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
       
    }
}