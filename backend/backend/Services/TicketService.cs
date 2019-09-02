
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

            string resultString = "";
            foreach (Sale sale in saleResults)
            {
                TicketResponse ticketResponse
                    = new TicketResponse(
                        "train name",
                        sale.from_station,
                        sale.to_station,
                        sale.sale_date.ToString(),
                        "departure date",
                        "departure hour",
                        "price",
                        "time",
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
