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

        public HistoricView GetAllTours()
        {
            List<Models.Historic> tours = db.Historics.ToList();
            HistoricView toursview = new HistoricView();


            toursview.Tours = tours;
            toursview.Reservering = new List<BesteldeActiviteit>();

            return toursview;
        }

        public List<Models.Location> GetAllLocations()
        {
            return db.Locations.ToList();
        }

        public Models.Historic GetTourForId(int id)
        {
            return db.Historics.Find(id);
        }
    }
}