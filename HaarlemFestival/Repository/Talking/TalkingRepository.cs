using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Talking
{
    public class TalkingRepository : ITalkingRepository
    {
        private HFDB db = new HFDB();

        public List<Talk> GetAllTalks()
        {
            return db.Talks.ToList();
        }

        public Talk GetTalkById(int id)
        {
            return db.Talks.FirstOrDefault(t => t.TalkId == id);
        }
    }
}