using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class InterviewQuestion
    {
        public int InterviewQuestionId { get; set; }
        public string Content { get; set; }
        public string Receiver { get; set; }

        public InterviewQuestion()
        {

        }
    }
}