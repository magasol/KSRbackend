using DatabaseConnection.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{

    public interface IRouteRepository : IRepository<Route>
    {
        List<SearchResult> SearchForTrainConnection(DateTime date, string from_station, string to_station);
    }
}
