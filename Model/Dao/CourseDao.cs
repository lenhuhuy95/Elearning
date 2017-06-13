using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ViewModel;

namespace Model.Dao
{
    public class CourseDao
    {
        ElearningDbContext db = null;
        public CourseDao()
        {
            db = new ElearningDbContext();
        }

        //Insert dữ liệu vào bảng Course
        public long Insert(Course entity)
        {
            db.Courses.Add(entity);
            db.SaveChanges();
            return entity.CourseID;
        }

        //Insert CoureID và UserId vào bảng User_Course
        public long InsertUserCourse(User_Course entity)
        {
            try
            {
                db.User_Course.Add(entity);
                db.SaveChanges();
                return entity.CourseID;
            }
            catch (Exception e)
            {

            }
            return -1;
        }

        //Check tài khoản đã đăng ký chưa
        public bool CheckAccountUser(long courseId, long userId)
        {
            //var userCourse = db.User_Course.Find(courseId, userId);
            //var userCourse = db.User_Course.Where(x => x.UserID == userId && x.CourseID == courseId).Select(x => new User_Course { ID = x.ID, UserID = x.UserID, CourseID = x.CourseID });
            var userCourse = db.User_Course.SingleOrDefault(x => x.UserID == userId && x.CourseID == courseId && x.Status == true);
            if (userCourse != null)
            {
                return true;//có đk khóa học
            }
            return false;//không dk khóa học
        }

