namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admin()
        {
            Salaries = new HashSet<Salary>();
        }

        public long AdminID { get; set; }

        [StringLength(250)]
        public string UserAdmin { get; set; }

        [StringLength(50)]
        public string NameAdmin { get; set; }

        [StringLength(50)]
        public string GroupID { get; set; }

        public string Description { get; set; }

        [StringLength(250)]
        public string PassWord { get; set; }

        public bool? Status { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salary> Salaries { get; set; }
    }
}
