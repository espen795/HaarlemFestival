using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class InterviewQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public InterviewGuest QuestionedGuest { get; set; }
    }
}