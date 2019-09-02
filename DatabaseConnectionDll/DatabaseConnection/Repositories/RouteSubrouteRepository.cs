using System.Collections.Generic;
using System.Data;
using DatabaseConnection.entities;
using Npgsql;

namespace DatabaseConnection.Repositories
{
    class RouteSubrouteRepository : Repository<RouteSubroute>, IRouteSubrouteRepository
    {
        private NpgsqlConnection conn = new NpgsqlConnection("User ID=postgres;Password=adminadmin;Host=localhost;Port=5432;Database=traintickets;");

        public List<RouteSubroute> GetRoutePart(int route_id, string from_station, string to_station)
        {
            List<RouteSubroute> results = new List<RouteSubroute>();
            using (var command = new NpgsqlCommand(
                "select " +
                    "route_subroute.id, route_subroute.route_order_number, route_subroute.seats_amount, " +
                    "route_subroute.subroute_id, route_subroute.route_id " +
               "from " +
                    "route inner join route_subroute " +
                    "on route.id = route_subroute.route_id " +
                    "inner join subroute " +
                "on subroute.id = route_subroute.subroute_id " +
               "where route.id = :route_id " +
                    "and route_subroute.route_order_number >= ( " +
                        "select route_subroute.route_order_number from " +
                            "subroute inner join route_subroute " +
                            "on subroute.id = route_subroute.subroute_id " +
                            "inner join route " +
                            "on route.id = route_subroute.route_id " +
                        "where subroute.from_station = :from_station and route.id = :route_id) " +
                    "and route_subroute.route_order_number <= ( " +
                        "select route_subroute.route_order_number from " +
                            "subroute inner join route_subroute " +
                            "on subroute.id = route_subroute.subroute_id " +
                            "inner join route " +
                            "on route.id = route_subroute.route_id " +
                        "where subroute.to_station =  :to_station and route.id = :route_id); "
                , conn))
            {
                try
                {
                    conn.Open();

                    var from_station_db = new NpgsqlParameter(":from_station", DbType.String);
                    from_station_db.Value = from_station;
                    command.Parameters.Add(from_station_db);

                    var to_station_db = new NpgsqlParameter(":to_station", DbType.String);
                    to_station_db.Value = to_station;
                    command.Parameters.Add(to_station_db);

                    var date_db = new NpgsqlParameter(":route_id", DbType.Int32);
                    date_db.Value = route_id;
                    command.Parameters.Add(date_db);

                    command.Prepare();

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            results.Add(new RouteSubroute
                            {
                                id = reader.GetInt32(0),
                                route_order_number = reader.GetInt16(1),
                                seats_amount = reader.GetInt16(2),
                                subroute_id = reader.GetInt32(3),
                                route_id = reader.GetInt32(4),
                            });
                        }
                        return results;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
