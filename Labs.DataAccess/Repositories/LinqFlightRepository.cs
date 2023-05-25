using Labs.DataAccess.Contracts;
using Labs.DataAccess.EFModels;
using Labs.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.DataAccess.Repositories
{
    public class LinqFlightRepository : IRepository<Flights>
    {
        public (bool created, string errorMessage) Create(Flights entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var routeId = context.Routes.FirstOrDefault(r => r.RouteNumber.Equals(entity.RouteNumber)).Id;
                    var aircraftTypeId = context.AircraftTypes.FirstOrDefault(x => x.AircraftTypeName.Equals(entity.AircraftType)).Id;

                    context.Flights.Add(new Flight()
                    {
                        RouteId = routeId,
                        ArrivalDate = entity.ArrivalDate,
                        DepartureDate = entity.DepartureDate,
                        AircraftTypeId = aircraftTypeId
                    });
                    context.SaveChanges();
                    result.created = true;
                }
                catch (Exception ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }

        public (bool deleted, string errorMessage) Delete(int id)
        {
            var result = (deleted: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var deletionFlight = context.Flights.FirstOrDefault(d => d.Id == id);

                    if (deletionFlight == null)
                    {
                        result.errorMessage = "Flights with given id not exists";
                    }
                    else
                    {
                        context.Flights.Remove(deletionFlight);
                        context.SaveChanges();
                        result.deleted = true;
                    }
                }
                catch (Exception ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }

        public Flights Get(int id)
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var result = context.Flights.FirstOrDefault(d => d.Id == id);

                    return result == null ? null : new Flights()
                    {
                        Id = result.Id,
                        RouteNumber = result.Route.RouteNumber,
                        AircraftType = result.AircraftType.AircraftTypeName,
                        ArrivalDate = result.ArrivalDate.Value,
                        DepartureDate = result.DepartureDate.Value
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<Flights> GetAll()
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    // check if lazy loading works
                    var deletionDestination = context.Flights.Select(x => new Flights()
                    {
                        Id = x.Id,
                        RouteNumber = x.Route.RouteNumber,
                        AircraftType = x.AircraftType.AircraftTypeName,
                        ArrivalDate = x.ArrivalDate.Value,
                        DepartureDate = x.DepartureDate.Value
                    }).ToList();

                    return deletionDestination;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public (bool updated, string errorMessage) Update(Flights entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var updationRouteId = context.Routes.FirstOrDefault(x => x.RouteNumber.Equals(entity.RouteNumber)).Id;
                    var updationAircraftTypeId = context.AircraftTypes.FirstOrDefault(x => x.AircraftTypeName.Equals(entity.AircraftType)).Id;

                    var updating = context.Flights.FirstOrDefault(x => x.Id == entity.Id);

                    if (updating == null)
                    {
                        result.errorMessage = "Flight with given id not exists";
                    }
                    else
                    {
                        updating.RouteId = updationRouteId;
                        updating.AircraftTypeId = updationAircraftTypeId;
                        updating.ArrivalDate = entity.ArrivalDate;
                        updating.DepartureDate = entity.DepartureDate;
                        context.SaveChanges();
                        result.updated = true;
                    }
                }
                catch (Exception ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }
    }
}
