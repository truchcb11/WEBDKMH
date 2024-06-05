using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEBDKMH.Models;


namespace WEBDKMH.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProfileController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44388/");
        }
        // GET: Account/EditProfile
        [HttpGet]
        public async Task<ActionResult> EditProfile()
        {
            int userId = (int)Session["UserID"];

            HttpResponseMessage response = await _httpClient.GetAsync($"api/EditProfile/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsAsync<Users>();

                if (user != null)
                {
                    var edit = new EditProfile
                    {
                        Fullname = user.Fullname,
                        Email = user.Email,
                        Address = user.Address,
                        Phone = user.Phone.HasValue ? user.Phone.ToString() : null
                    };
                    return View(edit);
                }
            }
            ModelState.AddModelError(string.Empty, "Không thể lấy thông tin người dùng. Vui lòng thử lại sau.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfile updatedUser)
        {
            int userId = (int)Session["UserID"];
            HttpResponseMessage re = await _httpClient.GetAsync($"api/EditProfile/{userId}");
            var user = await re.Content.ReadAsAsync<Users>();
            updatedUser.Password = user.Password;
            try
            {
                if (!ModelState.IsValid)
                {                  
                    return View(updatedUser);
                }

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/EditProfile/{userId}", updatedUser);


                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thông tin người dùng thành công.";   
                    return View(updatedUser);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi cập nhật thông tin người dùng.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
            }

            return View(updatedUser);
        }

        public async Task<ActionResult> History()
        {
            int userId = (int)Session["UserID"];
            HttpResponseMessage response = await _httpClient.GetAsync($"api/His/sua/{userId}");
            var h = await response.Content.ReadAsAsync<List<History>>();
            var subjectIds = h.Select(history => history.IDSub);
            List<Subjects> subjects = new List<Subjects>();
            foreach (var subjectId in subjectIds)
            {
                HttpResponseMessage subjectResponse = await _httpClient.GetAsync($"api/Sub/sua/{subjectId}");
                if (subjectResponse.IsSuccessStatusCode)
                {
                    var subject = await subjectResponse.Content.ReadAsAsync<Subjects>();
                    subjects.Add(subject);
                }
            }
            return View(subjects);
        }

        public async Task<ActionResult> Lessons()
        {
            int userId = (int)Session["UserID"];
            HttpResponseMessage response = await _httpClient.GetAsync($"api/His/sua/{userId}");
            var h = await response.Content.ReadAsAsync<List<History>>();
            var subjectIds = h.Select(history => history.IDSub);
            List<Subjects> subjects = new List<Subjects>();
            foreach (var subjectId in subjectIds)
            {
                HttpResponseMessage subjectResponse = await _httpClient.GetAsync($"api/Sub/sua/{subjectId}");
                if (subjectResponse.IsSuccessStatusCode)
                {
                    var subject = await subjectResponse.Content.ReadAsAsync<Subjects>();
                    subjects.Add(subject);
                }
            }
            return View(subjects);
        }

        public async Task<ActionResult> Lesson(int id)
        {
            try
            {
                HttpResponseMessage res = await _httpClient.GetAsync($"api/Les/sua/{id}");

                if (res.IsSuccessStatusCode)
                {

                    var lesson = await res.Content.ReadAsAsync<List<Models.Lesson>>();                   

                    return View(lesson);
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể tải danh sách khóa học.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> DetailLesson(int id)
        {
            try
            {
                

                HttpResponseMessage response = await _httpClient.GetAsync($"api/Less/sua/{id}");

                if (response.IsSuccessStatusCode)
                {
                    
                    var lesson = await response.Content.ReadAsAsync<Models.Lesson>();                   

                    return View(lesson);
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể tải danh sách khóa học.";
                    return View();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return View();
            }
        }
    }
}