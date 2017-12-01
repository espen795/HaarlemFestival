using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class InterviewQuestion
    {
        public int InterviewQuestionId { get; set; }
        public string content;
        public string receiver;

        public InterviewQuestion()
        {

        }
    }
}