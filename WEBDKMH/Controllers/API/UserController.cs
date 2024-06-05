using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WEBDKMH.Controllers
{
    public class UserController : ApiController
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        [Route("api/EL/ds")]
        public List<User> dsUser()
        {
            return db.Users.ToList();
        } 

        [HttpGet]
        [Route("api/EL/dstheoten")]
        public List<User> dsUsertheoten(string ten)
        {
            return db.Users.Where(us => us.Fullname.Contains(ten)).ToList();
        }

        [HttpGet]
        [Route("api/EL/Singerds")]
        public User Timuser(string ten)
            {
                if (ten == null)
                {
                    return null;
                }
                else
                {
                    return db.Users.FirstOrDefault(u => u.Email == ten);
                }
            }

        [HttpPost]
        [Route("api/EL/themuser")]
        public bool themuser([FromBody] User u)
        {
            try
            {
                db.Users.InsertOnSubmit(u);
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
        [Route("api/EL/suauser")]
        public bool suauser([FromBody] User u)
        {
            try
            {
                User us = db.Users.SingleOrDefault(s => s.ID == u.ID);
                us.Email = u.Email;
                us.password = u.password;
                us.Fullname = u.Fullname;
                us.Address = u.Address;
                us.Phone = u.Phone;
                us.IDRoll = u.IDRoll;
                us.Verify = u.Verify;
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi: " + ex.Message);
                return false;
            }
        }

        [HttpDelete]
        [Route("api/EL/xoauser")]
        public bool Xoauser(int idus)
        {
            User us = db.Users.FirstOrDefault(x => x.ID == idus);
            if (us != null)
            {
                db.Users.DeleteOnSubmit(us);
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("api/EL/verifyuser")]
        public bool VerifyUser(int userId)
        {
            try
            {
                User user = db.Users.FirstOrDefault(u => u.ID == userId);
                if (user != null)
                {
                    user.Verify = true; // Chuyển giá trị Verify thành true
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false; // Không tìm thấy người dùng với userId tương ứng
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Có lỗi: " + ex.Message);
                return false;
            }
        }

        [HttpGet]
        [Route("api/EditProfile/{userId}")]
        public IHttpActionResult GetUserById(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.ID == userId);
            if (user == null)
            {
                return NotFound(); 
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("api/EditProfile/{userId}")]
        public IHttpActionResult UpdateUser(int userId, [FromBody] User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var user = db.Users.FirstOrDefault(u => u.ID == userId);
            if (user == null)
            {
                return NotFound(); 
            }
            user.Email = updatedUser.Email;
            user.password = updatedUser.password;
            user.Fullname = updatedUser.Fullname;
            user.Address = updatedUser.Address;
            user.Phone = updatedUser.Phone;
            user.IDRoll = updatedUser.IDRoll;
            user.Verify = updatedUser.Verify;
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

