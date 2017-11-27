using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Jazz : Activity
    {
        public int JazzId { get; set; }
        public virtual Concert JazzDays { get; set; }

        public Jazz()
        {

        }
    }
}