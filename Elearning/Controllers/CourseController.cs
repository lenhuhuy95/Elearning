using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using Elearning.Models;

namespace Elearning.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Comment()
        {
            return View();
        }

        [ChildActionOnly] //chỉ cho PartialView gọi
        public PartialViewResult CourseCategory()
        {
            var model = new CourseCategoryDao().ListAll(); //Lấy danh sách tất cả danh sách chuyên mục có trong bảng
            return PartialView(model);
        }

        //Thực hiện gắn link cho từng danh mục khoá học theo cateId và lấy ra danh mục khoá học
        public ActionResult Category(long cateId, int page = 1, int pageSize = 4)
        {
            ViewBag.MoneyUnit = new MoneyDao().ListAllMoney(); //Show dữ liệu cho tiền

            var category = new CourseCategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new CourseDao().ListByCategoryId(cateId, ref totalRecord, page, pageSize); //Truyền dữ liệu tương ứng qua bên CourseDao trong hàm ListByCategoryId để lấy các khóa học của chuyên mục đó

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5; //số trang hiển thị tối đa
            int totalPage = 0; //Tổng số trang tính ra mặc định = 0

            //Lấy số bảng ghi(khóa học) chia cho pageSize rồi làm tròn lên
            double value = ((double)totalRecord / pageSize);
            totalPage = (int)Math.Ceiling(value);

            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }


        //Thực hiện gắn link cho từng chi tiêt khoá học theo detaId và lấy ra danh mục khoá học
        public ActionResult Detail(long detaId)
        {
            //Gọi session để lấy được user hiện tại
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];

            var courses = new CourseDao().ViewDetail(detaId);

            if (session == null)
            {
                ViewBag.SessionNow = "null"; //không có tài khoản đang đăng nhập check khi Click Take To Course
            }
            else
            {
                ViewBag.SessionNow = ""; // có tài khoản đang đăng nhập
            }

            //Xử lý nguồn Video
            if (courses.Video.Contains("www.youtube.com"))
            {
                ViewBag.videotype = "youtub";
            }
            if (courses.Video.Contains("vimeo.com"))
            {
                ViewBag.videotype = "vimeo";
            }

            //ViewBag.Category = new CourseCategoryDao().ViewDetail(courses.CategoryID.Value);
            ViewBag.InvolveCourse = new CourseDao().ListInvolveCourse(detaId); // Lấy khóa học liên quan
            ViewBag.ListCourse = new CourseDao().ListAllCourse(); //Lấy tất cả khóa học
            ViewBag.MoneyUnit = new MoneyDao().ListAllMoney(); //Show dữ liệu cho tiền
            ViewBag.Exam = new ExamDao().ListAll(detaId);
            if (session != null)
            {
                ViewBag.CheckUserExam = new ExamDao().CheckUserExam(session.UserID, detaId);
            }
            else
            {
                ViewBag.CheckUserExam = null;
            }

            //kiểm tra nếu có session hay chưa
            if (session != null)
            {
                ViewBag.CheckAccount = new CourseDao().CheckAccountUser(detaId, session.UserID);
            }
            else
            {
                ViewBag.CheckAccount = false;
            }
            return View(courses);
        }

        //Gọi Section để Show lên Client
        public ActionResult ListSection(long courseID, int page = 1, int pageSize = 10)
        {
            var dao = new SectionDao();
            var model = dao.ListAllPaging1(page, pageSize, courseID);
            //ViewBag.SearchString = searchString;
            return View(model);
        }

        //Gọi Lecture để Show lên Client
        public ActionResult ListLecture(long sectionID, int page = 1, int pageSize = 200)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var dao = new LectureDao();
            long detaId = new LectureDao().ViewCourseID(sectionID);
            var model = dao.ListAllPaging1(page, pageSize, sectionID);

            if (session == null)
            {
                ViewBag.SessionNow = "null"; //không có tài khoản đang đăng nhập check khi Click Take To Course
            }
            else
            {
                ViewBag.SessionNow = ""; // có tài khoản đang đăng nhập
            }

            //kiểm tra nếu có session hay chưa
            if (session != null)
            {
                ViewBag.CheckAccount = new CourseDao().CheckAccountUser(detaId, session.UserID);
            }
            else
            {
                ViewBag.CheckAccount = false;
            }
            //ViewBag.SearchString = searchString;
            return View(model);
        }

        //Show nội dung của từng chương tương ứng
        public ActionResult LectureContent(long detalecId)
        {
            var lecture = new LectureDao().ViewDetail(detalecId);
            ViewBag.InvolveLecture = new CourseDao().ListInvolveLecture(lecture.CourseID.Value);
            ViewBag.Exam = new ExamDao().ListAll(lecture.CourseID.Value);
            return View(lecture);
        }

        //Tạo nút đăng ký mua khóa học cho user
        public ActionResult InsertUserCourse(long courseId)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                var dao = new CourseDao();
                var userCourse = new User_Course();

                userCourse.CourseID = courseId;
                userCourse.UserID = session.UserID;
                dao.InsertUserCourse(userCourse);
            }
            return RedirectToAction("Detail", "Course", new { detaId = courseId });
        }

        [HttpGet]
        public ActionResult Exam(long id)
        {
            Exam ex = new ExamDao().ViewDetail(id);
            DateTime aDateTime = DateTime.Now;
            //compare1 mà nhỏ hơn 0 thì now sớm hơn ngày bắt đầu => chưa được thi
            int compare1 = DateTime.Compare(aDateTime, DateTime.Parse(ex.DateStart));
            //compare2 mà lớn hơn 0 thì now trễ hơn ngày kết thúc => hết thời hạn thi
            int compare2 = DateTime.Compare(aDateTime, DateTime.Parse(ex.DateEnd));

            int check = 0;
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            bool checkUser = new CourseDao().CheckAccountUser(ex.CourseID.Value, session.UserID);
            if (compare1 > 0 && compare2 < 0 && checkUser == true)
            {
                check = 1;
                ViewBag.Exam = ex;
                ViewBag.Question = new QuestionDao().ListQuestionIDCourse(ex.CourseID.Value);
                ViewBag.listques = new ExamDao().ListQuestionOfExam(id);
            }

            if (check == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult Exam(ListAnswer answer)
        {
            //inset đáp án vào csdl
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            long userID = session.UserID;

            var Examdao = new ExamDao();
            var entity = new Answer();
            for (int i = 0; i < answer.Answerrr.Count(); i++)
            {
                entity.ExamID = answer.Answerrr[i].ExamID;
                entity.CourseID = answer.Answerrr[i].CourseID;
                entity.UserID = userID;
                entity.QuesID = answer.Answerrr[i].QuesID;
                if (answer.Answerrr[i].Answer1 == null)
                {
                    entity.Answer1 = "";
                }
                else
                {
                    entity.Answer1 = answer.Answerrr[i].Answer1.ToString();
                }

                Examdao.InsertAnswer(entity);
            }

            //tính điểm
            long examID = answer.Answerrr[0].ExamID.Value;
            var listQues = new ExamDao().ListAllQuestionOfCourseID(examID, answer.Answerrr[0].CourseID.Value);
            int diem = 0;

            for (int i = 0; i < answer.Answerrr.Count(); i++)
            {
                if (answer.Answerrr[i].Answer1 == null)
                {
                    continue;
                }
                if (answer.Answerrr[i].Answer1.ToString().Equals(listQues[i].CorrectAnswer.ToString().Trim()))
                {
                    diem++;
                }
            }
            var score = new Score();
            score.UserID = userID;
            score.CourseID = answer.Answerrr[0].CourseID;
            score.ExamID = answer.Answerrr[0].ExamID;
            score.Score1 = diem.ToString();
            score.Date = DateTime.Now;

            long scoreID = Examdao.InsertScore(score);

            return RedirectToAction("ShowDiem", "Course", new { id = scoreID });
        }

        public ActionResult ShowDiem(long id)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            long userID = session.UserID;
            Score scr = new ExamDao().ScoreDetail(id);
            ViewBag.Score = scr;
            ViewBag.Question = new QuestionDao().ListQuestionIDCourse(scr.CourseID.Value);
            ViewBag.listques = new ExamDao().ListQuestionOfExam(scr.ExamID.Value);
            ViewBag.Answer = new ExamDao().ListAllAnswer(userID, scr.ExamID.Value);
            return View();
        }

        [HttpGet]
        public ActionResult SelectFile(long id)
        {
            ViewBag.examID = id;
            Exam ex = new ExamDao().ViewDetail(id);
            DateTime aDateTime = DateTime.Now;
            //compare1 mà nhỏ hơn 0 thì now sớm hơn ngày bắt đầu => chưa được thi
            int compare1 = DateTime.Compare(aDateTime, DateTime.Parse(ex.DateStart));
            //compare2 mà lớn hơn 0 thì now trễ hơn ngày kết thúc => hết thời hạn thi
            int compare2 = DateTime.Compare(aDateTime, DateTime.Parse(ex.DateEnd));
            int check2 = 0;
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            bool checkUser = false;
            if (session != null)
            {
                checkUser = new CourseDao().CheckAccountUser(ex.CourseID.Value, session.UserID);
            }

            if (compare1 > 0 && compare2 < 0 && checkUser == true)
            {
                check2 = 1;
                ViewBag.Exam = ex;
                //ViewBag.Question = new QuestionDao().ListQuestionIDCourse(ex.CourseID.Value);
                //ViewBag.listques = new ExamDao().ListQuestionOfExam(id);
                //check file của người dùng trong data
                ViewBag.fileSubmit = new ExamDao().CheckFileUser(id, session.UserID);
            }

            if (check2 == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SelectFile(SubmitFile subfile, HttpPostedFileBase file, string examID)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            if (file != null)
            {
                string path = Server.MapPath("~/File/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;
            }

            if (ModelState.IsValid)
            {
                var dao = new SelectFileDao();

                if (file == null)
                {
                    subfile.NameFile = "";
                }

                if (file != null)
                {
                    subfile.NameFile = file.FileName; //lưu tên đường dẫn xuống database
                    subfile.UserID = session.UserID;
                    subfile.ExamID = long.Parse(examID);
                }
                var fileSubmit = new ExamDao().CheckFileUser(long.Parse(examID), session.UserID);
                if (fileSubmit != null)
                {
                    //edit
                    subfile.FileID = fileSubmit.FileID;
                    long id = dao.Update(subfile);
                }
                else
                {
                    //insert
                    long id = dao.Insert(subfile);
                }

            }
            ViewBag.examID = examID;
            return RedirectToAction("SelectFile/" + examID,"Course");
        }
    }
}