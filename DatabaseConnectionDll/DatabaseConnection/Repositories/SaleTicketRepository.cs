using DatabaseConnection.entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    class SaleTicketRepository : Repository<SaleTicket>, ISaleTicketRepository
    {
        public void addSaleTicket(int sale_id, int ticket_id)
        {
            SaleTicket saleTicket = new SaleTicket();
            saleTicket.sale_id = sale_id;
            saleTicket.ticket_id = ticket_id;
            saleTicket.amount = 1;

            Add(saleTicket);
        }
    }
}
