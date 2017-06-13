using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class RelatedCoursDao
    {
        ElearningDbContext db = null;
        public RelatedCoursDao()
        {
            db = new ElearningDbContext();
        }

        public long Insert(RelatedCours entity)
        {
            db.RelatedCourses.Add(entity);
            db.SaveChanges();
            return entity.CourseID;
        }
    }
}
