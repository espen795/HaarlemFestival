using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class TalkViewModel
    {
        public List<Talking> Talkings { get; set; }
        public List<BesteldeActiviteit> BesteldeActiviteiten { get; set; }
    }
}