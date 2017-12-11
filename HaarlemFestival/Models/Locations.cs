using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HaarlemFestival.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        
        public string LocationName { get; set; }
        
        public string LocationDescription { get; set; }
    }
}