using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEBDKMH.Areas.Admin.Model;
using WEBDKMH.Models;

namespace WEBDKMH.Areas.Admin.Controllers
{
    public class QLMonHocController : Controller
    {
        // GET: Admin/QLKhoaHoc
        public ActionResult QLMonHoc(int? page)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/Sub/ds").Result;
            List<Subjects> subjects = response.Content.ReadAsAsync<List<Subjects>>().Result;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(subjects.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Search(string search, int? page)
        {
            string apiUrl = "https://localhost:44388/api/Sub/ds";
            if (!string.IsNullOrEmpty(search))
            {
                apiUrl = "https://localhost:44388/api/Sub/dstheoten?ten=" + search;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            List<Subjects> users = response.Content.ReadAsAsync<List<Subjects>>().Result;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            IPagedList<Subjects> usersPagedList = users.ToPagedList(pageNumber, pageSize);

            ViewBag.IsSearchResult = !string.IsNullOrEmpty(search);
            ViewBag.SearchTerm = search;
            return View("QLMonHoc", usersPagedList);
        }

        public ActionResult Delete(int Id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync("https://localhost:44388/api/Sub/xoa?idus=" + Id).Result;

            bool result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("QLMonHoc");
        }

        [HttpGet]
        public ActionResult AddMonHoc()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage categoryResponse = client.GetAsync("https://localhost:44388/api/Cate/ds").Result;
            List<Category> categories = categoryResponse.Content.ReadAsAsync<List<Category>>().Result;
            SelectList danhMucList = new SelectList(categories, "ID", "CateName");
            ViewBag.DanhMucList = danhMucList;

            return View();
        }

        [HttpPost]
        public ActionResult AddMonHoc(string Masubject, string SubjectTitle, string Describe, string DKtienquyet, int IdDanhMuc, HttpPostedFileBase hinhanh, int? Price)
        {
            try
            {
                string imageUrl = UploadHinhAnh(hinhanh);

                Subjects newsubject = new Subjects
                {
                    MaSubhect = Masubject,
                    SubjectTitle = SubjectTitle,
                    Describe = Describe,
                    Dktienquyet = DKtienquyet,
                    Images = imageUrl,
                    Idcate = IdDanhMuc,
                    Price = Price
                };

                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.PostAsJsonAsync("https://localhost:44388/api/Sub/them", newsubject).Result;

                if (response.IsSuccessStatusCode)
                {
                    Response.Write("<script>alert('successful');</script>");
                    return RedirectToAction("QLMonHoc");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi them mon hoc.");
                    return RedirectToAction("QLMonHoc");
                }
            }
            catch
            {
                return View();
            }           
        }

        [HttpPost]
        public string UploadHinhAnh(HttpPostedFileBase hinhanh)
        {
            try
            {
                if (hinhanh != null && hinhanh.ContentLength > 0)
                {
                    
                    string fileName = Path.GetFileName(hinhanh.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                    hinhanh.SaveAs(filePath);

                    
                    string imageUrl = Url.Content("~/Images/" + fileName);
                    return imageUrl;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult SuaMonHoc(int id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/Sub/sua/" + id).Result;
            var subject = response.Content.ReadAsAsync<Subject>().Result;

            HttpResponseMessage categoryResponse = client.GetAsync("https://localhost:44388/api/Cate/ds").Result;
            List<Category> categories = categoryResponse.Content.ReadAsAsync<List<Category>>().Result;
            SelectList danhMucList = new SelectList(categories, "ID", "CateName", subject.IDcate);
            ViewBag.Explicit = danhMucList;

            if (subject != null)
            {
                var edit = new SubViewModel
                {
                    Masubhect = subject.MaSubhect,
                    SubjectTitle = subject.SubjectTitle,
                    Describe = subject.Describe,
                    DKtienquyet = subject.DKtienquyet,
                    Images = subject.Images,
                    IDcate = subject.IDcate,
                    Price = subject.Price
                };
                return View(edit);
            }

            return View();
        }

        [HttpPost]
        public ActionResult SuaMonHoc(SubViewModel updatedSub, HttpPostedFileBase hinhanh)
        {
            try
            {
                // Check if a new image has been uploaded
                if (hinhanh != null && hinhanh.ContentLength > 0)
                {
                    // Call method to upload the new image and get its URL
                    string imageUrl = UploadHinhAnh(hinhanh);

                    // Update the Images property with the new URL
                    updatedSub.Images = imageUrl;
                }

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PutAsJsonAsync("https://localhost:44388/api/Sub/sua/" + updatedSub.Id, updatedSub).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("QLMonHoc");
                }
                else
                {
                    ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật thông tin môn học.";
                    return RedirectToAction("QLMonHoc");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật thông tin môn học: " + ex.Message;
                return RedirectToAction("QLMonHoc");
            }
        }

    }
}