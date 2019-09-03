
namespace backend.Responses
{
    class TicketResponse
    {
        public TicketResponse(
               string TrainName = "",
               string FromStation = "",
               string ToStation = "",
               string SaleDate = "",
               string DepartureDate = "",
               string DepartureHour = "",
               string Price = "",
               string Time = "",
               string PaymentStatus = "",
               string TicketsAmount = "",
               string TicketName = "")
        {
            this.TrainName = TrainName;
            this.FromStation = FromStation;
            this.ToStation = ToStation;
            this.SaleDate = SaleDate;
            this.DepartureDate = DepartureDate;
            this.DepartureHour = DepartureHour;
            this.Price = Price;
            this.Time = Time;
            this.PaymentStatus = PaymentStatus;
            this.TicketsAmount = TicketsAmount;
            this.TicketName = TicketName;
        }

        public string TrainName { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string SaleDate { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureHour { get; set; }
        public string Price { get; set; }
        public string Time { get; set; }
        public string PaymentStatus { get; set; }
        public string TicketsAmount { get; set; }
        public string TicketName { get; set; }

        public override string ToString()
        {
            return TrainName + "?"
                + FromStation + "?"
                + ToStation + "?"
                + SaleDate + "?"
                + DepartureDate + "?"
                + DepartureHour + "?"
                + Price + "?"
                + Time + "?"
                + PaymentStatus + "?"
                + TicketsAmount + "?"
                + TicketName;
        }
    }
}
