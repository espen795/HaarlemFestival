﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class Day
    {
        public int DayId { get; set; }
        public string Naam { get; set; }
        public DateTime Date { get; set; }
    }
}