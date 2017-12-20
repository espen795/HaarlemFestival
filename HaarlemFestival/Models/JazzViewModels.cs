using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class JazzViewModels
    {
        public List<Artist> Artists { get; set; }
        public List<Jazz> Jazz { get; set; }
        public List<Day> Days { get; set; }
    }
}