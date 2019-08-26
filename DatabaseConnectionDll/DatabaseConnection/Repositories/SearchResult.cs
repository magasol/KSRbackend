using System;

namespace DatabaseConnection.Repositories
{
    public class SearchResult
    {
        public int travel_id { get; set; }
        public string train_name { get; set; }
        public DateTime departure_date { get; set; }
        public TimeSpan departure_hour { get; set; }
        public decimal total_price { get; set; }
        public TimeSpan total_duration { get; set; }

        public SearchResult(
            int travel_id,
            string train_name,
            DateTime departure_date,
            TimeSpan departure_hour,
            decimal total_price,
            TimeSpan total_duration)
        {
            this.travel_id = travel_id;
            this.train_name = train_name;
            this.departure_date = departure_date;
            this.departure_hour = departure_hour;
            this.total_price = total_price;
            this.total_duration = total_duration;
        }

        public override string ToString()
        {
            return "\nSearchResult:"
                + "\ntravel_id: " + travel_id
                + "\ntrain_name: " + train_name
                + "\ndeparture_date: " + departure_date
                + "\ndeparture_hour: " + departure_hour
                + "\ntotal_price:" + total_price
                + "\ntota_duration: " + total_duration;
        }
    }
}