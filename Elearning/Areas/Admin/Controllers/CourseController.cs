using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using System.Text.RegularExpressions;
using System.IO;
using OfficeOpenXml;
using Elearning.Models;
using Model.ViewModel;

namespace Elearning.Areas.Admin.Controllers
{
    public class CourseController : BaseController
    {
        // GET: Admin/Course
        public ActionResult Index()
        {
            return View();
        }

        //Ban đầu sẽ vào phương thức HttpGet
        [HttpGet]
        [HasCredential(RoleID = "ADD_COURSE")]
        public ActionResult Create()
        {
            ViewBag.ListTag = new TagDao().ListAll(); //Show dữ liệu cho Tag
            ViewBag.category = new CategoryDao().List(); //Show dữ liệu cho chuyên mục
            ViewBag.Instructed = new AdminDao().List(); //show dữ liệu cho instructor
            ViewBag.RelatedCourses = new CourseDao().ListAllCourse(); //dùng cho khóa học liên quan
            ViewBag.ListMoney = new MoneyDao().ListAllMoney(); // dùng cho chọn đơn vị tiền tệ
            return View();
        }

        //Hàm xử lý tách chuỗi cho TagName và Lưu Tag xuống csdl bảng CourseTag
        void InsertCourseTag(string a, long courseID)
        {
            String[] listTagID = a.Split(',');


            var dao = new CourseTagDao();


            for (int i = 0; i < listTagID.Length; i++)
            {
                var coursetag = new CourseTag();
                coursetag.CourseID = courseID;
                coursetag.TagID = long.Parse(listTagID[i]);
                try
                {
                    dao.Insert(coursetag);
                }
                catch (Exception e)
                {

                }
            }
        }

        //tachs id course liên quan
        void InsertRelatedCoursesSelected(string a, long courseID)
        {
            String[] listCourseID = a.Split(',');
            var dao = new RelatedCoursDao();
            for (int i = 0; i < listCourseID.Length; i++)
            {
                var RelatedCourse = new RelatedCours();
                RelatedCourse.CourseID = courseID;
                RelatedCourse.RelatedID = long.Parse(listCourseID[i]);
                try
                {
                    dao.Insert(RelatedCourse);
                }
                catch (Exception e)
                {

                }
            }
        }

        //edit khóa học liên quan

