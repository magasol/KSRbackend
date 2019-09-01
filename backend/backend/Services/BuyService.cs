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

            string train = buyParams[(int)BuyParam.RouteId];
            string userId = buyParams[(int)BuyParam.UserId];
            int from = int.Parse(buyParams[(int)BuyParam.From]);
            int to = int.Parse(buyParams[(int)BuyParam.To]);

            bool result = saleRepository.addSale(train, userId, from, to);

            return result.ToString();
        }
        public static string PrepareEmptyBuyRepsonse()
        {
            return "false";
        }
    }
}
