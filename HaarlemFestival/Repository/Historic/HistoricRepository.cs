using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Historic
{
    public class HistoricRepository : IHistoricRepository
    {
        private HFDB db = new HFDB();

        public List<Models.Historic> GetAllTours()
        {
            List<Models.Historic> tours = db.Historics.ToList();

            return tours;
        }

        public List<Models.Location> GetAllLocations()
        {
            List<Models.Location> locations = db.Locations.ToList();

            return locations;
        }
    }
}