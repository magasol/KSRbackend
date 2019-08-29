using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend
{
    class SearchResponse
    {
        public SearchResponse(
               string TrainName = "",
               string DepartureDate = "",
               string DepartureHour = "",
               string Price = "",
               string Time = "")
        {
            this.TrainName = TrainName;
            this.DepartureDate = DepartureDate;
            this.DepartureHour = DepartureHour;
            this.Price = Price;
            this.Time = Time;
        }

        public string TrainName { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureHour { get; set; }
        public string Price { get; set; }
        public string Time { get; set; }

        public override string ToString()
        {
            return TrainName + ","
                + DepartureDate + ","
                + DepartureHour + ","
                + Price + ","
                + Time;

        }
    }
}
