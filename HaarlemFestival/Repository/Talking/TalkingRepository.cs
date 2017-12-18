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

        public List<Models.Talking> GetAllTalks()
        {
            return db.Talkings.ToList();
        }

        public Models.Talking GetTalkById(int id)
        {
            return db.Talkings.FirstOrDefault(t => t.TalkId == id);
        }

        public void SaveQuestionToDB(InterviewQuestion question)
        {
            db.InterviewQuestions.Add(question);
            db.SaveChanges();
        }
    }
}