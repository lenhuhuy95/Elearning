namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentAnswer")]
    public partial class StudentAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SAID { get; set; }

        public long? UserID { get; set; }

        public long? QuestionID { get; set; }

        public string CorrectAnswer { get; set; }
    }
}
