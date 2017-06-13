namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            Comments = new HashSet<Comment>();
            Sections = new HashSet<Section>();
        }

        public long CourseID { get; set; }

        [StringLength(250)]
        public string CourseName { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public double? Cost { get; set; }

        public int? Percent { get; set; }

        public double? PromotionCost { get; set; }

        public long? CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        [StringLength(250)]
        public string Video { get; set; }

        [StringLength(50)]
        public string CourseSerial { get; set; }

        public long? UserID { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(100)]
        public string Tag { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        public int? PercentSalary { get; set; }

        public long? Instructed { get; set; }

        public int? NumberSection { get; set; }

        public bool? Status { get; set; }

        public long? MoneyID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Section> Sections { get; set; }
    }
}
