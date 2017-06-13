using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class UserDao 
    {
        ElearningDbContext db = null;
        public UserDao()
        {
            db = new ElearningDbContext();
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.UserID;
        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.UserID;
            }
            else
            {
                return user.UserID;
            }

        }

        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0; // tk ko ton tai
            }
            else
            {
                if (result.Status == false)
                {
                    return -1; //tai khoan bi khoa
                }
                else
                {
                    if (result.PassWord == passWord)

                        return 1; // Đăng nhập thành công
                    else
                        return -2; //Mật khẩu không đúng
                }
            }
        }

        //lấy ds user và search
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x=>x.UserID).ToPagedList(page, pageSize);
        }

        //thay đổi status user
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        //xóa user
        public bool DeleteUser(long id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //update user
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.UserID);
               
                if (!string.IsNullOrEmpty(entity.PassWord))
                {
                    user.PassWord = entity.PassWord;
                }
                user.Name = entity.Name;             
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.ModifiedDate = DateTime.Now;
                user.Image = entity.Image;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //logging
                return false;
            }
        }

        //trả về user theo id
        public User ViewDetail(long id)
        {
            return db.Users.Find(id);
        }

        //
        public int CountUserCourse(long id)
        {
            return db.User_Course.Where(x => x.CourseID == id).Count();
        }

        //kiểm tra các thuộc tính khi đăng ký User
        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }
        //
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
        //
        public List<User> ListAllUser()
        {
            return db.Users.OrderBy(x => x.UserID).ToList();
        }
    }
}
