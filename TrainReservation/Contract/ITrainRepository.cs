using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainReservation.DTO;

namespace TrainReservation.Contract
{
    public interface ITrainRepository
    {
        IEnumerable<Train> GetTrainByDestinationAndDate(string destination, DateTime scheduledDate);
    }


}
