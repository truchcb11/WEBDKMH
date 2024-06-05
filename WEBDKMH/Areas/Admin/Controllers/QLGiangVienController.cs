    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Mvc;
    using WEBDKMH.Models;
    using WEBDKMH.Areas.Admin.Model;

    namespace WEBDKMH.Areas.Admin.Controllers
    {
        public class QLGiangVienController : Controller
        {
            // GET: Admin/QLGiangVien
            public ActionResult QLGiangVien(int? page)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/Lec/ds").Result;
                List<Lecturers> users = response.Content.ReadAsAsync<List<Lecturers>>().Result;
                int pageSize = 5; // Số lượng mục trên mỗi trang
                int pageNumber = (page ?? 1); // Số trang hiện tại, nếu không có thì mặc định là 1

                return View(users.ToPagedList(pageNumber, pageSize));
            }

            public ActionResult Search(string search, int? page)
            {
                string apiUrl = "https://localhost:44388/api/Lec/ds";
                if (!string.IsNullOrEmpty(search))
                {
                    apiUrl = "https://localhost:44388/api/Lec/dstheoten?ten=" + search;
                }

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                List<Lecturers> users = response.Content.ReadAsAsync<List<Lecturers>>().Result;

                int pageSize = 5; // Số lượng mục trên mỗi trang
                int pageNumber = (page ?? 1); // Số trang hiện tại, nếu không có thì mặc định là 1

                IPagedList<Lecturers> usersPagedList = users.ToPagedList(pageNumber, pageSize);

                ViewBag.IsSearchResult = !string.IsNullOrEmpty(search);
                ViewBag.SearchTerm = search;
                return View("QLGiangVien", usersPagedList);
            }

            public ActionResult Delete(int Id)
            {
                // Gọi API để xóa người dùng
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.DeleteAsync("https://localhost:44388/api/Lec/xoa?idus=" + Id).Result;

                // Đọc kết quả từ phản hồi
                bool result = response.Content.ReadAsAsync<bool>().Result;

                // Redirect lại trang QLUser sau khi xóa
                return RedirectToAction("QLGiangVien");
            }

            [HttpGet]
            public ActionResult ThemGiangVien()
            {
                return View();
            }

            [HttpPost]
            public ActionResult ThemGiangVien(string email, string fullname, string address, string phone)
            {
                try
                {
                    int? phoneValue = string.IsNullOrEmpty(phone) ? null : (int.TryParse(phone, out int parsedPhone) ? (int?)parsedPhone : null);
                    // Tạo một đối tượng giảng viên từ dữ liệu được gửi từ form
                    Lecturers newLecturer = new Lecturers
                    {
                        Email = email,
                        Fullname = fullname,
                        Address = address,
                        Phone = phoneValue  
                    };

                    // Gọi API để thêm giảng viên mới
                    bool success = ThemGiangVienTuAPI(newLecturer);

                    if (success)
                    {
                        // Chuyển hướng về trang "QLGiangVien" sau khi thêm thành công
                        return RedirectToAction("QLGiangVien");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Có lỗi xảy ra khi thêm giảng viên.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Có lỗi xảy ra khi thêm giảng viên: " + ex.Message;
                    return View();
                }
            }

            private bool ThemGiangVienTuAPI(Lecturers lecturer)
            {
                try
                {
                    // Gửi yêu cầu POST tới API để thêm giảng viên
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = client.PostAsJsonAsync("https://localhost:44388/api/Lec/them", lecturer).Result;
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Có lỗi khi gọi API để thêm giảng viên: " + ex.Message);
                    return false;
                }
            }

            [HttpGet]
            public ActionResult SuaGiangVien(int id)
            {
                // Gọi API để lấy thông tin của giảng viên cần sửa
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/Lec/sua/" + id).Result;

                // Đọc thông tin của giảng viên từ phản hồi
                var lecturer = response.Content.ReadAsAsync<Lecturer>().Result;

                if (lecturer != null)
                {
                    var edit = new EditLecViewModel
                    {
                        Fullname = lecturer.Fullname,
                        Email = lecturer.email,
                        Address = lecturer.Address,
                        Phone = lecturer.phone.HasValue ? lecturer.phone.ToString() : null

                    };
                    return View(edit);
                }

                return View();
            }

            [HttpPost]
            public ActionResult SuaGiangVien(EditLecViewModel updatedLecturer)
            {
                try
                {
                    // Gọi API để cập nhật thông tin của giảng viên
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = client.PutAsJsonAsync("https://localhost:44388/api/Lec/sua/" + updatedLecturer.Id, updatedLecturer).Result;

                    // Kiểm tra xem cập nhật có thành công không
                    if (response.IsSuccessStatusCode)
                    {
                        // Chuyển hướng về trang "QLGiangVien" sau khi cập nhật thành công
                        return RedirectToAction("QLGiangVien");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật giảng viên.";
                        return View(updatedLecturer);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Có lỗi xảy ra khi cập nhật giảng viên: " + ex.Message;
                    return View(updatedLecturer);
                }
            }

        }
    }
