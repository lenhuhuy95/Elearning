using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class SlideDao
    {
        ElearningDbContext db = null;
        public SlideDao()
        {
            db = new ElearningDbContext();
        }

        //Lấy ds ảnh trong Table Slide theo Status = True và tăng dần theo DisplayOrder
        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder).ToList();
        }
    }
}
