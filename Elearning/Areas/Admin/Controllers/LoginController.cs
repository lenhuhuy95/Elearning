using Elearning.Areas.Admin.Models;
using Elearning.Common;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elearning.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new AdminDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord)); //mã hoá passWord
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new AdminLogin();
                    userSession.UserAdminName = user.UserAdmin;
                    userSession.AdminID = user.AdminID;
                    userSession.GroupID = user.GroupID;
                    userSession.image = user.Image;
                    userSession.NameAdmin = user.NameAdmin;
                    Session.Add(CommonConstants.ADMIN_SESSION, userSession);

                    var listCredentials = dao.GetListCredential(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    

                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng");
                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.ADMIN_SESSION] = null;
            return Redirect("/Admin/Login/Index");
        }
    }
}