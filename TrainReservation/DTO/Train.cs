using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainReservation.DTO
{
    public class Train
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public int Coachs { get; set; }
        public int SeatsInCoach { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
