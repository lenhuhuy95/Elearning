namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Course
    {
        public long ID { get; set; }

        public long UserID { get; set; }

        public long CourseID { get; set; }

        public bool? Status { get; set; }

        public DateTime? DateJoin { get; set; }
    }
}
