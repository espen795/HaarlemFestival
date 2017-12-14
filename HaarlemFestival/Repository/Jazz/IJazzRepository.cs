﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Jazz
{
    interface IJazzRepository
    {
        List<Artist> GetAllArtists();
    }
}