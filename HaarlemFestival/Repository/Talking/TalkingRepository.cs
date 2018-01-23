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

        // Deze werkt niet met activity? Wordt alleen gebruikt voor ophalen van talk in de info pagina; werkt niet bij bestellen activiteit.
        public Models.Talking GetTalkById(int id)
        {
            return db.Talkings.FirstOrDefault(m => m.TalkId == id);
        }

        // Deze werkt met activity? Wordt gebruikt bij bestellen van activiteit; werkt niet bij talk info.
        public Models.Talking GetOrderTalkById(int id)
        {
            return db.Talkings.Find(id);
        }

        public void SaveQuestionToDB(InterviewQuestion question)
        {
            db.InterviewQuestions.Add(question);
            db.SaveChanges();
        }
    }
}