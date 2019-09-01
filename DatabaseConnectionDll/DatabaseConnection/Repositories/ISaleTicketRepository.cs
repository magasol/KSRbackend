using DatabaseConnection.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    interface ISaleTicketRepository : IRepository<SaleTicket>
    {
        void addSaleTicket(int sale_id, int ticket_id);
    }
}
