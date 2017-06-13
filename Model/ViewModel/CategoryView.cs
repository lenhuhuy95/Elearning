using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class CategoryView
    {

        public long CategoryID { get; set; }


        public string CategoryName { get; set; }


        public string MetaTitle { get; set; }

        public long? ParentID { get; set; }

        public int? DisplayOrder { get; set; }


        public string SeoTitle { get; set; }

        public DateTime? CreateDate { get; set; }


        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        public string ModifiedBy { get; set; }

        public bool? Status { get; set; }

        //thêm 1 thuộc tính

        public string CategoryNameParent { get; set; }
    }
}
