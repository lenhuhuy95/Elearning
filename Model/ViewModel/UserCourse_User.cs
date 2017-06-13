using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class UserCourse_User
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long CourseID { get; set; }
        public bool Status { get; set; }
        public DateTime? DateJoin { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
