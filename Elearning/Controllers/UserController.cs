using BotDetect.Web.Mvc;
using Elearning.Common;
using Elearning.Models;
using Facebook;
using Model.Dao;
using Model.EF;
using SmartAssembly.StringsEncoding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Elearning.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback"); //gọi tới hàm FacebookCallback
                return uriBuilder.Uri;
            }
        }

        //
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"], //Gán Id FbAppId được cấu hình từ Webconfig
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"], //Gán Id FbAppSecret được cấu hình từ Webconfig
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"], //Gán Id FbAppId được cấu hình từ Webconfig
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"], //Gán Id FbAppSecret được cấu hình từ Webconfig
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email,picture");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                string picture = "https://graph.facebook.com/me/picture?type=large&access_token=" + accessToken; //get picture

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.Image = picture;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.Name = new UserDao().ViewDetail(resultInsert).Name;
                    userSession.UserID = resultInsert;
                    userSession.Image = new UserDao().ViewDetail(resultInsert).Image; //Set picture
                    //userSession.Image = user.Image; //Set picture
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }

        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            var session = (UserLogin)Session[Elearning.Common.CommonConstants.USER_SESSION];
            var user = new UserDao().ViewDetail(session.UserID);

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(User user, HttpPostedFileBase file)
        {
            if(file !=null)
            {
                string path = Server.MapPath("~/Avatar/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path; //đường dẫn Video
                user.Image = file.FileName;
            }
            else
            {
                user.Image = new UserDao().ViewDetail(user.UserID).Image;
            }
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("EditProfile", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                }
            }
            return View();

        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.Name = user.Name;
                    userSession.UserID = user.UserID;
                    userSession.Image = user.Image;
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    return Content("Đúng");
                    /* return Redirect("/");*/ //Trả về đường dẫn hiện tại

                }
                else if (result == 0)
                {
                    //ModelState.AddModelError("", "Tài khoản không tồn tại.");
                    return Content("Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    //ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                    return Content("Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    //ModelState.AddModelError("", "Mật khẩu không đúng.");
                    return Content("Mật khẩu không đúng.");
                }
                else
                {
                    //ModelState.AddModelError("", "Đăng nhập không đúng.");
                    return Content("Đăng nhập không đúng.");
                }
            }
            return Content("Sai tài khoản hoặc mật khẩu");
            //return Redirect(url);
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = Server.MapPath("~/Avatar/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path; //đường dẫn Video
                model.Image = file.FileName;
            }
            else
            {
                model.Image = "avatar.jpg";
            }

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.PassWord = Encryptor.MD5Hash(model.Password);
                    user.Name = model.Name;
                    user.Image = model.Image;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;

                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel(); //khi đăng ký thành công thì tạo mới lại Form Đăng ký
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return View(model);
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}