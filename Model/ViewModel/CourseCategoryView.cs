using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class CourseCategoryView
    {
        public long CourseID { get; set; }
        public string CourseName { get; set; }
        public long? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryParentName { get; set; }
    }
}
