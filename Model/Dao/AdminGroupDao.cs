using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AdminGroupDao
    {
        ElearningDbContext db = null;
        public AdminGroupDao()
        {
            db = new ElearningDbContext();
        }

        //Insert
        public string Insert(AdminGroup entity)
        {
            db.AdminGroups.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        //lấy danh sách và tìm kiếm admin
        public IEnumerable<AdminGroup> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<AdminGroup> model = db.AdminGroups;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ID.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        //DeleteAdminGroup
        public bool DeleteAdminGroup(string id)
        {
            try
            {
                var group = db.AdminGroups.Find(id);
                db.AdminGroups.Remove(group);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //
        public AdminGroup Detail(string id)
        {
            return db.AdminGroups.Find(id);
        }

        //
        public List<Role> ListRole()
        {
            return db.Roles.OrderBy(x => x.ID).ToList();
        }

        //
        public List<Credential> ListRoleSelected(string id)
        {
            return db.Credentials.Where(x => x.UserGroupID == id).OrderBy(x=>x.RoleID).ToList();
        }


        //
        public bool Update(AdminGroup entity)
        {
            try
            {
                var gr = db.AdminGroups.Find(entity.ID);
                gr.Name = entity.Name;
                gr.Description = entity.Description;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //logging
                return false;
            }
        }

        //InsertCredential
        public bool InsertCredential(Credential cr)
        {
            try
            {
                db.Credentials.Add(cr);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //RemoveCredential
        public bool RemoveCredential(string id)
        {
            try
            {
                var entity = db.Credentials.Where(x => x.UserGroupID == id).ToList();
                db.Credentials.RemoveRange(entity);
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
