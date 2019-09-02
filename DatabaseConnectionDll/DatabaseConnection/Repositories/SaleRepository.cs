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
                        RouteSubrouteRepository routeSubrouteRepository = new RouteSubrouteRepository();
                        List<RouteSubroute> routeParts = routeSubrouteRepository.GetRoutePart(route_id, from_station, to_station);

                        foreach (var routePart in routeParts)
                            if (routePart.seats_amount <= 0)
                                throw new Exception();

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
                        SaleTicket saleTicket = new SaleTicket
                        {
                            sale_id = saleId,
                            ticket_id = ticketId,
                            amount = 1
                        };
                        saleTicketRepository.Add(saleTicket);

                        for (int i = 0; i < routeParts.Count; i++)
                        {
                            routeParts[i].seats_amount -= 1;
                            var isSuccessful = routeSubrouteRepository.Update(routeParts[i]);
                            if (!isSuccessful)
                                throw new Exception();
                        }

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
