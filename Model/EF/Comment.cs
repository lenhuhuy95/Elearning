namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int CommentID { get; set; }

        public long? CourseID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime? Time { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public virtual Course Course { get; set; }
    }
}
