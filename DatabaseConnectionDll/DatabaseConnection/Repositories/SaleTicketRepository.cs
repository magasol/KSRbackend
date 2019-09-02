using DatabaseConnection.entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    internal class SaleTicketRepository : Repository<SaleTicket>, ISaleTicketRepository { }
}
