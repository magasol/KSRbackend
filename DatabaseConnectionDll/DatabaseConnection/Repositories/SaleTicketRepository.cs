using DatabaseConnection.entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    public class SaleTicketRepository : Repository<SaleTicket>, ISaleTicketRepository {
        public SaleTicket getSaleTicket(int sale_id)
        {
            using (var context = new GenericContext<SaleTicket>())
            {
                var item = context.Entity.Where<SaleTicket>(t => t.sale_id == sale_id).SingleOrDefault();
                if (item == null)
                {
                    return null;
                }
                return item;
            }
        }
    }
}
