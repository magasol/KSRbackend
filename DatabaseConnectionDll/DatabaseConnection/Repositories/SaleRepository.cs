using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System.Data.Entity;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        private NpgsqlConnection conn = new NpgsqlConnection("User ID=postgres;Password=adminadmin;Host=localhost;Port=5432;Database=traintickets;");

        public bool addSale(string from_station, string to_station, int route_id, int traveller_id)
        {
            using (var context = new GenericContext<Entity>())
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
    }
}
