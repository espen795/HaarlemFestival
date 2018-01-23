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
        Models.Jazz GetId(int id);
        List<Activity> getJazz(BesteldeActiviteit besteldeActiviteit);

    }
}
