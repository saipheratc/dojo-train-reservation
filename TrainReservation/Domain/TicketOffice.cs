using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainReservation.Contract;

namespace TrainReservation.Domain
{
    public class TicketOffice : ITicketOffice
    {
        private readonly ITrainRepository _repository;

        public TicketOffice(ITrainRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ReservationResponse MakeReservation(ReservationRequest request)
        {
            var trains = _repository.GetTrainByDestinationAndDate(request.Destination, request.ScheduledDate);

            if (trains == null || trains.Count() == 0)
                throw new ArgumentException();

            var train = trains.Where(x => x.SeatsInCoach >= request.Passengers.Length).FirstOrDefault();

            if (train == null)
                throw new ArgumentException();

            return new ReservationResponse
            {
                Id = 1,
                Destination = train.Destination,
                ScheduledDate = train.ScheduledDate,
                Seats = request.Passengers.Select((x, i) => "A" + (i + 1)).ToArray(),
                TrainId = train.Id
            };
        }
    }
}
