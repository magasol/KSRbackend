using DatabaseConnection.entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseConnection.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public bool AddSale(string from_station, string to_station, int route_id, int traveller_id)
        {
            using (var context = new GenericContext<Sale>())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        int saleId = NextId();
                        int ticketId = 1;
                        Sale sale = new Sale();
                        sale.payment_status = true;
                        sale.sale_date = DateTime.Now;
                        sale.to_station = to_station;
                        sale.from_station = from_station;
                        sale.route_id = route_id;
                        sale.traveller_id = traveller_id;

                        Add(sale);

                        //throw new Exception(); <-- TEST
                        SaleTicketRepository saleTicketRepository = new SaleTicketRepository();
                        saleTicketRepository.addSaleTicket(saleId, ticketId);

                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");

                        return false;
                    }
                }
            }
        }

        public List<Sale> GetUserTickets(int user_id)
        {
            using (var context = new GenericContext<Sale>())
            {
                var item = context.Entity.Where<Sale>(s => s.traveller_id == user_id).OrderBy(s => s.sale_date).ToList();
                if (item == null)
                {
                    return null;
                }
                return item;
            }
        }
    }
}
