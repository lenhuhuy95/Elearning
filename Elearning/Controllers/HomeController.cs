using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elearning.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Dùng 1 lần thì ko cần khởi tạo
            ViewBag.Slides = new SlideDao().ListAll(); //Dùng ViewBag theo key Slides để lấy danh sách từ View

            var courseDao = new CourseDao(); //Dùng 2 lần nên khởi tạo lại lớp Dao.
            ViewBag.ListNewCourses = courseDao.ListNewCourse(top:4); //Dùng ViewBag theo key NewCourse để lấy danh sách (4) từ View
            ViewBag.ListTopCourses = courseDao.ListTopCourse(4);
            ViewBag.MoneyUnit = new MoneyDao().ListAllMoney(); //Show dữ liệu cho tiền

            return View();
        }

        [ChildActionOnly] //chỉ cho PartialView gọi
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1); //Lấy danh sách MainMenu theo TypeID
            return PartialView(model);
        }
    }
}