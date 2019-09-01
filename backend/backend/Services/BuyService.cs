using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System;

namespace backend.Services
{
    class BuyService
    {
        public enum BuyParam { RouteId = 0, UserId = 1, From = 2, To = 3 };

        public static string PrepareBuyResponse(string buyRequest)
        {
            string[] buyParams = buyRequest.Split('?');
            Console.WriteLine(" Buy credentials: ({0})", buyRequest);

            SaleRepository saleRepository = new SaleRepository();

            int trainId = Convert.ToInt32(buyParams[(int)BuyParam.RouteId]);
            int userId = Convert.ToInt32(buyParams[(int)BuyParam.UserId]);

            string from_station = buyParams[(int)BuyParam.From];
            string to_station = buyParams[(int)BuyParam.To];

            bool result = saleRepository.addSale(from_station, to_station, trainId, userId);

            return result.ToString();
        }
        public static string PrepareEmptyBuyRepsonse()
        {
            return "false";
        }
    }
}
