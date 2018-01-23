using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarlemFestival.Models
{
    public class Talking : Activity
    {
        public int TalkId { get; set; }
        public virtual Talk Talk { get; set; }

        [NotMapped]
        public string QuestionReceiver { get; set; }

        public Talking() 
        {

        }

    }
}