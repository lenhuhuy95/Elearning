using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elearning.Models
{
    [Serializable] //tuần tự hóa
    public class CartItem
    {
        public Course Course { set; get; }
    }
}