using Labs.DataAccess.Contracts;
using Labs.DataAccess.Enums;
using Labs.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.DataAccess.Repositories
{
    public static class RepositoryContainer
    {
        public static Selector Selector { get; set; } = Selector.Lab67;

        public static IRepository<Destinations> DestinationRepository
        {
            get => Selector switch
            {
                Selector.Lab67 => new DestinationRepository(),
                Selector.Lab8 => new LinqDestinationRepository(),
                _ => throw new Exception("Not correct selector value")
            };
        }

        public static IRepository<AircraftTypes> AircraftTypeRepository
        {
            get => Selector switch
            {
                Selector.Lab67 => new AircraftTypeRepository(),
                Selector.Lab8 => new LinqAircraftTypeRepository(),
                _ => throw new Exception("Not correct selector value")
            };
        }

        public static IRepository<Routes> RouteRepository
        {
            get => Selector switch
            {
                Selector.Lab67 => new RouteRepository(),
                Selector.Lab8 => new LinqRouteRepository(),
                _ => throw new Exception("Not correct selector value")
            };
        }

        public static IRepository<Flights> FlightRepository
        {
            get => Selector switch
            {
                Selector.Lab67 => new FlightRepository(),
                Selector.Lab8 => new LinqFlightRepository(),
                _ => throw new Exception("Not correct selector value")
            };
        }
    }
}
