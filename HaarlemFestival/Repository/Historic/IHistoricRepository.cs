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
        HistoricView GetAllTours();

        List<Location> GetAllLocations();

        Models.Historic GetTourForId(int id);
    }
}
