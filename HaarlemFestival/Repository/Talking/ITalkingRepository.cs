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
        List<Talk> GetAllTalks();
        Talk GetTalkById(int id);
    }
}
