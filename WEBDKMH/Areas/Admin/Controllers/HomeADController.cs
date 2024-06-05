using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEBDKMH.Models;
using PagedList;

namespace WEBDKMH.Areas.Admin.Controllers
{
    public class HomeADController : Controller
    {

        // GET: Admin/Home
        public ActionResult Search(string search, int? page)
        {
            string apiUrl = "https://localhost:44388/api/EL/ds";
            if (!string.IsNullOrEmpty(search))
            {
                apiUrl = "https://localhost:44388/api/EL/dstheoten?ten=" + search;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            List<Users> users = response.Content.ReadAsAsync<List<Users>>().Result;

            int pageSize = 5; // Số lượng mục trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, nếu không có thì mặc định là 1

            IPagedList<Users> usersPagedList = users.ToPagedList(pageNumber, pageSize);

            ViewBag.IsSearchResult = !string.IsNullOrEmpty(search);
            ViewBag.SearchTerm = search;
            return View("QLUser", usersPagedList);
        }
        public ActionResult QLUser(int? page)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://localhost:44388/api/EL/ds").Result;
            List<Users> users = response.Content.ReadAsAsync<List<Users>>().Result;
            int pageSize = 5; // Số lượng mục trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại, nếu không có thì mặc định là 1

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult VerifyUser(int userId)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PostAsync("https://localhost:44388/api/EL/verifyuser?userId=" + userId, null).Result;
            bool result = response.Content.ReadAsAsync<bool>().Result;
            return RedirectToAction("QLUser");
        }

        public ActionResult DeleteUser(int userId)
        {
            // Gọi API để xóa người dùng
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync("https://localhost:44388/api/EL/xoauser?idus=" + userId).Result;

            // Đọc kết quả từ phản hồi
            bool result = response.Content.ReadAsAsync<bool>().Result;

            // Redirect lại trang QLUser sau khi xóa
            return RedirectToAction("QLUser");
        }

    }
}