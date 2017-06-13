using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CourseTagDao
    {
        ElearningDbContext db = null;
        public CourseTagDao()
        {
            db = new ElearningDbContext();
        }

        //Insert dữ liệu vào bảng CourseTag
        public long Insert(CourseTag entity)
        {
            db.CourseTags.Add(entity);
            db.SaveChanges();
            return entity.CourseID;
        }

        public bool Remove(long courseID)
        {
            try
            {
                var entity = db.CourseTags.Where(x => x.CourseID == courseID).ToList();
                db.CourseTags.RemoveRange(entity);
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {

            }
            return false;
        }
    }
}
