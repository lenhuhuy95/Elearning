using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elearning.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }

        //Hiển thị danh sách đơn hàng đã hoàn tất
        public ActionResult ListOrder(string searchString, int page = 1, int pageSize = 10)
        {
            var model = new OrderDao().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //Hiển thị danh sách đơn hàng đã hoàn tất
        public ActionResult ListOrderProcessing(string searchString, int page = 1, int pageSize = 10)
        {
            var model = new OrderDao().ListAllPaging1(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //Hiển thị danh sách chi tiết đơn hàng đã hoàn tất
        public ActionResult DetailOrder(long id, string searchString, int page = 1, int pageSize = 10)
        {

            var model = new OrderDao().ListOrderDetaiCourse(id, searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        //thay đổi trạng thái Order bằng orderControllerChangeStatus.js
        [HttpPost]
        public JsonResult ChangeStatusOrder(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            if(result)
            {
                //add user vào khóa học
                //lấy iduser
                long iduser = new OrderDao().ViewIDUser(id);
                var ds = new OrderDao().ListCourseOrderDetail(id);

                foreach(var item in ds)
                {
                    //thêm user vào từng khóa học trong ds
                    var UserCourse = new User_Course();
                    UserCourse.CourseID = item.CourseID;
                    UserCourse.Status = true;
                    UserCourse.UserID = iduser;
                    var ketqua = new CourseDao().InsertUserCourse1(UserCourse);
                }
                //lấy ds những khóa học
            }
            return Json(new
            {
                status = result
            });
        }

        // thêm học viên
        public ActionResult AddUserToCourse()
        {
            ViewBag.users = new UserDao().ListAllUser(); //Show dữ liệu cho users
            ViewBag.courses = new CourseDao().ListAllCourse(); //Show dữ liệu cho courses
            return View();
        }

        // thêm học viên
        [HttpPost]
        public ActionResult AddUserToCourse(string userid, string courseid)
        {
            var user_course = new User_Course();
            user_course.CourseID = long.Parse(courseid);
            user_course.UserID = long.Parse(userid);
            user_course.Status = true;
            var result = new CourseDao().InsertUserCourse1(user_course);
            ViewBag.users = new UserDao().ListAllUser(); //Show dữ liệu cho users
            ViewBag.courses = new CourseDao().ListAllCourse(); //Show dữ liệu cho courses
            return View();
        }
    }
}