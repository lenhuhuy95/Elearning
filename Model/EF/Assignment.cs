namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Assignment")]
    public partial class Assignment
    {
        public long AssignmentID { get; set; }

        public long LectureID { get; set; }

        [StringLength(250)]
        public string AssignmentName { get; set; }

        public string Description { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? Deadline { get; set; }

        [StringLength(50)]
        public string DeadlineBy { get; set; }

        public virtual Lecture Lecture { get; set; }
    }
}
