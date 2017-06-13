using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using Elearning.Common;
using PagedList;
using System.Web.UI;

namespace Elearning.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Index(User user, HttpPostedFileBase file)
        {
            //Xử lý UploadImage
            if (file != null)
            {
                string path = Server.MapPath("~/Avatar/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;

                user.Image = file.FileName;
            }
            else
            {
                user.Image = "avatar.jpg";
            }

            user.Status = true;

            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                if (dao.CheckUserName(user.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");               
                }
                else if (dao.CheckEmail(user.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }                

                else
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.PassWord);//mã hoá pass
                    user.PassWord = encryptedMd5Pas;
                    user.CreatedDate = DateTime.Now;

                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        return View(user);
                    }
                    ModelState.AddModelError("", "Thêm User không thành công");
                }                
            }
            return View(user);

        }

        //lấy thông tin user theo id
        [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(long id)
        {
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }

        //update user
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user, HttpPostedFileBase file)
        {
            //Xử lý UploadImage
            if (file != null)
            {
                string path = Server.MapPath("~/Avatar/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;

                user.Image = file.FileName; //Lấy tên ảnh khi được chọn
            }
            else
            {
                user.Image = new UserDao().ViewDetail(user.UserID).Image; //Nếu ko chỉnh sửa ảnh thì lấy ảnh hiện tại
            }

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.PassWord))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.PassWord);
                    user.PassWord = encryptedMd5Pas;
                }


                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("ListUser", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View("Edit");
        }

        // view ListUser; lấy và hiển thị ds các user
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult ListUser(string searchString, int page = 1,int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //thay đổi trạng thái user bằng userController.js
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        ////xóa user bằng js userControllerDetele.js
        //[HttpPost]
        //public JsonResult DeleteUser(long id)
        //{
        //    var result = new UserDao().DeleteUserAd(id);
        //    return Json(new
        //    {
        //        status = result
        //    });
        //}

        //xóa user bằng [HttpDelete]
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult DeleteUser(long id)
        {
            new UserDao().DeleteUser(id);
            return RedirectToAction("ListUser");
        }
    }
}