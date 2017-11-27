using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainReservation.Contract
{
    public interface ITicketOffice
    {
        ReservationResponse MakeReservation(ReservationRequest request);
    }

    public class ReservationRequest
    {
        public string Destination { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string[] Passengers { get; set; }
    }

    public class ReservationResponse
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string[] Seats { get; set; }
        public string[] Passengers { get; set; }
        public Guid TrainId { get; set; }
    }
}
