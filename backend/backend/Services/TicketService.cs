
using backend.Responses;
using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System;
using System.Collections.Generic;

namespace backend.Services
{
    class TicketService
    {
        public static string PrepareTicketResponse(string ticketRequest)
        {
            string userId = ticketRequest;

            Console.WriteLine($" Ticket input: \n" +
                $"userId: {userId}\n");

            SaleRepository saleRepository = new SaleRepository();
            List<Sale> saleResults = saleRepository.GetUserTickets(Convert.ToInt32(userId));

            RouteRepository routeRepository = new RouteRepository();

            string resultString = "";

            foreach (Sale sale in saleResults)
            {
                Route route = routeRepository.Get(sale.route_id);
                List<TrainConnection> result = routeRepository.SearchForTrainConnection(route.departure_date, sale.from_station, sale.to_station);
                decimal price = 0;
                TimeSpan time = new TimeSpan();
                TimeSpan hour = new TimeSpan();

                foreach (TrainConnection r in result)
                {
                    if(r.travel_id == route.id)
                    {
                        price = r.total_price;
                        time = r.total_duration;
                        hour = r.departure_hour;
                    }
                }

                TicketResponse ticketResponse
                    = new TicketResponse(
                        route.train_name.ToString(),
                        sale.from_station,
                        sale.to_station,
                        sale.sale_date.ToString(),
                        route.departure_date.ToString(),
                        hour.ToString(),
                        price.ToString(),
                        time.ToString(),
                        sale.payment_status.ToString());
                resultString += ticketResponse.ToString() + ';';
            }

            resultString = resultString.Remove(resultString.Length - 1, 1);
            return resultString;
        }

        public static string PrepareEmptyTicketRepsonse()
        {
            return " ? ? ? ? ? ? ? ? ";
        }
    }
}
