using System.Collections.Generic;
namespace HaarlemFestival.Models
{
    public class Event
    {
        public virtual List<Activity> Activities { get; set; }
    }
}