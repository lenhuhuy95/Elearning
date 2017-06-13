namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Money")]
    public partial class Money
    {
        public long MoneyID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string Unit { get; set; }

        public double? ExchangeUSD { get; set; }

        public bool? Status { get; set; }
    }
}
