using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SelectFileDao
    {
        ElearningDbContext db = null;
        public SelectFileDao()
        {
            db = new ElearningDbContext();
        }

        public long Insert(SubmitFile entity)
        {
            db.SubmitFiles.Add(entity);
            db.SaveChanges();
            return entity.FileID;
        }

        public long Update(SubmitFile entity)
        {
            var file = db.SubmitFiles.Find(entity.FileID);
            file.NameFile = entity.NameFile;
            file.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return file.FileID;
        }

        public List<SubmitFile> ListAllFile(long id)
        {
            return db.SubmitFiles.Where(x=>x.ExamID == id).OrderBy(x => x.FileID).ToList();
        }
    }
}
