namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        public long AnswerID { get; set; }

        public long? ExamID { get; set; }

        public long? CourseID { get; set; }

        public long? UserID { get; set; }

        [Column("Answer")]
        [StringLength(10)]
        public string Answer1 { get; set; }

        public DateTime? Date { get; set; }

        public long? QuesID { get; set; }
    }
}
