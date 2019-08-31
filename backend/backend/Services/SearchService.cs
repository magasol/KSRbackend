using DatabaseConnection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend
{
    class SearchService
    {
        public enum SearchParam { DepartureDate = 0, FromStation = 1, ToStation = 2 };

        public static string PrepareSearchResponse(string searchRequest)
        {
            string[] searchParams = searchRequest.Split('?');
            string departureDate = searchParams[(int)SearchParam.DepartureDate];
            string fromStation = searchParams[(int)SearchParam.FromStation];
            string toStation = searchParams[(int)SearchParam.ToStation];

            Console.WriteLine($" Search input: \n" +
                $"departureDate: {departureDate}\n" +
                $"fromStation: {fromStation}\n" +
                $"toStation: {toStation}\n");

            RouteRepository routeRepository = new RouteRepository();
            List<TrainConnection> result = routeRepository.SearchForTrainConnection(Convert.ToDateTime(departureDate), fromStation, toStation);

            string resultString = "";
            foreach (TrainConnection r in result)
            {
                SearchResponse searchResponse
                    = new SearchResponse(
                        r.travel_id,
                        r.train_name,
                        r.departure_date.ToString(),
                        r.departure_hour.ToString(),
                        r.total_price.ToString(),
                        r.total_duration.ToString());
                resultString += searchResponse.ToString() + ';';
            }
            resultString = resultString.Remove(resultString.Length - 1, 1);
            return resultString;
        }

        public static string PrepareEmptySearchRepsonse()
        {
            return " ? ? ? ? ? ";
        }
    }
}
