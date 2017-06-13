using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elearning.Models
{
    public class TNModels
    {
        public class Ques
        {
            public int ID { get; set; }

            public string dapan { get; set; }
        }
        public List<Ques> ListQues { get; set; }
    }
}