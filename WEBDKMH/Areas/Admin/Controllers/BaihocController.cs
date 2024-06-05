using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WEBDKMH.Areas.Admin.Model;
using WEBDKMH.Models;

namespace WEBDKMH.Areas.Admin.Controllers
{
    public class BaihocController : Controller
    {
        // GET: Admin/Baihoc
        public ActionResult Baihoc(int? page)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/Les/ds").Result;
            List<Models.Lesson> subjects = response.Content.ReadAsAsync<List<Models.Lesson>>().Result;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Search(string search, int? page)
        {
            string apiUrl = "https://localhost:44388/api/Les/ds";
            if (!string.IsNullOrEmpty(search))
            {
                apiUrl = "https://localhost:44388/api/Les/dstheoten?ten=" + search;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            List<Models.Lesson> users = response.Content.ReadAsAsync<List<Models.Lesson>>().Result;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            IPagedList<Models.Lesson> usersPagedList = users.ToPagedList(pageNumber, pageSize);

            ViewBag.IsSearchResult = !string.IsNullOrEmpty(search);
            ViewBag.SearchTerm = search;
            return View("Baihoc", usersPagedList);
        }

        public ActionResult Delete(int Id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync("https://localhost:44388/api/Les/xoa?id=" + Id).Result;

            bool result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Baihoc");
        }

        [HttpGet]
        public ActionResult AddBaihoc()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage categoryResponse = client.GetAsync("https://localhost:44388/api/Sub/ds").Result;
            List<Subjects> categories = categoryResponse.Content.ReadAsAsync<List<Subjects>>().Result;
            SelectList danhMucList = new SelectList(categories, "ID", "SubjectTitle");
            ViewBag.DanhMucList = danhMucList;

            return View();
        }

        [HttpPost]
        public string UploadVideo(HttpPostedFileBase video)
        {
            try
            {
                if (video != null && video.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(video.FileName);
                    string directoryPath = Server.MapPath("~/Videos");

                    // Đảm bảo thư mục Videos tồn tại
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, fileName);
                    video.SaveAs(filePath);

                    string videoUrl = Url.Content("~/Videos/" + fileName);
                    return videoUrl;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi để dễ dàng gỡ lỗi
                // Bạn có thể sử dụng một logging framework như log4net, NLog, Serilog, vv.
                System.Diagnostics.Debug.WriteLine("Error uploading video: " + ex.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddMonHoc(string Tieude, HttpPostedFileBase video, string Describe, int Idsub)
        {
            try
            {
                string videoUrl = UploadVideo(video);

                Models.Lesson newless = new Models.Lesson
                {
                    TieuDe = Tieude,
                    Video = videoUrl,
                    Describe = Describe,
                    Idsub = Idsub,
                    
                };

                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.PostAsJsonAsync("https://localhost:44388/api/Les/them", newless).Result;

                if (response.IsSuccessStatusCode)
                {
                    Response.Write("<script>alert('successful');</script>");
                    return RedirectToAction("Baihoc");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi them mon hoc.");
                    return RedirectToAction("Baihoc");
                }
            }
            catch
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult SuaBaihoc(int id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/Less/sua/" + id).Result;
            var lesson = response.Content.ReadAsAsync<Models.Lesson>().Result;

            HttpResponseMessage categoryResponse = client.GetAsync("https://localhost:44388/api/Sub/ds").Result;
            List<Subjects> categories = categoryResponse.Content.ReadAsAsync<List<Subjects>>().Result;
            SelectList danhMucList = new SelectList(categories, "ID", "SubjectTitle", lesson.Idsub);
            ViewBag.Explicit = danhMucList;

            if (lesson != null)
            {
                var edit = new LesViewModel
                {
                    Tieude = lesson.TieuDe,
                    Video = lesson.Video,
                    Describe = lesson.Describe,
                    Idsub = lesson.Idsub,
                };
                return View(edit);
            }

            return View();
        }

        [HttpPost]
        public ActionResult SuaBaihoc(LesViewModel updatedSub, HttpPostedFileBase video)
        {
            try
            {
                // Check if a new image has been uploaded
                if (video != null && video.ContentLength > 0)
                {
                    // Call method to upload the new image and get its URL
                    string videoUrl = UploadVideo(video);

                    // Update the Images property with the new URL
                    updatedSub.Video = videoUrl;
                }

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PutAsJsonAsync("https://localhost:44388/api/Less/sua/" + updatedSub.Id, updatedSub).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Baihoc");
                }
                else
                {
                    ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật thông tin môn học.";
                    return RedirectToAction("Baihoc");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật thông tin môn học: " + ex.Message;
                return RedirectToAction("Baihoc");
            }
        }
    }
}