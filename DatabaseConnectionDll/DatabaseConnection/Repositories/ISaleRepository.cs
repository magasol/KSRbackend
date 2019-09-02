using DatabaseConnection.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    public interface ISaleRepository : IRepository<Sale>
    {
        bool AddSale(string from_station, string to_station, int route_id, int traveller_id);

        List<Sale> GetUserTickets(int user_id);
    }
}
