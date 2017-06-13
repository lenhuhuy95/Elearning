using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MenuDao
    {
        ElearningDbContext db = null;
        public MenuDao()
        {
            db = new ElearningDbContext();
        }

        //Gọi danh sách MainMenu theo TypeID
        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId).ToList();
        }
    }
}
