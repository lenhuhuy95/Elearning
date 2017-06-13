using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ExamDao
    {
        ElearningDbContext db = null;
        public ExamDao()
        {
            db = new ElearningDbContext();
        }

        public bool DeleteQuesExam(long id)
        {
            try
            {
                var eq = db.ExamQuestions.Find(id);
                db.ExamQuestions.Remove(eq);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Update(Exam entity)
        {
            try
            {
                var exam = db.Exams.Find(entity.ExamID);
                exam.Name = entity.Name;
                exam.CourseID = entity.CourseID;
                exam.DateCreate = entity.DateCreate;
                exam.DateUpdate = entity.DateUpdate;
                exam.Type = entity.Type;
                exam.NumberQuestion = entity.NumberQuestion;
                exam.Status = entity.Status;
                exam.Time = entity.Time;
                exam.TimeStart = entity.TimeStart;
                exam.TimeEnd = entity.TimeEnd;
                exam.DateStart = entity.DateStart;
                exam.DateEnd = entity.DateEnd;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Exam ViewDetail(long id)
        {
            return db.Exams.Find(id);
        }
        //xóa Exam
        public bool DeleteExam(long id)
        {
            try
            {
                var ex = db.Exams.Find(id);
                db.Exams.Remove(ex);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ExamQuestion> ListQuesSelectIDExam(long id)
        {
            return db.ExamQuestions.Where(x => x.ExamID == id).OrderBy(x => x.QuestionID).ToList();
        }

        //Lấy ds ảnh trong Table Exam theo Status = True 
        public List<Exam> ListAll(long courseid)
        {
            return db.Exams.Where(x => x.Status == true && x.CourseID == courseid).ToList();
        }

        public Score ScoreDetail(long scoreid)
        {
            return db.Scores.Find(scoreid);
        }

        //Insert dữ liệu vào bảng Answer
        public long InsertAnswer(Answer entity)
        {
            db.Answers.Add(entity);
            db.SaveChanges();
            return entity.CourseID.Value;
        }

        //Insert dữ liệu vào bảng Score
        public long InsertScore(Score entity)
        {
            db.Scores.Add(entity);
            db.SaveChanges();
            return entity.ScoreID;
        }
        public List<Answer> ListAllAnswer(long userid, long examid)
        {
            return db.Answers.Where(x => x.ExamID == examid && x.UserID == userid).ToList();
        }

        //Insert dữ liệu vào bảng ExamQues
        public long InsertExamQues(ExamQuestion entity)
        {
            db.ExamQuestions.Add(entity);
            db.SaveChanges();
            return entity.ExamID;
        }
        //thay đổi trạng thái của exam
        public bool ChangeStatus(long id)
        {
            var ex = db.Exams.Find(id);
            ex.Status = !ex.Status;
            db.SaveChanges();
            return ex.Status.Value;
        }

        public List<ExamQuestion> ListQuestionOfExam(long examid)
        {
            return db.ExamQuestions.Where(x => x.ExamID == examid).ToList();
        }


        public List<Question> ListAllQuestionOfCourseID(long examid, long courseid)
        {
            var listquesOfExam = db.ExamQuestions.Where(x => x.ExamID == examid).ToList();
            var listquesOfCourse = db.Questions.Where(x => x.CourseID == courseid).ToList();
            var ques = new List<Question>();
            foreach (var item in listquesOfCourse)
            {
                foreach (var item2 in listquesOfExam)
                {
                    if (item.QuestionID == item2.QuestionID)
                    {
                        ques.Add(item);
                    }
                }
            }
            return ques;
        }

        public List<Score> CheckUserExam(long userID, long courseid)
        {
            return db.Scores.Where(x => x.UserID == userID && x.CourseID == courseid).ToList();
        }


        public SubmitFile CheckFileUser(long examid, long userid)
        {
            return db.SubmitFiles.FirstOrDefault(x => x.ExamID == examid && x.UserID == userid);
        }

        public List<Score> getListScore(long examID)
        {
            return db.Scores.Where(x => x.ExamID.Value == examID).ToList();
        }

        //Insert dữ liệu vào bảng Score
        public long ReInsertScore(Score entity)
        {
            var sc = db.Scores.FirstOrDefault(x => x.ExamID == entity.ExamID && x.UserID == entity.UserID);
            if (sc != null)
            {
                //update diem
                sc.Score1 = entity.Score1;
                db.SaveChanges();
                return entity.ScoreID;
            }
            else
            {
                //insert
                db.Scores.Add(entity);
                db.SaveChanges();
                return entity.ScoreID;
            }
        }
    }
}
