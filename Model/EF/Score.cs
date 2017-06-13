namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Score")]
    public partial class Score
    {
        public long ScoreID { get; set; }

        public long? CourseID { get; set; }

        public long? UserID { get; set; }

        public long? ExamID { get; set; }

        [Column("Score")]
        [StringLength(10)]
        public string Score1 { get; set; }

        public DateTime? Date { get; set; }
    }
}
