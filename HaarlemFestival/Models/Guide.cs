using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Guide
    {
        public int GuideId { get; set; }
        public string GuideName { get; set; }
        public DateTime WorksOn { get; set; }

        public Guide()
        {

        }
    }
}