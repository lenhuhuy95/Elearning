using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SalaryDao
    {
        ElearningDbContext db = null;
        public SalaryDao()
        {
            db = new ElearningDbContext();
        }

        public IEnumerable<Salary> History(long id, string searchString, int page, int pageSize)
        {
            IQueryable<Salary> model = db.Salaries;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CourseID == id);
            }
            else
            {
                model = model.Where(x => x.CourseID == id);
            }

            return model.OrderBy(x => x.CourseID).ToPagedList(page, pageSize);
        }

        public int CountUserCourseLast(long id)
        {
            var list = db.Salaries.Where(x => x.CourseID == id).OrderBy(x => x.SalaryID).ToList();
            int total = 0;
            foreach (var item in list)
            {
                total = total + (int)item.CountUser;
            }
            return total;
        }

        //Insert dữ liệu vào bảng Salary
        public long Insert(Salary entity)
        {
            db.Salaries.Add(entity);
            db.SaveChanges();
            return entity.SalaryID;
        }
    }
}