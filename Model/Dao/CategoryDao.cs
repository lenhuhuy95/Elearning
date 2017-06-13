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
    public class CategoryDao
    {
        ElearningDbContext db = null;
        public CategoryDao()
        {
            db = new ElearningDbContext();
        }

        //Insert dữ liệu vào bảng CourseCategory
        public long Insert(CourseCategory entity)
        {
            db.CourseCategories.Add(entity);
            db.SaveChanges();
            return entity.CategoryID;
        }

        //Lấy danh sách Category có Status bằng True
        public List<CourseCategory> ListAll()
        {
            return db.CourseCategories.Where(x => x.Status == true).ToList();
        }

        //Lấy danh sách Category có ParentID bằng Null
        public List<CourseCategory> ListParentAll()
        {
            return db.CourseCategories.Where(x => x.ParentID == null).OrderBy(x => x.DisplayOrder).ToList();
        }

        //lấy ds Category show vào table quản lý
        //public IEnumerable<CourseCategory> ListAllPaging(string searchString, int page, int pageSize)
        //{
        //    IQueryable<CourseCategory> model = db.CourseCategories;
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        model = model.Where(x => x.CategoryName.Contains(searchString) || x.MetaTitle.Contains(searchString));
        //    }

        //    return model.OrderByDescending(x => x.CategoryID).ToPagedList(page, pageSize);
        //}

        //Trả list cha,con chuyên mục
        public IEnumerable<CategoryView> ListAllCategory(string searchString, int page, int pageSize)
        {


            var cha = (from a in db.CourseCategories
                       join b in db.CourseCategories
                       on a.CategoryID equals b.CategoryID
                       where a.ParentID == null
                       select new CategoryView()
                       {
                           CategoryNameParent = a.CategoryName,
                           CategoryID = a.CategoryID,
                           CategoryName = a.CategoryName,
                           MetaTitle = a.MetaTitle,
                           ParentID = a.ParentID,
                           DisplayOrder = a.DisplayOrder,
                           SeoTitle = a.SeoTitle,
                           CreateDate = a.CreateDate,
                           CreateBy = a.CreateBy,
                           ModifiedDate = a.ModifiedDate,
                           ModifiedBy = a.ModifiedBy,
                           Status = a.Status
                       }).ToList();


            var ds = new List<CategoryView> { };


            if (!string.IsNullOrEmpty(searchString))
            {

                foreach (var item in cha)
                {

                    if (item.CategoryName.Contains(searchString) || item.MetaTitle.Contains(searchString))
                    {
                        ds.Add(item);
                    }

                    var dstemp = (from a in db.CourseCategories
                                  join b in db.CourseCategories on a.ParentID equals b.CategoryID
                                  select new CategoryView()
                                  {
                                      CategoryNameParent = b.CategoryName,
                                      CategoryID = a.CategoryID,
                                      CategoryName = a.CategoryName,
                                      MetaTitle = a.MetaTitle,
                                      ParentID = a.ParentID,
                                      DisplayOrder = a.DisplayOrder,
                                      SeoTitle = a.SeoTitle,
                                      CreateDate = a.CreateDate,
                                      CreateBy = a.CreateBy,
                                      ModifiedDate = a.ModifiedDate,
                                      ModifiedBy = a.ModifiedBy,
                                      Status = a.Status
                                  }).Where(x => x.ParentID == item.CategoryID && (x.CategoryName.Contains(searchString) || x.MetaTitle.Contains(searchString))).ToList();
                    foreach (var item2 in dstemp)
                    {
                        ds.Add(item2);
                    }
                }
            }
            else
            {
                foreach (var item in cha)
                {
                    ds.Add(item);
                    var dstemp = (from a in db.CourseCategories
                                  join b in db.CourseCategories on a.ParentID equals b.CategoryID
                                  select new CategoryView()
                                  {
                                      CategoryNameParent = b.CategoryName,
                                      CategoryID = a.CategoryID,
                                      CategoryName = a.CategoryName,
                                      MetaTitle = a.MetaTitle,
                                      ParentID = a.ParentID,
                                      DisplayOrder = a.DisplayOrder,
                                      SeoTitle = a.SeoTitle,
                                      CreateDate = a.CreateDate,
                                      CreateBy = a.CreateBy,
                                      ModifiedDate = a.ModifiedDate,
                                      ModifiedBy = a.ModifiedBy,
                                      Status = a.Status
                                  }).Where(x => x.ParentID == item.CategoryID).ToList();
                    foreach (var item2 in dstemp)
                    {
                        ds.Add(item2);
                    }
                }
            }

            return ds.ToPagedList(page, pageSize);
        }

        //Trả list cha,con chuyên mục
        public List<CategoryView> List()
        {
            var cha = (from a in db.CourseCategories
                       join b in db.CourseCategories
                       on a.CategoryID equals b.CategoryID
                       where a.ParentID == null
                       select new CategoryView()
                       {
                           CategoryNameParent = a.CategoryName,
                           CategoryID = a.CategoryID,
                           CategoryName = a.CategoryName,
                           MetaTitle = a.MetaTitle,
                           ParentID = a.ParentID,
                           DisplayOrder = a.DisplayOrder,
                           SeoTitle = a.SeoTitle,
                           CreateDate = a.CreateDate,
                           CreateBy = a.CreateBy,
                           ModifiedDate = a.ModifiedDate,
                           ModifiedBy = a.ModifiedBy,
                           Status = a.Status
                       }).ToList();


            var ds = new List<CategoryView> { };

            foreach (var item in cha)
            {
                ds.Add(item);
                var dstemp = (from a in db.CourseCategories
                              join b in db.CourseCategories on a.ParentID equals b.CategoryID
                              select new CategoryView()
                              {
                                  CategoryNameParent = b.CategoryName,
                                  CategoryID = a.CategoryID,
                                  CategoryName = a.CategoryName,
                                  MetaTitle = a.MetaTitle,
                                  ParentID = a.ParentID,
                                  DisplayOrder = a.DisplayOrder,
                                  SeoTitle = a.SeoTitle,
                                  CreateDate = a.CreateDate,
                                  CreateBy = a.CreateBy,
                                  ModifiedDate = a.ModifiedDate,
                                  ModifiedBy = a.ModifiedBy,
                                  Status = a.Status
                              }).Where(x => x.ParentID == item.CategoryID).ToList();
                foreach (var item2 in dstemp)
                {
                    ds.Add(item2);
                }
            }
            return ds.ToList();
        }

        //thay đổi trạng thái của category
        public bool ChangeStatus(long id)
        {
            var category = db.CourseCategories.Find(id);
            category.Status = !category.Status;
            db.SaveChanges();
            return category.Status.Value;
        }

        //Lấy Category có  ParentID = null
        public List<CourseCategory> Categoryparent()
        {
            return db.CourseCategories.Where(x => x.ParentID == null).OrderBy(x => x.CategoryID).ToList();
        }

        //Lấy Category có  ParentID = null
        public List<CourseCategory> EditCategoryparent()
        {
            return db.CourseCategories.Where(x => x.ParentID == null).OrderBy(x => x.CategoryID).ToList();
        }
    }
}
