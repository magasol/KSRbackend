using DatabaseConnection.entities;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace DatabaseConnection.Repositories
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        private NpgsqlConnection conn = new NpgsqlConnection("User ID=postgres;Password=adminadmin;Host=localhost;Port=5432;Database=traintickets;");

        public List<SearchResult> SearchForTrainConnection(DateTime date, string from_station, string to_station)
        {
            using (var command = new NpgsqlCommand(
                "SELECT s1.travel_id , s1.train_name, s1.departure_date, s1.departure_hour," +
                    " sum(s1.price) AS total_price, sum(s1.duration) AS total_duration " +
                "FROM " +
                    "(SELECT route.id AS travel_id, route.train_name AS train_name, route.departure_date AS departure_date, " +
                        "route.departure_hour AS departure_hour, subroute.price AS price, subroute.travel_duration AS duration, " +
                        "route_subroute.route_order_number AS from_station_number, route_subroute.route_order_number AS to_station_number " +
                        "FROM public.route " +
                        "INNER JOIN public.route_subroute ON route.id = route_subroute.route_id " +
                        "INNER JOIN public.subroute ON subroute.id = route_subroute.subroute_id) s1 " +
                "RIGHT JOIN " +
                    "(SELECT t1.travel_id AS travel_id, t1.from_station_number AS from_station_number,t2.to_station_number AS to_station_number " +
                        "FROM " +
                            "(SELECT route.id AS travel_id, route_subroute.route_order_number AS from_station_number " +
                            "FROM public.route " +
                            "INNER JOIN public.route_subroute ON route.id = route_subroute.route_id " +
                            "INNER JOIN public.subroute ON subroute.id = route_subroute.subroute_id " +
                            "WHERE subroute.from_station = :from_station) t1 " +
                    "INNER join " +
                            "(SELECT route.id AS travel_id, route_subroute.route_order_number AS to_station_number " +
                            "FROM public.route " +
                            "INNER JOIN public.route_subroute ON route.id = route_subroute.route_id " +
                            "INNER JOIN public.subroute ON subroute.id = route_subroute.subroute_id " +
                            "WHERE subroute.to_station = :to_station) t2 ON(t1.travel_id = t2.travel_id)) s2 " +
                "ON s1.travel_id=s2.travel_id AND s1.from_station_number>=s2.from_station_number " +
                    "and s1.to_station_number<=s2.to_station_number " +
                "WHERE " +
                    "s1.departure_date=:date " +
                "GROUP BY " +
                    "s1.travel_id, s1.train_name, s1.departure_date, s1.departure_hour " +
                "ORDER BY " +
                    "s1.departure_hour;", conn))
            {
                //command.Parameters.AddWithValue("@from_station", from_station);
                // command.Parameters.AddWithValue("@to_station", to_station);
                //command.Parameters.AddWithValue("@date", date)

                try
                {
                    conn.Open();

                    var from_station_db = new NpgsqlParameter(":from_station", DbType.String);
                    from_station_db.Value = from_station;
                    command.Parameters.Add(from_station_db);

                    var to_station_db = new NpgsqlParameter(":to_station", DbType.String);
                    to_station_db.Value = to_station;
                    command.Parameters.Add(to_station_db);

                    var date_db = new NpgsqlParameter(":date", DbType.DateTime);
                    date_db.Value = date;
                    command.Parameters.Add(date_db);

                    command.Prepare();

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<SearchResult> results = new List<SearchResult>();
                        while (reader.Read())
                        {
                            results.Add(new SearchResult(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetDateTime(2),
                            reader.GetTimeSpan(3),
                            reader.GetDecimal(4),
                            reader.GetTimeSpan(5)));
                        }

                        return results;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}