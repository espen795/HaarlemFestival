﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Jazz
{
    public class JazzRepository : IJazzRepository
    {
        private HFDB db = new HFDB();

        public List<Artist> GetAllArtists()
        {
            List<Artist> artists = db.Artists.ToList();

            return artists;
        }

    }
}