        void EditRelatedCourses(string a, long courseID)
        {
            String[] listCourseID = a.Split(',');

            var result = new CourseDao().RemoveRelatedCourses(courseID);

            try
            {
                var dao = new RelatedCoursDao();


                for (int i = 0; i < listCourseID.Length; i++)
                {
                    var RelatedCourse = new RelatedCours();
                    RelatedCourse.CourseID = courseID;
                    RelatedCourse.RelatedID = long.Parse(listCourseID[i]);
                    try
                    {
                        dao.Insert(RelatedCourse);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }

        }

        //edit Tag của khóa học
        void EditCourseTag(string a, long courseID)
        {
            String[] listTagID = a.Split(',');

            var result = new CourseTagDao().Remove(courseID);

            try
            {
                var dao = new CourseTagDao();

                for (int i = 0; i < listTagID.Length; i++)
                {
                    var coursetag = new CourseTag();
                    coursetag.CourseID = courseID;
                    coursetag.TagID = long.Parse(listTagID[i]);
                    try
                    {
                        dao.Insert(coursetag);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }

        }

        //Sau Khi post Form lên (vào) là phương thức HttpPost
        [HttpPost]
        [ValidateInput(false)] //Xử lí úp nội dung trong Editor của Detail
        //Hàm tạo khoá học
        [HasCredential(RoleID = "ADD_COURSE")]
        public ActionResult Create(Course course, HttpPostedFileBase file, string listTagSelect, string listCategorySelect, string instructedselected, string linkvideo, string RelatedCoursesSelected, string moneyselected)
        {
            //Xử lý UploadVideo
            if (file != null)
            {
                string path = Server.MapPath("~/Video/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;
            }

            if (moneyselected != "")
            {
                course.MoneyID = long.Parse(moneyselected);
            }
            else
            {
                //course.MoneyID = long.Parse(moneyselected);
            }
            //Chuyển đổi tiêu đề sáng slug
            course.MetaTitle = ConvertToUnSign(course.CourseName);

            //Lấy ngày hiện tiện khi tạo khóa học
            course.CreateDate = DateTime.Now;

            //Mặc định khóa học tạo ra là true
            course.Status = false;
            //Lưu Instructed xuống csdl
            course.Instructed = long.Parse(instructedselected);
            //Lưu CategoryID xuống csdl
            if (listCategorySelect != "")
            {
                course.CategoryID = long.Parse(listCategorySelect);
            }
            else
            {
                course.CategoryID = null;
            }

            //Xử lý Insert dữ liệu vào Database        
            if (ModelState.IsValid)
            {
                var dao = new CourseDao();
                //course.Video = file.FileName; //lưu tên đường dẫn video xuống database
                if (file == null && linkvideo != "")
                {
                    course.Video = linkvideo;
                }

                if (file != null)
                {
                    course.Video = file.FileName; //lưu tên đường dẫn video xuống database
                }

                long id = dao.Insert(course);
                if (id > 0)
                {
                    if (listTagSelect != "")
                    {
                        InsertCourseTag(listTagSelect, id); // Lưu TagId và CourseID xuống bảng CourseTag
                    }
                    if (RelatedCoursesSelected != "")
                    {
                        InsertRelatedCoursesSelected(RelatedCoursesSelected, id); // lưu khóa học liên quan
                    }

                    return RedirectToAction("Create", "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Course thành công");
                }
            }

            return View("Create");
        }

        //Hàm chuyển đổi tiêu đề sáng slug
        public static string ConvertToUnSign(string text)
        {
            text = text.ToLower();
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            text = text.Replace(" ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        //Hàm sửa khoá học lấy thông tin Course theo id
        [HttpGet]
        [HasCredential(RoleID = "EDIT_COURSE")]
        public ActionResult EditCourse(long id)
        {
            var course = new CourseDao().ViewDetail(id);
            ViewBag.ListTag = new TagDao().ListAll(); //Show dữ liệu cho Tag
            ViewBag.ListTagSelect = new CourseDao().ListTagSelected(id); //Show dữ liệu khi đã được chọn khi chọn Tag
            ViewBag.categoryselect = new CourseDao().ListSelectCourseCategory(id); //Show dữ liệu khi đã được chọn khi Create Category
            ViewBag.Instructed = new AdminDao().List(); //show dữ liệu cho instructor
            ViewBag.RelatedCoursesSelect = new CourseDao().RelatedCoursesSelect(id); //dùng cho khóa học liên quan
            ViewBag.RelatedCourses = new CourseDao().ListAllCourse(); //dùng cho khóa học liên quan
            ViewBag.category = new CategoryDao().List(); //Show dữ liệu cho chuyên mục
            ViewBag.ListMoney = new MoneyDao().ListAllMoney(); // dùng cho chọn đơn vị tiền tệ
            return View(course);
        }

        //update Course
        [HttpPost]
        [ValidateInput(false)] //Xử lí úp nội dung trong Editor của Detail
        [HasCredential(RoleID = "EDIT_COURSE")]
        public ActionResult EditCourse(Course course, HttpPostedFileBase file, long id, string listTagSelect, string listcategoryselect, string Instructedselect, string linkvideo, string RelatedCoursesSelected, string imageurl, string moneyselected)
        {
            if (moneyselected != "")
            {
                course.MoneyID = long.Parse(moneyselected);
            }
            else
            {
                //course.MoneyID = long.Parse(moneyselected);
            }
            //Nếu muốn cập nhật lại Video
            if (file != null)
            {
                //Xử lý UploadVideo
                string path = Server.MapPath("~/Video/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;

                //Chuyển đổi tiêu đề sáng slug
                course.MetaTitle = ConvertToUnSign(course.CourseName);

                course.Instructed = long.Parse(Instructedselect); //Lấy ID Giảng Viên

                //Xử lý Update dữ liệu vào Database   
                if (ModelState.IsValid)
                {
                    var dao = new CourseDao();
                    course.Video = file.FileName; //lưu tên đường dẫn file video xuống database
                    course.MetaTitle = ConvertToUnSign(course.CourseName); //Hàm chuyển đổi Slug
                    course.CategoryID = long.Parse(listcategoryselect); //Lấy Id của chuyên mục

                    //Xử lý upload image
                    if (imageurl != "")
                    {
                        course.Image = imageurl;
                    }
                    else
                    {
                        course.Image = new CourseDao().ViewDetail(id).Image; //Lấy image hiện tại nếu ko chọn gì
                    }
                    EditCourseTag(listTagSelect, id); //Lấy ID của Tag
                    EditRelatedCourses(RelatedCoursesSelected, id); //Lấy Id của khóa học liên quan
                    var result = dao.Update(course);
                    if (result)
                    {
                        if (listTagSelect != "")
                        {
                            InsertCourseTag(listTagSelect, id); // Lưu TagId và CourseID xuống bảng CourseTag
                        }

                        SetAlert("Sửa khoá học thành công", "success");
                        return RedirectToAction("ListCourses", "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật khoá học không thành công");
                    }
                    return View("EditCourse");
                }
            }
            if (file == null && linkvideo == "")
            {
                //Xử lý Update dữ liệu vào Database   
                if (ModelState.IsValid)
                {
                    var dao = new CourseDao();
                    course.Video = new CourseDao().ViewDetail(course.CourseID).Video; //Lấy Video hiện tại khi không chọn

                    course.Instructed = long.Parse(Instructedselect); //Lấy ID Giảng Viên

                    //Lấy ID của chuyên mục
                    if (listcategoryselect != "")
                    {
                        course.CategoryID = long.Parse(listcategoryselect);
                    }
                    else
                    {
                        course.CategoryID = null;
                    }

                    //Xử lý upload ảnh
                    if (imageurl != "")
                    {
                        course.Image = imageurl;
                    }
                    else
                    {
                        course.Image = new CourseDao().ViewDetail(id).Image; //Lấy image hiện tại nếu ko chọn gì
                    }
                    EditCourseTag(listTagSelect, id); //chỉnh sửa tag khóa học
                    EditRelatedCourses(RelatedCoursesSelected, id); //chỉnh sửa khóa học liên quan
                    var result = dao.Update(course);
                    if (result)
                    {
                        SetAlert("Sửa khoá học thành công", "success");
                        return RedirectToAction("ListCourses", "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật khoá học không thành công");
                    }
                    return View("EditCourse");
                }
            }
            if (file == null && linkvideo != "")
            {
                //Xử lý Update dữ liệu vào Database   
                if (ModelState.IsValid)
                {
                    var dao = new CourseDao();

                    course.Video = linkvideo; //lưu tên đường dẫn file video xuống database

                    course.Instructed = long.Parse(Instructedselect); //Lấy ID Giảng Viên
                    if (listcategoryselect != "")
                    {
                        course.CategoryID = long.Parse(listcategoryselect);
                    }
                    else
                    {
                        course.CategoryID = null;
                    }

                    //Xử lý upload image
                    if (imageurl != "")
                    {
                        course.Image = imageurl;
                    }
                    else
                    {
                        course.Image = new CourseDao().ViewDetail(id).Image; //Lấy image hiện tại nếu ko chọn gì
                    }
                    EditCourseTag(listTagSelect, id);//chỉnh sửa tag khóa học
                    EditRelatedCourses(RelatedCoursesSelected, id); //chỉnh sửa khóa học liên quan
                    var result = dao.Update(course);
                    if (result)
                    {
                        SetAlert("Sửa khoá học thành công", "success");
                        return RedirectToAction("ListCourses", "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật khoá học không thành công");
                    }
                    return View("EditCourse");
                }
            }
            return View("ListCourses");
        }

        //Hàm sửa Section lấy thông tin Section theo id
        [HttpGet]
        public ActionResult EditSection(long id)
        {
            var section = new SectionDao().ViewDetail(id);
            return View(section);
        }

        //update section
        [HttpPost]
        public ActionResult EditSection(Section section)
        {
            if (ModelState.IsValid)
            {
                var dao = new SectionDao();

                var result = dao.Update(section);
                if (result)
                {
                    SetAlert("Sửa chương thành công", "success");
                    return RedirectToAction("ListSection/" + section.CourseID, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật chương không thành công");
                }
            }

            return View("EditSection");
        }

        //Hàm sửa Lecture lấy thông tin Lecture theo từng id
        [HttpGet]
        public ActionResult EditLecture(long id)
        {
            var lecture = new LectureDao().ViewDetail(id);
            return View(lecture);
        }

        //update lecture
        [HttpPost]
        [ValidateInput(false)] //Xử lí úp nội dung trong Editor của Detail
        public ActionResult EditLecture(Lecture lecture, HttpPostedFileBase file, HttpPostedFileBase file1, string linkvideo)
        {
            //Xử lý UploadVideo
            //trường hợp cần sửa lại video và Fileattached, với tên file, file1 ban đầu truyền từ nút Video,Fileattached
            if (file != null && file1 != null)
            {
                string path = Server.MapPath("~/Video/" + file.FileName);
                string path1 = Server.MapPath("~/Video/" + file1.FileName);
                file.SaveAs(path);
                file1.SaveAs(path1);
                ViewBag.Path = path; //đường dẫn Video
                ViewBag.Path = path1; //đường dẫn Fileattached

                //Xử lý Update dữ liệu vào Database 
                if (ModelState.IsValid)
                {
                    var dao = new LectureDao();
                    lecture.Video = file.FileName; //lưu đường dẫn video xuống database
                    lecture.Fileattached = path1; //lưu đường dẫn Fileattached xuống database
                    var result = dao.Update(lecture);
                    if (result)
                    {
                        SetAlert("Sửa bài học thành công", "success");
                        return RedirectToAction("ListLecture/" + lecture.SectionID, "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật bài học không thành công");
                    }
                }
                return View("EditLecture");
            }

            //Trường hợp khi ko cần sửa lại video mà cần sửa Fileattached
            if (file == null && file1 != null)
            {
                string path1 = Server.MapPath("~/Video/" + file1.FileName);
                file1.SaveAs(path1);
                ViewBag.Path = path1;

                //Xử lý Update dữ liệu vào Database 
                if (ModelState.IsValid)
                {
                    var dao = new LectureDao();
                    if (linkvideo == "")
                    {
                        lecture.Video = new LectureDao().ViewDetail(lecture.LectureID).Video; //Lấy Video hiện tại khi không chọn
                    }
                    else
                    {
                        lecture.Video = linkvideo;
                    }
                    lecture.Fileattached = path1; //lưu đường dẫn Fileattached xuống database
                    var result = dao.Update(lecture);
                    if (result)
                    {
                        SetAlert("Sửa bài học thành công", "success");
                        return RedirectToAction("ListLecture/" + lecture.SectionID, "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật bài học không thành công");
                    }
                }
                return View("EditLecture");
            }

            //Trường hợp khi cần sửa lại video mà ko cần sửa Fileattached
            if (file != null && file1 == null)
            {
                string path = Server.MapPath("~/Video/" + file.FileName);
                file.SaveAs(path);
                ViewBag.Path = path;

                //Xử lý Update dữ liệu vào Database 
                if (ModelState.IsValid)
                {
                    var dao = new LectureDao();
                    lecture.Video = file.FileName; //lưu đường dẫn Fileattached xuống database
                    lecture.Fileattached = new LectureDao().ViewDetail(lecture.LectureID).Fileattached; //Lấy Fileattached hiện tại khi không chọn
                    var result = dao.Update(lecture);
                    if (result)
                    {
                        SetAlert("Sửa bài học thành công", "success");
                        return RedirectToAction("ListLecture/" + lecture.SectionID, "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật bài học không thành công");
                    }
                }
                return View("EditLecture");
            }

            //Trường hợp khi ko cần sửa lại video và ko cần sửa Fileattached
            if (file == null && file1 == null)
            {
                //Xử lý Update dữ liệu vào Database 
                if (ModelState.IsValid)
                {
                    var dao = new LectureDao();
                    if (linkvideo == "")
                    {
                        lecture.Video = new LectureDao().ViewDetail(lecture.LectureID).Video; //Lấy Video hiện tại khi không chọn
                    }
                    else
                    {
                        lecture.Video = linkvideo;
                    }
                    lecture.Fileattached = new LectureDao().ViewDetail(lecture.LectureID).Fileattached; //Lấy Fileattached hiện tại khi không chọn
                    var result = dao.Update(lecture);
                    if (result)
                    {
                        SetAlert("Sửa bài học thành công", "success");
                        return RedirectToAction("ListLecture/" + lecture.SectionID, "Course");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật bài học không thành công");
                    }
                }
                return View("EditLecture");
            }

            return View("EditLecture");
        }

        ////Truyền CategoryID theo trường ứng với table để xử lý Dropdownlist
        //public void SetViewBag(long? selectedId = null)
        //{
        //    var dao = new CategoryDao();
        //    ViewBag.CategoryID = new SelectList(dao.ListAll(), "CategoryID", "CategoryName", selectedId);
        //}

        //thay đổi trạng thái course bằng courseControllerChangeStatus.js
        [HttpPost]
        [HasCredential(RoleID = "STATUS_COURSE")]
        public JsonResult ChangeStatusCourse(long id)
        {
            var result = new CourseDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //thay đổi trạng thái Chương bằng sectionControllerChangeStatus.js
        [HttpPost]
        public JsonResult ChangeStatusSection(long id)
        {
            var result = new SectionDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa khóa học bằng courseControllerDelete.js
        //[HttpPost]
        //public JsonResult DeleteCourse(long id)
        //{
        //    var result = new CourseDao().DeleteCourse(id);
        //    return Json(new
        //    {
        //        status = result
        //    });
        //}

        //xóa khoá học bằng [HttpDelete]
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_COURSE")]
        public ActionResult DeleteCourse(long id)
        {
            new CourseDao().DeleteCourse(id);
            return RedirectToAction("ListCourses");
        }

        //xóa Chương bằng sectionControllerDelete.js
        //[HttpPost]
        //public JsonResult DeleteSection(long id)
        //{
        //    //Khi xoá Section thì phải trừ đi 1 section trong Course
        //    var daosection = new SectionDao();
        //    var section = daosection.ViewDetail(id);
        //    var dao = new CourseDao();
        //    var course = dao.ViewDetail(section.CourseID);
        //    course.NumberSection--;
        //    dao.Update(course);

        //    var result = new SectionDao().DeleteSection(id);
        //    return Json(new
        //    {
        //        status = result
        //    });
        //}

        //xóa chương bằng [HttpDelete]
        [HttpDelete]
        public ActionResult DeleteSection(long id)
        {
            //xóa section thì xóa luôn lecture của section đó. đếm lại số section hiện tại cập nhật lại course
            var daosection = new SectionDao();
            var section = daosection.ViewDetail(id);
            var dao = new CourseDao();
            var course = dao.ViewDetail(section.CourseID.Value);



            new SectionDao().DeleteSection(id);
            course.NumberSection = new SectionDao().getNum(course.CourseID);
            dao.Update(course);
            return RedirectToAction("ListSection");
        }

        // View ListCourses; lấy và hiển thị danh sách các khoá học
        [HasCredential(RoleID = "VIEW_COURSE")]
        public ActionResult ListCourses(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            var dao = new CourseDao();
            var model = dao.ListAllPaging(searchString, page, pageSize, session.AdminID, session.GroupID);
            ViewBag.Instructed = new AdminDao().List(); //show dữ liệu cho instructor
            ViewBag.SearchString = searchString; //Search cho nhiều trang
            ViewBag.money = new MoneyDao().ListAllMoney();
            return View(model);
        }

        //trả về ds các section của course thành table
        [HttpGet]
        public ActionResult ListSection(long id, string searchString, int page = 1, int pageSize = 10)
        {
            //tạo section
            var section = new Section();
            var course = new CourseDao().ViewDetail(id); //để lấy course theo ID
            var dao = new SectionDao();
            string a = "Section Name";

            if (dao.getNum(id) == 0) //nếu chưa có Section thì tạo mới
            {
                for (int i = 0; i < course.NumberSection; i++)
                {
                    section.SectionName = a;
                    section.CourseID = id;
                    section.Status = true;
                    dao.Insert(section);
                }
            }
            else
            {
                if (dao.getNum(id) < course.NumberSection)//TH tạo Section rồi nhưng muốn thêm section
                {
                    for (int i = dao.getNum(id); i < course.NumberSection; i++)
                    {
                        section.SectionName = a;
                        section.CourseID = id;
                        section.Status = true;
                        dao.Insert(section);
                    }
                }
            }
            var model = dao.ListAllPaging(searchString, page, pageSize, id); //Tìm ra tất cả các Section ứng với từng CourseID
            ViewBag.SearchString = searchString; //Search cho nhiều trang

            return View(model);
        }

        //trả về ds các Lecture ứng với từng Section trong table
        [HttpGet]
        public ActionResult ListLecture(long id, string searchString, int page = 1, int pageSize = 10)
        {
            //tạo section
            var lecture = new Lecture();
            var section = new SectionDao().ViewDetail(id); //để lấy Section theo ID
            var dao = new LectureDao();
            string a = "Lecture Name";

            if (dao.getNum(id) == 0) //nếu chưa có lecture thì tạo mới
            {
                for (int i = 0; i < section.NumberLecture; i++)
                {
                    lecture.LectureName = a;
                    lecture.SectionID = id;
                    lecture.CourseID = section.CourseID;
                    lecture.Status = true;
                    dao.Insert(lecture);
                }
            }
            else
            {
                if (dao.getNum(id) < section.NumberLecture)//TH tạo Lecture rồi nhưng muốn thêm Lecture
                {
                    for (int i = dao.getNum(id); i < section.NumberLecture; i++)
                    {
                        lecture.LectureName = a;
                        lecture.SectionID = id;
                        lecture.CourseID = section.CourseID;
                        lecture.Status = true;
                        dao.Insert(lecture);
                    }
                }
            }

            var model = dao.ListAllPaging(searchString, page, pageSize, id); //Tìm ra tất cả các Lecture ứng với từng SectionID
            ViewBag.SearchString = searchString; //Search cho nhiều trang

            return View(model);
        }

        //thay đổi trạng thái bài học bằng lectureControllerChangeStatus.js
        [HttpPost]
        public JsonResult ChangeStatusLecture(long id)
        {
            var result = new LectureDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa bài học bằng lectureControllerDelete.js
        //[HttpPost]
        //public JsonResult DeleteLecture(long id)
        //{
        //    //Khi xoá Lecture thì phải trừ đi 1 lecture trong Section
        //    var daolecture = new LectureDao();
        //    var lecture = daolecture.ViewDetail(id);
        //    var dao = new SectionDao();
        //    var section = dao.ViewDetail(lecture.SectionID);
        //    section.NumberLecture--;
        //    dao.Update(section);

        //    var result = new LectureDao().DeleteLecture(id);
        //    return Json(new
        //    {
        //        status = result
        //    });
        //}

        //xóa bài học bằng [HttpDelete]
        [HttpDelete]
        public ActionResult DeleteLecture(long id)
        {
            //xóa lecture xong thì đếm lại số lecture hiện có trong data để cập nhật lại section tương ứng
            var daolecture = new LectureDao();
            var lecture = daolecture.ViewDetail(id);
            var dao = new SectionDao();
            var section = dao.ViewDetail(lecture.SectionID.Value);
            new LectureDao().DeleteLecture(id);
            section.NumberLecture = new LectureDao().getNum(section.SectionID);
            dao.Update(section);
            return RedirectToAction("ListLecture");
        }

        public ActionResult ListUserCourses(long id, string searchString, int page = 1, int pageSize = 10)
        {
            //var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            var dao = new CourseDao();
            var model = dao.ListCourseUserCourse(id, searchString, page, pageSize);

            ViewBag.SearchString = searchString; //Search cho nhiều trang

            return View(model);
        }
        //thay đổi trạng thái học viên bằng userCourseControllerChangeStatus.js
        [HttpPost]
        public JsonResult ChangeStatusUserCourse(long id)
        {
            var result = new CourseDao().ChangeStatusUserCourse(id);
            return Json(new
            {
                status = result
            });
        }
        //xóa học viên
        [HttpDelete]
        public ActionResult DeleteUserCourse(long id)
        {

            var result = new CourseDao().DeleteUserCourse(id);
            return RedirectToAction("ListUserCourse");
        }

        // View ListExam; lấy và hiển thị danh sách các bài kiểm tra
        //[HasCredential(RoleID = "VIEW_COURSE")]
        [HttpGet]
        public ActionResult ListExams(string searchString, long id, int page = 1, int pageSize = 10)
        {
            var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            var dao = new CourseDao();
            //var model = dao.ListAllPaging(searchString, page, pageSize, session.AdminID, session.GroupID);
            var model = dao.ListAllExam(searchString, page, pageSize, session.AdminID, session.GroupID, id);
            ViewBag.Instructed = new AdminDao().List(); //show dữ liệu cho instructor
            ViewBag.SearchString = searchString; //Search cho nhiều trang
            ViewBag.courseid = id;
            return View(model);
        }

        // View ListQuestion; lấy và hiển thị danh sách các câu hỏi
        [HttpGet]
        public ActionResult ListQuestion(string searchString, long id, int page = 1, int pageSize = 10)
        {
            var session = (Elearning.Common.AdminLogin)Session[Elearning.Common.CommonConstants.ADMIN_SESSION];
            var dao = new CourseDao();
            //var model = dao.ListAllPaging(searchString, page, pageSize, session.AdminID, session.GroupID);
            var model = dao.ListAllQuestion(searchString, page, pageSize, session.AdminID, id);
            ViewBag.Instructed = new AdminDao().List(); //show dữ liệu cho instructor
            ViewBag.SearchString = searchString; //Search cho nhiều trang
            ViewBag.courseid = id;
            return View(model);
        }

        //thay đổi trạng thái user bằng questionController.js
        [HttpPost]
        public JsonResult ChangeStatusQuestion(long id)
        {
            var result = new QuestionDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        //xóa question bằng [HttpDelete]
        [HttpDelete]
        public ActionResult DeleteQuestion(long id)
        {
            new QuestionDao().DeleteQuestion(id);
            return RedirectToAction("ListQuestion");
        }

        [HttpGet]
        //[HasCredential(RoleID = "ADD_COURSE")]
        public ActionResult CreateExam(long id)
        {
            ViewBag.courseid = id;
            ViewBag.ListQues = new QuestionDao().ListQuestionIDCourse(id);
            return View();
        }

        void TachChuoiThoiGian(Exam exam, string start, string end)
        {
            start.Trim();
            exam.TimeStart = start.Substring(10, 8);
            exam.DateStart = start.Substring(0, 10);

            end.Trim();
            exam.TimeEnd = end.Substring(10, 8);
            exam.DateEnd = end.Substring(0, 10);
        }

        //Hàm xử lý tách chuỗi cho TagName và Lưu Tag xuống csdl bảng CourseTag
        void InsertQuestionExam(string a, long examID)
        {
            if (a.Length == 0)
            {
                return;
            }
            String[] listQuesID = a.Split(',');


            var dao = new ExamDao();


            for (int i = 0; i < listQuesID.Length; i++)
            {
                var examques = new ExamQuestion();
                examques.ExamID = examID;
                examques.QuestionID = long.Parse(listQuesID[i]);
                try
                {
                    dao.InsertExamQues(examques);
                }
                catch (Exception e)
                {

                }
            }
        }


        [HttpPost]
        [ValidateInput(false)] //Xử lí úp nội dung trong Editor của Detail
        //Hàm tạo khoá học
        //[HasCredential(RoleID = "ADD_COURSE")]
        public ActionResult CreateExam(Exam exam, long courseid, string time, string start,
            string end, string type, HttpPostedFileBase file)
        {
            var dao = new CourseDao();
            exam.DateStart = start;
            exam.DateEnd = end;
            //TachChuoiThoiGian(exam, start, end);
            exam.Status = true;
            //exam.NumberQuestion = Int32.Parse(sl.Trim());
            exam.NumberQuestion = 0;
            long examID = dao.InsertExam(exam, courseid);

            //InsertQuestionExam(listQuesSelect, examID);

            //if (file != null && file.ContentLength > 0)
            //{

            //    Stream stream = file.InputStream;

            // We return the interface, so that
            //IExcelDataReader reader = null;


            //if (upload.FileName.EndsWith(".xls"))
            //{
            //    reader = ExcelReaderFactory.CreateBinaryReader(stream);
            //}
            //else if (upload.FileName.EndsWith(".xlsx"))
            //{
            //    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //}
            //else
            //{
            //    ModelState.AddModelError("File", "This file format is not supported");
            //    return View();
            //}

            //reader.IsFirstRowAsColumnNames = true;
            //DataSet result = reader.AsDataSet();
            //reader.Close();



            //    using (var package = new ExcelPackage(file.InputStream))
            //    {
            //        //tạo exam để lấy examID
            //        var dao = new CourseDao();
            //        TachChuoiThoiGian(exam, start, end);
            //        exam.Status = true;
            //        long examID = dao.InsertExam(exam, courseid);

            //        //đọc câu hỏi, lưu vào csdl
            //        ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

            //        var noOfCol = workSheet.Dimension.End.Column;
            //        var noOfRow = workSheet.Dimension.End.Row;


            //        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            //        {
            //            var ques = new Question();
            //            ques.Ques = workSheet.Cells[rowIterator, 1].Value.ToString();
            //            ques.Option1 = workSheet.Cells[rowIterator, 2].Value.ToString();
            //            ques.Option2 = workSheet.Cells[rowIterator, 3].Value.ToString();
            //            ques.Option3 = workSheet.Cells[rowIterator, 4].Value.ToString();
            //            ques.Option4 = workSheet.Cells[rowIterator, 5].Value.ToString();
            //            ques.CorrectAnswer = workSheet.Cells[rowIterator, 6].Value.ToString();
            //            ques.CourseID = courseid;
            //            dao.InsertQues(ques);
            //        }
            //    }
            //}
            //else
            //{
            //}
            if (exam.Type.Trim().Equals("1"))
            {
                return RedirectToAction("SelectQuestion/" + examID, "Course");
            }
            return RedirectToAction("ListExams/" + exam.CourseID, "Course");

        }
        [HttpGet]
        public ActionResult EditExam(long id)
        {
            var exam = new ExamDao().ViewDetail(id);
            ViewBag.ListQues = new QuestionDao().ListQuestionIDCourse(exam.CourseID.Value);
            ViewBag.type = Int32.Parse(exam.Type.Trim());
            ViewBag.ListQuesSelect = new ExamDao().ListQuesSelectIDExam(id);
            return View(exam);
        }

        [HttpPost]
        public ActionResult EditExam(Exam exam, string time, string start,
           string end, string type, string sl, string listQuesSelect)
        {
            var dao = new ExamDao();
            exam.DateStart = start;
            exam.DateEnd = end;
            //(exam, start, end);
            exam.Status = true;
            exam.NumberQuestion = Int32.Parse(sl.Trim());
            dao.Update(exam);
            dao.DeleteQuesExam(exam.ExamID);
            InsertQuestionExam(listQuesSelect, exam.ExamID);
            return RedirectToAction("ListExams/" + exam.CourseID, "Course");
        }

        [HttpGet]
        public ActionResult CreateQuestion(long id)
        {
            ViewBag.courseid = id;
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestion(Question question, long courseid, string corectAnswer)
        {
            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();
                question.Status = true;
                question.CorrectAnswer = corectAnswer;
                question.CreateDate = DateTime.Now;
                question.Type = "1";
                long id = dao.Insert(question);
                if (id > 0)
                {
                    return RedirectToAction("ListQuestion/" + courseid, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thành công");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditQuestion(long id)
        {
            ViewBag.questionid = id;
            var question = new QuestionDao().ViewDetail(id);
            return View(question);
        }

        [HttpPost]
        public ActionResult EditQuestion(Question question, long questionid, string corectAnswer)
        {
            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();
                question.CorrectAnswer = corectAnswer;
                question.Type = "2";
                var result = dao.Update(question);
                if (result)
                {
                    return RedirectToAction("EditQuestion/" + questionid, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật câu hỏi không thành công");
                }
            }
            return View("EditQuestion");
        }

        [HttpGet]
        public ActionResult MultipleQuestion(long id)
        {
            ViewBag.courseid = id;
            return View();
        }

        [HttpPost]
        public ActionResult MultipleQuestion(Question question, long courseid, string CorrectAnswer1, string CorrectAnswer2, string CorrectAnswer3
            , string CorrectAnswer4, string CorrectAnswer5, string CorrectAnswer6, string CorrectAnswer7, string CorrectAnswer8
            , string CorrectAnswer9, string CorrectAnswer10)
        {
            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();
                question.Status = true;

                if(CorrectAnswer1.Equals("True"))
                {
                    question.CorrectAnswer1 = true;
                }
                else
                {
                    question.CorrectAnswer1 = false;
                }

                if (CorrectAnswer2.Equals("True"))
                {
                    question.CorrectAnswer2 = true;
                }
                else
                {
                    question.CorrectAnswer2 = false;
                }

                if (CorrectAnswer3.Equals("True"))
                {
                    question.CorrectAnswer3 = true;
                }
                else
                {
                    question.CorrectAnswer3 = false;
                }
                if (CorrectAnswer4.Equals("True"))
                {
                    question.CorrectAnswer4 = true;
                }
                else
                {
                    question.CorrectAnswer4 = false;
                }

                if (CorrectAnswer5.Equals("True"))
                {
                    question.CorrectAnswer5 = true;
                }
                else
                {
                    question.CorrectAnswer5 = false;
                }

                if (CorrectAnswer6.Equals("True"))
                {
                    question.CorrectAnswer6 = true;
                }
                else
                {
                    question.CorrectAnswer6 = false;
                }
                if (CorrectAnswer7.Equals("True"))
                {
                    question.CorrectAnswer7 = true;
                }
                else
                {
                    question.CorrectAnswer7 = false;
                }

                if (CorrectAnswer8.Equals("True"))
                {
                    question.CorrectAnswer8 = true;
                }
                else
                {
                    question.CorrectAnswer8 = false;
                }

                if (CorrectAnswer9.Equals("True"))
                {
                    question.CorrectAnswer9 = true;
                }
                else
                {
                    question.CorrectAnswer9 = false;
                }
                if (CorrectAnswer10.Equals("True"))
                {
                    question.CorrectAnswer10 = true;
                }
                else
                {
                    question.CorrectAnswer10 = false;
                }

                question.CreateDate = DateTime.Now;
                question.Type = "2";
                long id = dao.Insert(question);
                if (id > 0)
                {
                    return RedirectToAction("ListQuestion/" + courseid, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thành công");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditMultipleQuestion(long id)
        {
            ViewBag.questionid = id;
            var question = new QuestionDao().ViewDetail(id);
            return View(question);
        }

        [HttpPost]
        public ActionResult EditMultipleQuestion(Question question, long questionid, string CorrectAnswer1, string CorrectAnswer2, string CorrectAnswer3
            , string CorrectAnswer4, string CorrectAnswer5, string CorrectAnswer6, string CorrectAnswer7, string CorrectAnswer8
            , string CorrectAnswer9, string CorrectAnswer10)
        {
            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();
                if (CorrectAnswer1.Equals("True"))
                {
                    question.CorrectAnswer1 = true;
                }
                else
                {
                    question.CorrectAnswer1 = false;
                }

                if (CorrectAnswer2.Equals("True"))
                {
                    question.CorrectAnswer2 = true;
                }
                else
                {
                    question.CorrectAnswer2 = false;
                }

                if (CorrectAnswer3.Equals("True"))
                {
                    question.CorrectAnswer3 = true;
                }
                else
                {
                    question.CorrectAnswer3 = false;
                }
                if (CorrectAnswer4.Equals("True"))
                {
                    question.CorrectAnswer4 = true;
                }
                else
                {
                    question.CorrectAnswer4 = false;
                }

                if (CorrectAnswer5.Equals("True"))
                {
                    question.CorrectAnswer5 = true;
                }
                else
                {
                    question.CorrectAnswer5 = false;
                }

                if (CorrectAnswer6.Equals("True"))
                {
                    question.CorrectAnswer6 = true;
                }
                else
                {
                    question.CorrectAnswer6 = false;
                }
                if (CorrectAnswer7.Equals("True"))
                {
                    question.CorrectAnswer7 = true;
                }
                else
                {
                    question.CorrectAnswer7 = false;
                }

                if (CorrectAnswer8.Equals("True"))
                {
                    question.CorrectAnswer8 = true;
                }
                else
                {
                    question.CorrectAnswer8 = false;
                }

                if (CorrectAnswer9.Equals("True"))
                {
                    question.CorrectAnswer9 = true;
                }
                else
                {
                    question.CorrectAnswer9 = false;
                }
                if (CorrectAnswer10.Equals("True"))
                {
                    question.CorrectAnswer10 = true;
                }
                else
                {
                    question.CorrectAnswer10 = false;
                }
                question.Type = "2";
                var result = dao.Update(question);
                if (result)
                {
                    return RedirectToAction("EditMultipleQuestion/" + questionid, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật câu hỏi không thành công");
                }
            }
            return View("EditMultipleQuestion");
        }

        [HttpGet]
        public ActionResult TrueFalseQuestion(long id)
        {
            ViewBag.courseid = id;
            return View();
        }

        [HttpPost]
        public ActionResult TrueFalseQuestion(Question question, long courseid, string corectAnswer)
        {
            //Xử lý Insert dữ liệu vào Database
            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();
                question.Status = true;
                question.CorrectAnswer = corectAnswer;
                question.CreateDate = DateTime.Now;
                question.Type = "3";
                long id = dao.Insert(question);
                if (id > 0)
                {
                    return RedirectToAction("ListQuestion/" + courseid, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thành công");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditTrueFalseQuestion(long id)
        {
            ViewBag.questionid = id;
            var question = new QuestionDao().ViewDetail(id);
            return View(question);
        }

        [HttpPost]
        public ActionResult EditTrueFalseQuestion(Question question, long questionid, string corectAnswer)
        {
            if (ModelState.IsValid)
            {
                var dao = new QuestionDao();
                question.CorrectAnswer = corectAnswer;
                question.Type = "3";
                var result = dao.Update(question);
                if (result)
                {
                    return RedirectToAction("EditTrueFalseQuestion/" + questionid, "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật câu hỏi không thành công");
                }
            }
            return View("EditTrueFalseQuestion");
        }

        [HttpGet]
        public ActionResult Preview(long id)
        {
            var exam = new ExamDao().ViewDetail(id);
            ViewBag.ListQues = new QuestionDao().ListQuestionIDCourse(exam.CourseID.Value);
            ViewBag.ListQuesSelect = new ExamDao().ListQuesSelectIDExam(id);
            return View(exam);
        }


        [HttpDelete]
        public ActionResult DeleteExam(long id)
        {
            new ExamDao().DeleteExam(id);
            return RedirectToAction("ListExams");
        }

        [HttpPost]
        public JsonResult ChangeStatusExam(long id)
        {
            var result = new ExamDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpGet]
        public ActionResult SelectQuestion(long id)
        {
            var exam = new ExamDao().ViewDetail(id);
            ViewBag.ListQues = new QuestionDao().ListQuestionIDCourse(exam.CourseID.Value);
            ViewBag.examid = exam.ExamID;
            return View();
        }

        [HttpPost]
        public ActionResult SelectQuestion(ListQuestion question, string listQuesSelect)
        {
            //InsertQuestionExam(listQuesSelect, int.Parse(examid));
            var dao = new ExamDao();
            foreach (var item in question.ListQuestionnn)
            {
                if (item.QuestionID == 0)
                {
                    continue;
                }
                var examques = new ExamQuestion();
                examques.ExamID = item.ExamID;
                examques.QuestionID = item.QuestionID;
                try
                {
                    dao.InsertExamQues(examques);
                }
                catch (Exception e)
                {

                }
            }
            return RedirectToAction("Preview/" + question.ListQuestionnn[0].ExamID, "Course");
        }

        [HttpGet]
        public ActionResult ResultFile(long id, string searchString, string listScore, int page = 1, int pageSize = 10)
        {
            var dao = new CourseDao();
            var exam = new ExamDao().ViewDetail(id);

            var ds = dao.ListCourseUserCourseScore(exam.CourseID.Value, exam.ExamID);
            ViewBag.listScoreUser = new ExamDao().getListScore(id);

            ViewBag.SearchString = searchString; //Search cho nhiều trang
            ViewBag.SubmitFile = new SelectFileDao().ListAllFile(id);

            return View(ds);
        }

        [HttpPost]
        public ActionResult ResultFile(List<ScoreViewModel> sc)
        {
            foreach (var item in sc)
            {
                var score = new Score();
                score.UserID = item.UserID;
                score.CourseID = item.CourseID;
                score.ExamID = item.ExamID;
                score.Score1 = item.ScoreOfExam.ToString().Trim();
                score.Date = DateTime.Now;

                long scoreID = new ExamDao().ReInsertScore(score);
            }
            return RedirectToAction("ResultFile/" + sc[0].ExamID, "Course");
        }

        [HttpGet]
        public ActionResult Result(long id, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CourseDao();
            var exam = new ExamDao().ViewDetail(id);
            var model = dao.ListCourseUserCourse(exam.CourseID.Value, searchString, page, pageSize);

            ViewBag.SearchString = searchString; //Search cho nhiều trang
            ViewBag.ResultScore = new ScoreDao().ListAllScore(id);

            return View(model);
        }

        [HttpGet]
        public ActionResult TypeQuestion(long id)
        {
            ViewBag.courseid = id;
            return View();
        }

        [HttpPost]
        public ActionResult TypeQuestion()
        {
            return View();
        }

    }
}