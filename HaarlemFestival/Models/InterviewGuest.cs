using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class InterviewGuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<InterviewQuestion> Questions { get; set; }
    }
}