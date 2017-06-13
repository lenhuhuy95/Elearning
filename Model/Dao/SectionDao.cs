using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class SectionDao
    {
        ElearningDbContext db = null;

        public SectionDao()
        {
            db = new ElearningDbContext();
        }

        //lấy ds các section có mã CourseID là id
        public List<Section> ViewSectionByCourseID(long id)
        {
            return db.Sections.Where(x => x.CourseID == id).ToList();
        }

        //lấy số lượng section có cùng CourseID
        public int getNum(long courseid)
        {
            return db.Sections.Where(x => x.CourseID == courseid).Count();
        }

        //trả về ds các section tìm theo mã courseID
        public IEnumerable<Section> ListAllPaging(string searchString, int page, int pageSize, long courseID)
        {
            IQueryable<Section> model = db.Sections;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CourseID == courseID && x.SectionName.Contains(searchString));
            }
            else
            {
                model = model.Where(x => x.CourseID == courseID);
            }

            return model.OrderBy(x => x.SectionID).ToPagedList(page, pageSize);
            //return db.Sections.Where(x => x.CourseID == courseID).OrderBy(x => x.SectionID).ToPagedList(page, pageSize);
        }

        //trả về ds các section tìm theo mã courseID cho trang client
        public IEnumerable<Section> ListAllPaging1(int page, int pageSize, long courseID)
        {
            return db.Sections.Where(x => x.CourseID == courseID).OrderBy(x => x.SectionID).ToPagedList(page, pageSize);
        }

        //Truyền id và tìm đúng theo section
        public Section ViewDetail(long id)
        {
            return db.Sections.Find(id);
        }

        //Insert dữ liệu vào bảng Section
        public long Insert(Section entity)
        {
            db.Sections.Add(entity);
            db.SaveChanges();
            return entity.CourseID.Value;
        }

        //update Section
        public bool Update(Section entity)
        {
            try
            {
                var section = db.Sections.Find(entity.SectionID);
                section.SectionName = entity.SectionName;
                section.Description = entity.Description;
                section.NumberLecture = entity.NumberLecture;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //thay đổi trạng thái của Section
        public bool ChangeStatus(long id)
        {
            var section = db.Sections.Find(id);
            section.Status = !section.Status;
            db.SaveChanges();
            return section.Status.Value;
        }

        //xóa khóa học
        public bool DeleteSection(long id)
        {
            try
            {
                //xóa lecture
                var lecture = db.Lectures.Where(x => x.SectionID == id).ToList();
                db.Lectures.RemoveRange(lecture);
                //xóa section
                var section = db.Sections.Find(id);
                db.Sections.Remove(section);

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
