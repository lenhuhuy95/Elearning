using Elearning.Common;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;

namespace Elearning.Areas.Admin.Controllers
{
    public class UserAdminController : BaseController
    {
        // GET: Admin/UserAdmin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_ADMINGROUP")]
        public ActionResult CreateGroup()
        {
            ViewBag.ListRole = new AdminGroupDao().ListRole();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_ADMINGROUP")]
        public ActionResult CreateGroup(AdminGroup group, string roleSelect)
        {
            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new AdminGroupDao();
                string id = dao.Insert(group);
                var result = InsertCredential(roleSelect, id);
                if (id != null && result == true)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("CreateGroup", "UserAdmin");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công");
                }
            }
            return View();
        }

        //danh sách quyền
        [HasCredential(RoleID = "VIEW_ADMINGROUP")]
        public ActionResult ListAdminGroup(string searchString, int page = 1, int pageSize = 10)
        {
            var model = new AdminGroupDao().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_ADMIN")]
        public ActionResult CreateAdmin()
        {
            ViewBag.GroupID = new AdminDao().ListGroupID(); //Show dữ liệu cho GroupID
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_ADMIN")]
        public ActionResult CreateAdmin(Model.EF.Admin admin, HttpPostedFileBase file, string listGroupSelect)
        {

            //Xử lý UploadImage
            if (file != null)
            {
                string path = Server.MapPath("~/Avatar/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;

                admin.Image = file.FileName;
            }
            else
            {
                admin.Image = "avatar.jpg";
            }

            //Lưu GroupID xuống csdl
            if (listGroupSelect != "")
            {
                admin.GroupID = listGroupSelect;
            }
            else
            {
                admin.GroupID = null;
            }

            admin.Status = true;

            if (ModelState.IsValid)
            {
                var dao = new AdminDao();

                if (dao.CheckUserAdmin(admin.UserAdmin))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(admin.PassWord);//mã hoá pass
                    admin.PassWord = encryptedMd5Pas;

                    admin.CreatedDate = DateTime.Now;

                    long id = dao.Insert(admin);
                    if (id > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        return RedirectToAction("CreateAdmin", "UserAdmin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm Admin thành công");
                    }
                }
            }
            return RedirectToAction("CreateAdmin", "UserAdmin");

        }

        //lấy thông tin userAdmin theo id
        [HttpGet]
        [HasCredential(RoleID = "EDIT_ADMIN")]
        public ActionResult Edit(long id)
        {
            var admin = new AdminDao().ViewDetail(id);
            ViewBag.GroupID = new AdminDao().ListGroupID(); //Show dữ liệu cho GroupID
            return View(admin);
        }

        //update userAdmin
        [HttpPost]
        [HasCredential(RoleID = "EDIT_ADMIN")]
        public ActionResult Edit(Model.EF.Admin admin, HttpPostedFileBase file, string listGroupSelect)
        {
            //Xử lý UploadImage
            if (file != null)
            {
                string path = Server.MapPath("~/Avatar/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;

                admin.Image = file.FileName; //Lấy tên ảnh khi được chọn
            }
            else
            {
                admin.Image = new AdminDao().ViewDetail(admin.AdminID).Image; //Nếu ko chỉnh sửa ảnh thì lấy ảnh hiện tại
            }

            //Lưu GroupID xuống csdl
            if (listGroupSelect != "")
            {
                admin.GroupID = listGroupSelect;
            }
            else
            {
                admin.GroupID = null;
            }

            if (ModelState.IsValid)
            {
                var dao = new AdminDao();
                if (!string.IsNullOrEmpty(admin.PassWord))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(admin.PassWord);
                    admin.PassWord = encryptedMd5Pas;
                }


                var result = dao.Update(admin);
                if (result)
                {
                    SetAlert("Sửa UserAdmin thành công", "success");
                    return RedirectToAction("CreateAdmin", "UserAdmin");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật UserAdmin không thành công");
                }
            }
            return RedirectToAction("CreateAdmin", "UserAdmin");
        }

        //Show danh sách user admin
        [HasCredential(RoleID = "VIEW_ADMIN")]
        public ActionResult ListAdmin(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            var model = new AdminDao().ListAllPaging(searchString, page, pageSize, session.AdminID, session.GroupID);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //Show danh sách lương cho giảng viên
        [HasCredential(RoleID = "VIEW_SALARY")]
        public ActionResult ListAdminSalary(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            var model = new AdminDao().ListAllPaging(searchString, page, pageSize, session.AdminID, session.GroupID);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //Trang chi tiết lương cho mỗi khóa học
        public ActionResult Detail(long id, string searchString, int page = 1, int pageSize = 10)
        {
            var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            ViewBag.GroupAdmin = session.GroupID;
            var model = new CourseDao().ListCourseIntructor(id, searchString, page, pageSize);
            int[] CountUser = new int[model.Count()];
            int i = 0;
            var dao = new UserDao();
            var dao2 = new SalaryDao();
            foreach (var item in model)
            {
                CountUser[i] = dao.CountUserCourse(item.CourseID) - dao2.CountUserCourseLast(item.CourseID);
                i++;
            }
            i = 0;

            float tong = 0;
            int j = 0;
            foreach (var item in model)
            {
                tong = tong + (CountUser[j] * (float)item.Cost * (float)item.Percent) / 100;
                j++;
            }
            j = 0;

            ViewBag.Tong = tong;
            ViewBag.CountUser = CountUser;
            return View(model);
        }

        //Tính lương khóa học với giảng viên
        public ActionResult InsertSalary(long adminid)
        {
            var model = new CourseDao().ListCourseIntructor2(adminid);
            int i = 0;
            int[] CountUser = new int[model.Count()];
            var dao = new UserDao();
            var dao2 = new SalaryDao();
            foreach (var item in model)
            {
                CountUser[i] = dao.CountUserCourse(item.CourseID) - dao2.CountUserCourseLast(item.CourseID);
                i++;
            }

            var salary = new Salary();
            int j = 0;
            foreach (var item in model)
            {
                if (CountUser[j] != 0)
                {
                    salary.CourseID = item.CourseID;
                    salary.Cost = item.Cost;
                    salary.Percent = item.Percent;
                    salary.CountUser = CountUser[j];
                    salary.SalaryTotal = item.Cost * item.Percent * CountUser[j] / 100;
                    salary.CreatedDate = DateTime.Now;
                    salary.AdminID = adminid;
                    dao2.Insert(salary);
                    j++;
                    continue;
                }
                j++;

            }
            i = 0;
            j = 0;
            return RedirectToAction("ListAdminSalary", "UserAdmin");
        }

        //Show lịch sử lương cho mỗi khóa học đã được thanh toán
        public ActionResult History(long id, string searchString, int page = 1, int pageSize = 10)
        {
            var model = new SalaryDao().History(id, searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.HistoryCourse = new CourseDao().ListAllCourse();
            return View(model);
        }

        //thay đổi trạng thái user bằng userAdminControllerChangeStatus.js
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new AdminDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa admin bằng [HttpDelete]
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_ADMIN")]
        public ActionResult DeleteAdmin(long id)
        {
            new AdminDao().DeleteAdmin(id);
            return RedirectToAction("ListAdmin");
        }

        //DeleteAdminGroup
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_ADMINGROUP")]
        public ActionResult DeleteAdminGroup(string id)
        {
            new AdminGroupDao().DeleteAdminGroup(id);
            new AdminGroupDao().RemoveCredential(id);
            return RedirectToAction("ListAdminGroup", "UserAdmin");
        }


        [HttpGet]
        [HasCredential(RoleID = "EDIT_ADMINGROUP")]
        public ActionResult EditGroupRole(string id)
        {
            var model = new AdminGroupDao().Detail(id);
            ViewBag.ListRole = new AdminGroupDao().ListRole();
            ViewBag.ListRoleSelected = new AdminGroupDao().ListRoleSelected(id);
            return View(model);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_ADMINGROUP")]
        public ActionResult EditGroupRole(AdminGroup admingr, string roleSelect)
        {
            if (ModelState.IsValid)
            {
                var dao = new AdminGroupDao();
                var result2 = EditCredential(roleSelect, admingr.ID);
                var result = dao.Update(admingr);
                if (result == true && result2 == true)
                {
                    SetAlert("Sửa nhóm quyền thành công", "success");
                    return RedirectToAction("CreateGroup", "UserAdmin");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật nhóm quyền không thành công");
                }
            }
            return RedirectToAction("CreateGroup", "UserAdmin");
        }

        //tạo mới nhóm quyền => thêm các quyền
        bool InsertCredential(string a, string ID)
        {
            String[] list = a.Split(',');


            var dao = new AdminGroupDao();


            for (int i = 0; i < list.Length; i++)
            {
                var cr = new Credential();
                cr.UserGroupID = ID;
                cr.RoleID = list[i];
                try
                {
                    dao.InsertCredential(cr);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        //chỉnh sửa các quyền của nhóm quyền
        bool EditCredential(string a, string ID)
        {
            String[] list = a.Split(',');

            var result = new AdminGroupDao().RemoveCredential(ID);

            try
            {
                var dao = new AdminGroupDao();


                for (int i = 0; i < list.Length; i++)
                {
                    var cr = new Credential();
                    cr.UserGroupID = ID;
                    cr.RoleID = list[i];
                    try
                    {
                        dao.InsertCredential(cr);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}