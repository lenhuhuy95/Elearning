namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubmitFile")]
    public partial class SubmitFile
    {
        [Key]
        public long FileID { get; set; }

        public long? UserID { get; set; }

        public long? ExamID { get; set; }

        [StringLength(500)]
        public string NameFile { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
