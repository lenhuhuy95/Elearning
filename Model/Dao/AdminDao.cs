using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Model.Dao
{
    public class AdminDao
    {
        ElearningDbContext db = null;
        public AdminDao()
        {
            db = new ElearningDbContext();
        }

        public Admin GetById(string userAdminName)
        {
            return db.Admins.SingleOrDefault(x => x.UserAdmin == userAdminName);
        }

        //trả về user theo id
        public Admin ViewDetail(long id)
        {
            return db.Admins.Find(id);
        }

        public int Login(string userAdminName, string passWord)
        {
            var result = db.Admins.SingleOrDefault(x => x.UserAdmin == userAdminName);
            if (result == null)
            {
                return 0; // tk ko ton tai
            }
            else
            {
                if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP || result.GroupID == CommonConstants.TEACHER_GROUP) //được phân quyền
                {
                    if (result.Status == false)
                    {
                        return -1; //tai khoan bi khoa
                    }
                    else
                    {
                        if (result.PassWord == passWord)

                            return 1;
                        else
                            return -2; //sai pass
                    }
                }
                else
                {
                    return -3; //không có quyền
                }
            }
        }

        //lấy danh sách và tìm kiếm admin
        public IEnumerable<Admin> ListAllPaging(string searchString, int page, int pageSize, long ID, string groupID)
        {
            IQueryable<Admin> model = db.Admins;
            if (!string.IsNullOrEmpty(searchString))
            {
                if(groupID =="ADMIN" || groupID == "MOD")
                {
                    model = model.Where(x => x.UserAdmin.Contains(searchString));
                }
                else
                {
                    model = model.Where(x => x.UserAdmin.Contains(searchString) && x.AdminID == ID);
                }
            }
            else
            {
                if (groupID == "ADMIN" || groupID == "MOD")
                {
                 
                }
                else
                {
                    model = model.Where(x => x.AdminID == ID);
                }
            }

            return model.OrderBy(x => x.AdminID).ToPagedList(page, pageSize);
        }

        public List<Admin> List()
        {
            return db.Admins.OrderBy(x => x.AdminID).ToList();
        }

        public List<AdminGroup> ListGroupID()
        {
            return db.AdminGroups.OrderBy(x => x.ID).ToList();
        }

        public long Insert(Admin entity)
        {
            db.Admins.Add(entity);
            db.SaveChanges();
            return entity.AdminID;
        }

        //update userAdmin
        public bool Update(Admin entity)
        {
            try
            {
                var admin = db.Admins.Find(entity.AdminID);
                admin.UserAdmin = entity.UserAdmin;
                if (!string.IsNullOrEmpty(entity.PassWord))
                {
                    admin.PassWord = entity.PassWord;
                }
                admin.NameAdmin = entity.NameAdmin;
                admin.Description = entity.Description;
                admin.Image = entity.Image;
                admin.ModifiedDate = DateTime.Now;
                admin.GroupID = entity.GroupID;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //logging
                return false;
            }
        }

        //thay đổi status admin
        public bool ChangeStatus(long id)
        {
            var admin = db.Admins.Find(id);
            admin.Status = !admin.Status;
            db.SaveChanges();
            return admin.Status.Value;
        }

        //xóa admin
        public bool DeleteAdmin(long id)
        {
            try
            {
                var admin = db.Admins.Find(id);
                db.Admins.Remove(admin);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //kiểm tra các thuộc tính khi đăng ký ADmin
        public bool CheckUserAdmin(string userAdmin)
        {
            return db.Admins.Count(x => x.UserAdmin == userAdmin) > 0;
        }

        //lấy ds các quyền truy cập
        public List<string> GetListCredential(string adminName)
        {
            var user = db.Admins.Single(x => x.UserAdmin == adminName);
            var data = (from a in db.Credentials
                        join b in db.AdminGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }
    }
}
