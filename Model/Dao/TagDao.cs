using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class TagDao
    {
        ElearningDbContext db = null;
        public TagDao()
        {
            db = new ElearningDbContext();
        }

        //Insert dữ liệu vào bảng Tag
        public long Insert(Tag entity)
        {
            db.Tags.Add(entity);
            db.SaveChanges();
            return entity.TagID;
        }

        //update Tag
        public bool Update(Tag entity)
        {
            try
            {
                var tag = db.Tags.Find(entity.TagID);
                tag.TagName = entity.TagName;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //lấy ds tag và search
        public IEnumerable<Tag> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Tag> model = db.Tags;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TagName.Contains(searchString));
            }

            return model.OrderBy(x => x.TagID).ToPagedList(page, pageSize);
        }

        //Truyền id và tìm đúng theo tagid
        public Tag ViewDetail(long id)
        {
            return db.Tags.Find(id);
        }

        //Lấy danh sách Tag tăng dần theo TagID
        public List<Tag> ListAll()
        {
            return db.Tags.OrderBy(x=>x.TagID).ToList();
        }

        //xóa Tag
        public bool DeleteTag(long id)
        {
            try
            {
                var tag = db.Tags.Find(id);
                db.Tags.Remove(tag);
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
