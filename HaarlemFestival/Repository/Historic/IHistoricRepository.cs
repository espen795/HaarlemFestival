using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaarlemFestival.Models;

namespace HaarlemFestival.Repository.Historic
{
    interface IHistoricRepository
    {
        List<Models.Historic> GetAllTours();

        List<Models.Location> GetAllLocations();
    }
}
