﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Historic : Activity
    {
        public int GuideId { get; set; }
        public virtual Guide Guide { get; set; }

        public Historic()
        {

        }
    }
}