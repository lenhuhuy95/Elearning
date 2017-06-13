namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        public long QuestionID { get; set; }

        public long? CourseID { get; set; }

        public string Description { get; set; }

        public string Ques { get; set; }

        [StringLength(500)]
        public string Option1 { get; set; }

        [StringLength(500)]
        public string Option2 { get; set; }

        [StringLength(500)]
        public string Option3 { get; set; }

        [StringLength(500)]
        public string Option4 { get; set; }

        [StringLength(500)]
        public string Option5 { get; set; }

        [StringLength(500)]
        public string Option6 { get; set; }

        [StringLength(500)]
        public string Option7 { get; set; }

        [StringLength(500)]
        public string Option8 { get; set; }

        [StringLength(500)]
        public string Option9 { get; set; }

        [StringLength(500)]
        public string Option10 { get; set; }

        public string CorrectAnswer { get; set; }

        public bool? CorrectAnswer1 { get; set; }

        public bool? CorrectAnswer2 { get; set; }

        public bool? CorrectAnswer3 { get; set; }

        public bool? CorrectAnswer4 { get; set; }

        public bool? CorrectAnswer5 { get; set; }

        public bool? CorrectAnswer6 { get; set; }

        public bool? CorrectAnswer7 { get; set; }

        public bool? CorrectAnswer8 { get; set; }

        public bool? CorrectAnswer9 { get; set; }

        public bool? CorrectAnswer10 { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public bool? Status { get; set; }

        [StringLength(10)]
        public string Type { get; set; }
    }
}
