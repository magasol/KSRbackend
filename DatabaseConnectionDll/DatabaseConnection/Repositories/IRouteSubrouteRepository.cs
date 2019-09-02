using DatabaseConnection.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    interface IRouteSubrouteRepository: IRepository<RouteSubroute>
    {
        List<RouteSubroute> GetRoutePart(int route_id, string from_station, string to_station);
    }
}
