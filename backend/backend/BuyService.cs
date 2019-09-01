using DatabaseConnection.entities;
using DatabaseConnection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend
{
    class BuyService
    {
        public enum RegisterParam { Train = 0, User = 1};

        public static string PrepareBuyResponse(string buyRequest)
        {
            string[] buyParams = buyRequest.Split(',');
            Console.WriteLine(" Buy credentials: ({0})", buyRequest);

           // TravellerRepository travellerRepository = new TravellerRepository();
            
            return "false";
        }
    }
}
