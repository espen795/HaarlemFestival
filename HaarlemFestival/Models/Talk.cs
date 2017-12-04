using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Talk
    {
        public int TalkId { get; set; }
        public string Person1 { get; set; }
        public string Person2 { get; set; }

        public int maxTickets { get; set; }
        
        public Talk()
        {

        }
    }
}