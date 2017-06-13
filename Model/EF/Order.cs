namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [Key]
        public long OderID { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? CustomerID { get; set; }

        [StringLength(250)]
        public string ShipName { get; set; }

        [StringLength(50)]
        public string ShipMobile { get; set; }

        [StringLength(500)]
        public string ShipAddress { get; set; }

        [StringLength(250)]
        public string ShipEmail { get; set; }

        public bool? Status { get; set; }
    }
}
