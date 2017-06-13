namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Exam")]
    public partial class Exam
    {
        public long ExamID { get; set; }

        public long? CourseID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [Column("Exam")]
        [StringLength(250)]
        public string Exam1 { get; set; }

        public DateTime? DateCreate { get; set; }

        public DateTime? DateUpdate { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        public int? NumberQuestion { get; set; }

        public bool? Status { get; set; }

        [StringLength(50)]
        public string Time { get; set; }

        [StringLength(50)]
        public string TimeStart { get; set; }

        [StringLength(50)]
        public string TimeEnd { get; set; }

        [StringLength(50)]
        public string DateStart { get; set; }

        [StringLength(50)]
        public string DateEnd { get; set; }
    }
}
