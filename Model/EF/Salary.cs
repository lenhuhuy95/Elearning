namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Salary")]
    public partial class Salary
    {
        public long SalaryID { get; set; }

        public long? AdminID { get; set; }

        public long? CourseID { get; set; }

        public double? Cost { get; set; }

        public int? Percent { get; set; }

        public long? CountUser { get; set; }

        public double? payment { get; set; }

        public double? SalaryTotal { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual Admin Admin { get; set; }
    }
}
