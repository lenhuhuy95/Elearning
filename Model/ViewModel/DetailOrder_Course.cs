using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class DetailOrder_Course
    {
        public long CourseID { get; set; }
        public long OderID { get; set; }
        public double? Cost { get; set; }
        public string CourseName { get; set; }
    }
}
