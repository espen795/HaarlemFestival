using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Dinner : Activity
    {
        public EventType EventType { set => EventType = EventType.DinnerInHaarlem; }
        public virtual Restaurant Restaurant { get; set; }
    }
}