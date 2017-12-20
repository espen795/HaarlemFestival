﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaarlemFestival.Models
{
    public class BesteldeActiviteit
    {
        public int BesteldeActiviteitId { get; set; }
        public int ReserveringId { get; set; }
        public virtual Reservering Reservering { get; set; }
        public int Aantal { get; set; }
        public int AantalAlternatief { get; set; }
        public string Opmerking { get; set; }
        public virtual Activity Activiteit { get; set; }
    }
}