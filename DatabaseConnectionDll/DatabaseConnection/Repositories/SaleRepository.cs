using DatabaseConnection.entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DatabaseConnection.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        private NpgsqlConnection conn = new NpgsqlConnection("User ID=postgres;Password=adminadmin;Host=localhost;Port=5432;Database=traintickets;");

        public bool AddSale(string from_station, string to_station, int route_id, int traveller_id, short amount, string ticket_name)
        {
            conn.Open();

            NpgsqlTransaction transaction = conn.BeginTransaction();
            try
            {
                RouteSubrouteRepository routeSubrouteRepository = new RouteSubrouteRepository();
                List<RouteSubroute> routeParts = routeSubrouteRepository.GetRoutePart(route_id, from_station, to_station);

                foreach (var routePart in routeParts)
                    if (routePart.seats_amount <= 0)
                        throw new Exception();

                int saleId = NextId();

                Sale sale = new Sale();
                sale.id = saleId;
                sale.payment_status = true;
                sale.sale_date = DateTime.Now;
                sale.to_station = to_station;
                sale.from_station = from_station;
                sale.route_id = route_id;
                sale.traveller_id = traveller_id;

                NpgsqlCommand addSale = new NpgsqlCommand(" insert into sale " +
                    "(id, payment_status, from_station, to_station, route_id, traveller_id) " +
                    "values(:id,:payment_status, :from_station, :to_station, :route_id, :traveller_id); ", conn);

                var id_db_sale = new NpgsqlParameter(":id", DbType.Int32);
                id_db_sale.Value = sale.id;
                addSale.Parameters.Add(id_db_sale);

                var payment_status_db = new NpgsqlParameter(":payment_status", DbType.Boolean);
                payment_status_db.Value = sale.payment_status;
                addSale.Parameters.Add(payment_status_db);

                var to_station_db = new NpgsqlParameter(":to_station", DbType.String);
                to_station_db.Value = sale.to_station;
                addSale.Parameters.Add(to_station_db);

                var from_station_db = new NpgsqlParameter(":from_station", DbType.String);
                from_station_db.Value = sale.from_station;
                addSale.Parameters.Add(from_station_db);

                var route_id_db = new NpgsqlParameter(":route_id", DbType.Int32);
                route_id_db.Value = sale.route_id;
                addSale.Parameters.Add(route_id_db);

                var traveller_id_db = new NpgsqlParameter(":traveller_id", DbType.Int32);
                traveller_id_db.Value = sale.traveller_id;
                addSale.Parameters.Add(traveller_id_db);

                addSale.Prepare();

                //throw new Exception(); <-- TEST

                NpgsqlCommand selectTicket = new NpgsqlCommand("SELECT id FROM ticket" +
                    " WHERE name=:ticket_name", conn);

                var ticket_name_db = new NpgsqlParameter(":ticket_name", DbType.String);
                ticket_name_db.Value = ticket_name;
                selectTicket.Parameters.Add(ticket_name_db);

                selectTicket.Prepare();

                int ticketId = (int) selectTicket.ExecuteScalar();

                SaleTicket saleTicket = new SaleTicket
                {
                    sale_id = saleId,
                    ticket_id = ticketId,
                    amount = amount
                };

                NpgsqlCommand addSaleTicket = new NpgsqlCommand("insert into sale_ticket (amount,sale_id,ticket_id) " +
                    " values(:amount, :sale_id, :ticket_id); ", conn);
                var amount_db = new NpgsqlParameter(":amount", DbType.Int32);
                amount_db.Value = saleTicket.amount;
                addSaleTicket.Parameters.Add(amount_db);

                var sale_id_db = new NpgsqlParameter(":sale_id", DbType.Int32);
                sale_id_db.Value = saleTicket.sale_id;
                addSaleTicket.Parameters.Add(sale_id_db);

                var ticket_id_db = new NpgsqlParameter(":ticket_id", DbType.Int32);
                ticket_id_db.Value = saleTicket.ticket_id;
                addSaleTicket.Parameters.Add(ticket_id_db);

                addSaleTicket.Prepare();

                int rowsAddedToSale = addSale.ExecuteNonQuery();
                int rowsAddedToTicketSale = addSaleTicket.ExecuteNonQuery();


                for (int i = 0; i < routeParts.Count; i++)
                {
                    routeParts[i].seats_amount -= 1;
                    NpgsqlCommand updateRouteSubroute = new NpgsqlCommand("update route_subroute set seats_amount = :seats_amount " +
                    "where id = :id;", conn);

                    var seats_amount_db = new NpgsqlParameter(":seats_amount", DbType.Int32);
                    seats_amount_db.Value = routeParts[i].seats_amount;
                    updateRouteSubroute.Parameters.Add(seats_amount_db);

                    var id_db = new NpgsqlParameter(":id", DbType.Int32);
                    id_db.Value = routeParts[i].id;
                    updateRouteSubroute.Parameters.Add(id_db);

                    updateRouteSubroute.Prepare();

                    int rowsUpdated = updateRouteSubroute.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                conn.Close();
                return false;
            }
            transaction.Commit();
            conn.Close();
            return true;
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
