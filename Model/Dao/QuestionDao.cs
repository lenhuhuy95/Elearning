using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class QuestionDao
    {
        ElearningDbContext db = null;
        public QuestionDao()
        {
            db = new ElearningDbContext();
        }
        public List<Question> ListQuestionIDCourse(long courseID)
        {
            return db.Questions.Where(x => x.CourseID == courseID).OrderBy(y => y.QuestionID).ToList();
        }

        public Question ViewDetail(long id)
        {
            return db.Questions.Find(id);
        }

        public long Insert(Question entity)
        {
            db.Questions.Add(entity);
            db.SaveChanges();
            return entity.QuestionID;
        }

        public bool Update(Question entity)
        {
            try
            {
                var question = db.Questions.Find(entity.QuestionID);
                question.Ques = entity.Ques;
                question.Option1 = entity.Option1;
                question.Option2 = entity.Option2;
                question.Option3 = entity.Option3;
                question.Option4 = entity.Option4;
                question.Option5 = entity.Option5;
                question.Option6 = entity.Option6;
                question.Option7 = entity.Option7;
                question.Option8 = entity.Option8;
                question.Option9 = entity.Option9;
                question.Option10 = entity.Option10;
                question.Description = entity.Description;
                question.Type = entity.Type;
                question.ModifiedDate = DateTime.Now;
                question.CorrectAnswer = entity.CorrectAnswer;
                question.CorrectAnswer1 = entity.CorrectAnswer1;
                question.CorrectAnswer2 = entity.CorrectAnswer2;
                question.CorrectAnswer3 = entity.CorrectAnswer3;
                question.CorrectAnswer4 = entity.CorrectAnswer4;
                question.CorrectAnswer5 = entity.CorrectAnswer5;
                question.CorrectAnswer6 = entity.CorrectAnswer6;
                question.CorrectAnswer7 = entity.CorrectAnswer7;
                question.CorrectAnswer8 = entity.CorrectAnswer8;
                question.CorrectAnswer9 = entity.CorrectAnswer9;
                question.CorrectAnswer10 = entity.CorrectAnswer10;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //thay đổi status
        public bool ChangeStatus(long id)
        {
            var question = db.Questions.Find(id);
            question.Status = !question.Status;
            db.SaveChanges();
            return question.Status.Value;
        }

        //xóa 
        public bool DeleteQuestion(long id)
        {
            try
            {
                var question = db.Questions.Find(id);
                db.Questions.Remove(question);
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
