using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Talking : Activity
    {
        public int TalkingId { get; set; }
        public virtual Talk Talk { get; set; }

        public Talking() 
        {

        }

    }
}