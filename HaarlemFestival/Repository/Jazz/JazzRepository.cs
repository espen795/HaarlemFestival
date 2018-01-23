using System;
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

        public List<Models.Jazz> GetAllJazzs()
        {
            List<Models.Jazz> jazzs = db.Jazzs.ToList();
            return jazzs;
        }

        public Models.Jazz GetId(int id)
        {
            return db.Jazzs.Find(id);
        }

        public List<Activity> getJazz(BesteldeActiviteit besteldeActiviteit)
        {
            List<Activity> dbActivity = db.Activities.Where(a => (a.Day.DayId == besteldeActiviteit.Activiteit.Day.DayId) && (a.EventType == besteldeActiviteit.Activiteit.EventType)).ToList();
            return dbActivity;
        }

    }
}