using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Talk
    {
        public int TalkId { get; set; }
        public string person1;
        public string person2;
        public InterviewQuestion question;
        public int maxTickets;
        
        public Talk()
        {

        }
    }
}