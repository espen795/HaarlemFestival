using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class AnswerContactViewModel
    {
        public List<ContactMessage> Messages { get; set; }
        public QuestionCategory Category { get; set; }
    }
}