        //update khoá học
        public bool Update(Course entity)
        {
            try
            {
                var course = db.Courses.Find(entity.CourseID);
                course.CourseName = entity.CourseName;
                course.Description = entity.Description;
                course.Image = entity.Image;
                course.Video = entity.Video;
                course.Detail = entity.Detail;
                course.Cost = entity.Cost;
                course.PromotionCost = entity.PromotionCost;
                course.Percent = entity.Percent;
                course.CategoryID = entity.CategoryID;
                course.Tag = entity.Tag;
                course.CourseCategory = entity.CourseCategory;
                course.NumberSection = entity.NumberSection;
                course.Instructed = entity.Instructed;
                course.MoneyID = entity.MoneyID;
                course.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //xử lý tìm id để sửa category
        public Course GetByID(long id)
        {
            return db.Courses.Find(id);
        }

        //Lấy danh sách ListNewCourse giảm dần (OrderByDescending) theo CreateDate, Take (top) để lấy bao nhiêu cái
        public List<Course> ListNewCourse(int top)
        {
            return db.Courses.Where(x => x.Status == true).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        /// <summary>
        /// Lấy ra danh sách khóa học khi click vào các chuyên mục và phân trang trên client
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<Course> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Courses.Where(x => x.CategoryID == categoryID).Count();
            var model = db.Courses.Where(x => x.CategoryID == categoryID && x.Status == true).OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return model;
        }

        //Lấy danh sách ListTopCourse với đk TopHot != null nhưng còn hạn giảm dần (OrderByDescending) theo CreateDate, Take (top) để lấy bao nhiêu cái
        public List<Course> ListTopCourse(int top)
        {
            return db.Courses.Where(x => x.Status == true).OrderBy(x => x.CreateDate).Take(top).ToList();
        }

        //Lấy danh sách khóa học liên quan ListInvolveCourse, khoá học phải khác với khoá học truyền vào, cùng danh mục CategoryID và củng TagID
        public List<RelatedCours> ListInvolveCourse(long RelatedId)
        {
            var course = db.RelatedCourses.Where(x => x.CourseID == RelatedId).OrderBy(x => x.CourseID).ToList();
            return course;
        }

        //Lấy danh sách khóa học liên quan cho lecture
        public List<Course> ListInvolveLecture(long CourseId)
        {
            var course = db.Courses.Find(CourseId);
            return db.Courses.Where(x => x.CourseID != CourseId && x.CategoryID == course.CategoryID && x.Status == true).OrderByDescending(x => x.CourseID).ToList();
        }

        //Truyền id và tìm đúng theo chi tiết khoá học
        public Course ViewDetail(long id)
        {
            return db.Courses.Find(id);
        }

        //lấy danh sách Courses
        public IEnumerable<Course> ListAllPaging(string searchString, int page, int pageSize, long id, string groupID)
        {
            //var list = new AdminGroupDao().ListRoleSelected(groupID);
            IQueryable<Course> model = db.Courses;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (groupID == "ADMIN" || groupID == "MOD") //admin với mod được xem toàn bộ ds course
                {
                    model = model.Where(x => x.CourseName.Contains(searchString));
                }
                else //khác admin và mod thì chỉ được xem course theo id instructed tương ứng
                {
                    model = model.Where(x => x.CourseName.Contains(searchString) && x.Instructed == id);
                }
            }
            else
            {
                if (groupID == "ADMIN" || groupID == "MOD") //admin với mod được xem toàn bộ ds
                {
                }
                else//khác admin và mod thì chỉ được xem course theo id instructed tương ứng
                {
                    model = model.Where(x => x.Instructed == id);
                }

            }

            return model.OrderByDescending(x => x.CourseID).ToPagedList(page, pageSize);
        }

        //thay đổi trạng thái của khóa học
        public bool ChangeStatus(long id)
        {
            var course = db.Courses.Find(id);
            course.Status = !course.Status;
            db.SaveChanges();
            return course.Status.Value;
        }

        //xóa khóa học
        public bool DeleteCourse(long id)
        {
            try
            {
                //xóa lecture
                var lecture = db.Lectures.Where(x => x.CourseID == id).ToList();
                db.Lectures.RemoveRange(lecture);
                //xóa section
                var section = db.Sections.Where(x => x.CourseID == id).ToList();
                db.Sections.RemoveRange(section);
                //xóa khóa học
                var course = db.Courses.Find(id);
                db.Courses.Remove(course);

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //kết bảng hiện thị được Category đã được chọn khi CreateCourse
        public List<CourseCategoryView> ListSelectCourseCategory(long id)
        {
            var list = (from a in db.Courses
                        join b in db.CourseCategories
                        on a.CategoryID equals b.CategoryID
                        where a.CourseID == id
                        join c in db.CourseCategories
                        on b.ParentID equals c.CategoryID
                        select new CourseCategoryView()
                        {
                            CategoryID = a.CategoryID,
                            CourseID = a.CourseID,
                            CourseName = a.CourseName,
                            CategoryName = b.CategoryName,
                            CategoryParentName = c.CategoryName
                        }).ToList();
            return list;
        }


        //kết bảng show những tag đã được chọn phục vụ cho edit course
        public List<CourseTag_Tag> ListTagSelected(long id)
        {
            var list = (from a in db.CourseTags
                        join b in db.Tags
                        on a.TagID equals b.TagID
                        where a.CourseID == id
                        select new CourseTag_Tag()
                        {
                            TagID = a.TagID,
                            CourseID = a.CourseID,
                            TagName = b.TagName

                        }).ToList();
            return list;
        }


        //
        public IEnumerable<Course> ListCourseIntructor(long id, string searchString, int page, int pageSize)
        {
            IQueryable<Course> model = db.Courses;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Instructed == id && x.CourseName.Contains(searchString));
            }
            else
            {
                model = model.Where(x => x.Instructed == id);
            }

            return model.OrderBy(x => x.CourseID).ToPagedList(page, pageSize);
        }

        //Lấy danh sách Course theo Instructed
        public List<Course> ListCourseIntructor2(long id)
        {
            return db.Courses.Where(x => x.Instructed == id).OrderBy(x => x.CourseID).ToList();
        }

        //lấy ds các môn học hiện có
        public List<Course> ListAllCourse()
        {
            return db.Courses.OrderByDescending(x => x.CourseID).ToList();
        }

        public List<RelatedCours> RelatedCoursesSelect(long id)
        {
            return db.RelatedCourses.Where(x => x.CourseID == id).OrderBy(x => x.RelatedID).ToList();
        }

        public bool RemoveRelatedCourses(long courseID)
        {
            try
            {
                var entity = db.RelatedCourses.Where(x => x.CourseID == courseID).ToList();
                db.RelatedCourses.RemoveRange(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

            }
            return false;
        }

        //danh sách học viên của khóa học
        //public IEnumerable<User_Course> ListCourseUserCourse(long id, string searchString, int page, int pageSize)
        //{
        //    IQueryable<User_Course> model = db.User_Course;
        //    model = model.Where(x => x.CourseID == id);
        //    return model.OrderBy(x => x.CourseID).ToPagedList(page, pageSize);
        //}

        //trả ds học viên của khóa học
        public IEnumerable<UserCourse_User> ListCourseUserCourse(long courseid, string searchString, int page, int pageSize)
        {


            var ds = (from a in db.User_Course
                      join b in db.Users
                      on a.UserID equals b.UserID
                      where a.CourseID == courseid
                      select new UserCourse_User()
                      {
                          ID = a.ID,
                          UserID = a.UserID,
                          CourseID = a.CourseID,
                          Status = a.Status.Value,
                          DateJoin = a.DateJoin,
                          Name = b.Name,
                          Email = b.Email,
                          Phone = b.Phone,
                      }).ToList();

            return ds.ToPagedList(page, pageSize);
        }

        //trả ds học viên của khóa học keets bangr
        public List<ScoreViewModel> ListCourseUserCourseScore(long courseid, long examID)
        {


            var ds = (from a in db.User_Course
                      join b in db.Users
                      on a.UserID equals b.UserID
                      where a.CourseID == courseid
                      select new ScoreViewModel()
                      {
                          ID = a.ID,
                          UserID = a.UserID,
                          CourseID = a.CourseID,
                          Status = a.Status.Value,
                          //DateJoin = a.DateJoin,
                          Name = b.Name,
                          Email = b.Email,
                          Phone = b.Phone,
                          NameFile = "",
                          ExamID = examID
                      }).ToList();

             return ds;
        }

        //thay đổi trạng thái của học viên
        public bool ChangeStatusUserCourse(long id)
        {
            var usercourse = db.User_Course.Find(id);
            usercourse.Status = !usercourse.Status;
            db.SaveChanges();
            return usercourse.Status.Value;
        }
        //xóa hocj vieen
        public bool DeleteUserCourse(long id)
        {
            try
            {
                var usercourse = db.User_Course.Find(id);
                db.User_Course.Remove(usercourse);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Insert dữ liệu vào bảng User_Course
        public long InsertUserCourse1(User_Course entity)
        {
            try
            {
                db.User_Course.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (Exception)
            {
                return -1;
            }

        }

        //
        public IEnumerable<Exam> ListAllExam(string searchString, int page, int pageSize, long id, string groupID, long courseID)
        {
            //var list = new AdminGroupDao().ListRoleSelected(groupID);
            IQueryable<Exam> model = db.Exams;
            model = model.Where(x => x.CourseID == courseID);
            return model.OrderByDescending(x => x.ExamID).ToPagedList(page, pageSize);
        }

        //
        public IEnumerable<Question> ListAllQuestion(string searchString, int page, int pageSize, long id, long courseID)
        {
            //var list = new AdminGroupDao().ListRoleSelected(groupID);
            IQueryable<Question> model = db.Questions;

            model = model.Where(x => x.CourseID == courseID);
            return model.OrderByDescending(x => x.QuestionID).ToPagedList(page, pageSize);
        }

        //Insert dữ liệu vào bảng Exam
        public long InsertExam(Exam entity, long courseid)
        {
            db.Exams.Add(entity);
            db.SaveChanges();
            return entity.ExamID;
        }

        //Insert dữ liệu vào bảng Question
        public long InsertQues(Question entity)
        {
            db.Questions.Add(entity);
            db.SaveChanges();
            return entity.QuestionID;
        }

    }
}


