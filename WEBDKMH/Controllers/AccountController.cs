using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEBDKMH.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
//using System.Net.Mail;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WEBDKMH.Controllers
{
    public class AccountController : Controller
    {

        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44388/"); 
        }

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/EL/Singerds?ten={model.email}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadAsAsync<User>();
                    if (user != null && user.password == model.password)
                    {
                        if (user.IDRoll == 1 )
                        {
                            if (user.Verify == true)
                            {
                                Session["UserID"] = user.ID;
                                Session["UserName"] = user.Fullname;
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Tài khoản của bạn đang chờ xác nhận từ admin. Vui lòng đợi và thử lại sau.");
                            }
                        }
                        else if (user.IDRoll == 2)
                        {
                            Session["UserID"] = user.ID;
                            return RedirectToAction("QLUser", "HomeAD", new { area = "Admin" }); ;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên người dùng hoặc mật khẩu không đúng.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gọi API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear(); 
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage checkEmailResponse = await _httpClient.GetAsync($"api/EL/Singerds?ten={model.Email}");

                    if (checkEmailResponse.IsSuccessStatusCode)
                    {
                        var responseData = await checkEmailResponse.Content.ReadAsStringAsync();

                        if (responseData == "null")
                        {
                            if (model.Password != model.ConfirmPassword)
                            {
                                ModelState.AddModelError(string.Empty, "Password và Confirm Password không giống nhau.");
                                return View(model);
                            }
                            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/EL/themuser", model);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Login");
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gọi API.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Email đã được đăng ký. Vui lòng sử dụng email khác.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gọi API kiểm tra email.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
            }

            return View(model);
        }
       
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel updatedUser)
        {
            int userId = (int)Session["UserID"];
            HttpResponseMessage re = await _httpClient.GetAsync($"api/EditProfile/{userId}");
            var user = await re.Content.ReadAsAsync<Users>();
            updatedUser.Fullname = user.Fullname;
            updatedUser.Email = user.Email;
            updatedUser.Address = user.Address;
            updatedUser.Phone = user.Phone.HasValue ? user.Phone.ToString() : null;
            try
            {
                
                if (!ModelState.IsValid)
                {
                    return View(updatedUser);
                }

                if(user.Password == updatedUser.OldPassword)
                {
                    if (updatedUser.Password == updatedUser.ConfirmPassword)
                    {
                        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/EditProfile/{userId}", updatedUser);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["SuccessMessage"] = "Cập nhật Password người dùng thành công.";
                            
                            return View(updatedUser);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi cập nhật thông tin người dùng.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password và ConfirmPassword không giống nhau!!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "OldPassword và Password không giống nhau!!");
                }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
            }

            return View(updatedUser);
        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset(string email, ResetPasswordViewModel updatedUser)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/EL/Singerds?ten={email}");
                var user = await response.Content.ReadAsAsync<Users>();
                updatedUser.Fullname = user.Fullname;
                updatedUser.Email = user.Email;
                updatedUser.Address = user.Address;
                updatedUser.Phone = user.Phone.HasValue ? user.Phone.ToString() : null;

                if (response.IsSuccessStatusCode)
                {

                    updatedUser.Password = GenerateRandomPassword(8);

                    if (user != null)
                    {
                        HttpResponseMessage res = await _httpClient.PutAsJsonAsync($"api/EditProfile/{user.Id}", updatedUser);

                        SendPasswordResetEmail(email, updatedUser.Password);

                        ViewBag.Message = "Một email chứa mật khẩu mới đã được gửi đến địa chỉ email của bạn.";
                    }
                    else
                    {
                        ViewBag.Message = "Email không tồn tại trong hệ thống.";
                    }
                }
                else
                {
                    ViewBag.Message = "Đã xảy ra lỗi khi gọi API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Đã xảy ra lỗi: " + ex.Message;
            }

            return View();
        }

        private async Task SendPasswordResetEmail(string email, string newPassword)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DuFime", "truchcb11@gmail.com"));
            message.To.Add(new MailboxAddress("DuFime",email));
            message.Subject = "Reset Your Password";

            message.Body = new TextPart("plain")
            {
                Text = $"Your new password is: {newPassword}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls).ConfigureAwait(false);
                await client.AuthenticateAsync("truchcb11@gmail.com", "ngunguoi321");
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}