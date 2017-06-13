using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class LectureDao
    {
        ElearningDbContext db = null;

        public LectureDao()
        {
            db = new ElearningDbContext();
        }

        //Insert dữ liệu vào bảng Lecture
        public long Insert(Lecture entity)
        {
            db.Lectures.Add(entity);
            db.SaveChanges();
            return entity.LectureID;
        }

        //lấy số lượng lecture có cùng SectionID
        public int getNum(long sectionid)
        {
            return db.Lectures.Where(x => x.SectionID == sectionid).Count();
        }

        //Truyền id và tìm đúng theo lecture
        public Lecture ViewDetail(long id)
        {
            return db.Lectures.Find(id);
        }

        public long ViewCourseID(long idsection)
        {
            
            Lecture lecture = db.Lectures.FirstOrDefault(x => x.SectionID == idsection);
            if(lecture!=null)
            {
                return lecture.CourseID.Value;
            }
            return -1;
        }

        //trả về ds các Lecture tìm theo mã SectionID
        public IEnumerable<Lecture> ListAllPaging(string searchString, int page, int pageSize, long sectionID)
        {
            IQueryable<Lecture> model = db.Lectures;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.SectionID == sectionID && x.LectureName.Contains(searchString));
            }
            else
            {
                model = model.Where(x => x.SectionID == sectionID);
            }

            return model.OrderBy(x => x.LectureID).ToPagedList(page, pageSize);
            //return db.Lectures.Where(x => x.SectionID == sectionID).OrderBy(x => x.LectureID).ToPagedList(page, pageSize);
        }

        //trả về ds các Lecture tìm theo mã SectionID cho trang client
        public IEnumerable<Lecture> ListAllPaging1(int page, int pageSize, long sectionID)
        {
            return db.Lectures.Where(x => x.SectionID == sectionID).OrderBy(x => x.LectureID).ToPagedList(page, pageSize);
        }

        //thay đổi trạng thái của Lecture
        public bool ChangeStatus(long id)
        {
            var lecture = db.Lectures.Find(id);
            lecture.Status = !lecture.Status;
            db.SaveChanges();
            return lecture.Status.Value;
        }

        //xóa bài học
        public bool DeleteLecture(long id)
        {
            try
            {
                var lecture = db.Lectures.Find(id);
                db.Lectures.Remove(lecture);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //update Lecture
        public bool Update(Lecture entity)
        {
            try
            {
                var lecture = db.Lectures.Find(entity.LectureID);
                lecture.LectureName = entity.LectureName;
                lecture.Description = entity.Description;
                lecture.Detail = entity.Detail;
                lecture.Image = entity.Image;
                lecture.Video = entity.Video;
                lecture.Fileattached = entity.Fileattached;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
