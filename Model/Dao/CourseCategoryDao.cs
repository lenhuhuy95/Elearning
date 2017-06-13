using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CourseCategoryDao
    {
        ElearningDbContext db = null;
        public CourseCategoryDao()
        {
            db = new ElearningDbContext();
        }

        //Lấy danh sách CourseCategory có Status bằng True, DisplayOrder # 0 để trừ Chuyên mục gốc và tăng dần theo DisplayOrder.
        public List<CourseCategory> ListAll()
        {
            return db.CourseCategories.Where(x => x.Status == true && x.DisplayOrder != 0).OrderBy(x => x.DisplayOrder).ToList();
        }

        //Truyền id và tìm đúng theo danh mục khoá học
        public CourseCategory ViewDetail(long id)
        {
            return db.CourseCategories.Find(id);
        }

        //xóa category
        public bool DeleteCategory(long id)
        {
            try
            {
                var category = db.CourseCategories.Find(id);
                db.CourseCategories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //update category
        public bool Update(CourseCategory entity)
        {
            try
            {
                var category = db.CourseCategories.Find(entity.CategoryID);
                category.CategoryName = entity.CategoryName;
                category.MetaTitle = entity.MetaTitle;
                category.CategoryID = entity.CategoryID;
                category.ModifiedDate = DateTime.Now;
                category.ParentID = entity.ParentID;
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
