using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Talking
{
    interface ITalkingRepository
    {
        List<Models.Talking> GetAllTalks();
        Models.Talking GetTalkById(int id);
    }
}
