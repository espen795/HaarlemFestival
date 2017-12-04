using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class Talk
    {
        public int TalkId { get; set; }
        public string Naam { get; set; }
        public string Person1 { get; set; }
        public string Person2 { get; set; }
        public string Description { get; set; }
        public int MaxTickets { get; set; }

        public string Person1IMG { get; set; }
        public string Person1AltIMG { get; set; }
        public string Person2IMG { get; set; }
        public string Person2AltIMG { get; set; }

        public Talk()
        {

        }
    }
}