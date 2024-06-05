using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEBDKMH.Models;
using WEBDKMH.Models.Payment;

namespace WEBDKMH.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44388/");
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Sub/ds");
                if (response.IsSuccessStatusCode)
                {
                    var lessons = await response.Content.ReadAsAsync<List<Subjects>>();
                    return View(lessons);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gọi API.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> Detail(int id)
        {
            try
            {              
                HttpResponseMessage res = await _httpClient.GetAsync($"api/Cou/sua/{id}");

                HttpResponseMessage response = await _httpClient.GetAsync($"api/Sub/sua/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    var courses = JsonConvert.DeserializeObject<List<Course>>(content);

                    var lesson = await response.Content.ReadAsAsync<Subjects>();
                    ViewBag.CourseList = courses;

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

        [HttpPost]
        public async Task<ActionResult> Buy(int id, int courseId)
        {
            if(Session["UserID"] == null)
            {
                TempData["Message"] = "Chưa Đăng nhập!!";
                return RedirectToAction("Detail", "Home", new { id = id });
            }
            var userId = (int)Session["UserID"];
            HttpResponseMessage re = await _httpClient.GetAsync($"api/Sub/sua/{id}");
            var s = await re.Content.ReadAsAsync<Subject>();
            HttpResponseMessage his = await _httpClient.GetAsync($"api/His/sua/{userId}");
            var h = await his.Content.ReadAsAsync<List<History>>();
            if (h != null)
            {
                var subjectIds = h.Select(history => history.IDSub);
                List<Subject> subjects = new List<Subject>();
                foreach (var subjectId in subjectIds)
                {
                    HttpResponseMessage subjectResponse = await _httpClient.GetAsync($"api/Sub/sua/{subjectId}");
                    if (subjectResponse.IsSuccessStatusCode)
                    {
                        var subject = await subjectResponse.Content.ReadAsAsync<Subject>();
                        subjects.Add(subject);
                    }
                }

                if (s.DKtienquyet != null)
                {
                    if (!subjects.Any(su => su.MaSubhect == s.DKtienquyet))
                    {
                        TempData["Message"] = "Không đủ điều kiện đăng ký";                        
                        return RedirectToAction("Detail", "Home", new { id = id });
                    }
                }
            }          
            try
            {
                var history = new HisViewModel
                {
                    Iduser = userId,
                    Idsub = id ,
                    Idcou = courseId
                };

                var url = UrlPayment(id);
                

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/His/them", history);
             
                if (response.IsSuccessStatusCode)
                {
                    Response.Write("<script>alert('successful');</script>");
                    return Redirect(url);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi mua khóa học.");
                    return RedirectToAction("");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return RedirectToAction("");
            }
        }

        public ActionResult Search(string search)
        {
            string apiUrl = "https://localhost:44388/api/Sub/ds";
            if (!string.IsNullOrEmpty(search))
            {
                apiUrl = "https://localhost:44388/api/Sub/dstheoten?ten=" + search;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            List<Subjects> users = response.Content.ReadAsAsync<List<Subjects>>().Result;

            ViewBag.IsSearchResult = !string.IsNullOrEmpty(search);
            return View("Index", users);
        }

        public ActionResult Category(int id)
        {           
                string Url = "https://localhost:44388/api/Sub/cate/" + id;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(Url).Result;
                List<Subjects> subjectsInCategory = response.Content.ReadAsAsync<List<Subjects>>().Result;                   
                return View("Index",subjectsInCategory);           
        }

        #region Thanh toán VnPay
        public string UrlPayment(int id)
        {
            var urlPayment = "";
            string apiUrl = "https://localhost:44388/api/Sub/sua/"+ id;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            var order = response.Content.ReadAsAsync<Subjects>().Result;
            //get config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Price * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000           
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan bai hoc:" + order.SubjectTitle);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return urlPayment;
        }
        #endregion

        public ActionResult PaymentCallback(string vnp_Amount, string vnp_ResponseCode, string vnp_TransactionStatus, string vnp_BankCode, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_TmnCode, string vnp_TransactionNo, string vnp_TxnRef, string vnp_SecureHash )
        {
            // Kiểm tra mã trạng thái thanh toán
            if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
            {
                // Thanh toán thành công
                ViewBag.PaymentStatus = "success";
            }
            else
            {
                // Thanh toán thất bại
                ViewBag.PaymentStatus = "failure";
            }

            // Trả về view với ViewBag.PaymentStatus được set
            return View();
        }

        public async Task<ActionResult> Exam(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Que/sua/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var questions = await response.Content.ReadAsAsync<List<Models.Question>>();
                    return View(questions);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gọi API.");
                    return View(new List<Models.Question>());
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return View(new List<Models.Question>());
            }
        }

        public async Task<ActionResult> Result( List<string> selectedAnswers)
        {
            if (Session["UserID"] == null)
            {
                TempData["Message"] = "Chưa Đăng nhập!!";
                return RedirectToAction("Index", "Home");
            }

            var userId = (int)Session["UserID"];
            try
            {
                for (int i = 0; i < selectedAnswers.Count; i++)
                {
                    var result = new ResultViewModel
                    {
                        IDuser = userId,
                        IDexam = 1,
                        IDques = i + 1,
                        answer = selectedAnswers[i]
                    };

                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Result/them", result);

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi lưu kết quả.");
                        return RedirectToAction("Exam", "Home", new { });
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return RedirectToAction("Exam", "Home", new {});
            }
        }


        public async Task<ActionResult> ShowResult(int id)
        {
            if (Session["UserID"] == null)
            {
                TempData["Message"] = "Chưa Đăng nhập!!";
                return RedirectToAction("Index", "Home");
            }

            var userId = (int)Session["UserID"];
            List<Models.Result> results = new List<Models.Result> ();
            List<Models.Question> questions = new List<Models.Question>();
            List<Models.History> Sub = new List<Models.History>();

            try
            {
                
                HttpResponseMessage resultResponse = await _httpClient.GetAsync($"api/Result/getResultsByExamId/{userId}");
                if (resultResponse.IsSuccessStatusCode)
                {
                    results = await resultResponse.Content.ReadAsAsync<List<Models.Result>>();
                }

                
                HttpResponseMessage questionResponse = await _httpClient.GetAsync($"api/Que/sua/{id}");
                if (questionResponse.IsSuccessStatusCode)
                {
                    questions = await questionResponse.Content.ReadAsAsync<List<Models.Question>>();
                }              

                
                var model = new ExamResultViewModel
                {
                    Questions = questions,
                    Results = results
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                return View();
            }
        }

    }
}
