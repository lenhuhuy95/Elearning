using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ScoreDao
    {
        ElearningDbContext db = null;
        public ScoreDao()
        {
            db = new ElearningDbContext();
        }

        public List<Score> ListAllScore(long id)
        {
            return db.Scores.Where(x => x.ExamID == id).OrderBy(x => x.ScoreID).ToList();
        }
    }
}
