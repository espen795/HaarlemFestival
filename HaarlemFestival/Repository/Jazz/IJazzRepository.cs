using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Jazz
{
    interface IJazzRepository
    {

        List<Models.Jazz> GetAllJazzs();
        List<Models.Jazz> GetAllJazzsByDay(int dayid);
        Models.Jazz GetId(int id); //GetJazzAllDays
        List<Activity> GetJazzByDay(BesteldeActiviteit besteldeActiviteit);
        List<Activity> GetJazzAllDays(BesteldeActiviteit besteldeActiviteit);
    }
}
