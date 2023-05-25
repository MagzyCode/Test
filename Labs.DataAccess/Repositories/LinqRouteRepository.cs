using Labs.DataAccess.Contracts;
using Labs.DataAccess.EFModels;
using Labs.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.DataAccess.Repositories
{
    public class LinqRouteRepository : IRepository<Routes>
    {
        public (bool created, string errorMessage) Create(Routes entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var destinations = context.Destinations.ToList();
                    var arrivalDestinationId = destinations.FirstOrDefault(x => x.DestinationName.Equals(entity.ArrivalDestination)).Id;
                    var departureDestinationId = destinations.FirstOrDefault(x => x.DestinationName.Equals(entity.DepartureDestination)).Id;


                    context.Routes.Add(new Route()
                    {
                        RouteNumber = entity.RouteNumber,
                        ArrivalDestinationId = arrivalDestinationId,
                        DepartureDestinationId = departureDestinationId
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
                    var deletionType = context.Routes.FirstOrDefault(d => d.Id == id);

                    if (deletionType == null)
                    {
                        result.errorMessage = "Aircraft type with given id not exists";
                    }
                    else
                    {
                        context.Routes.Remove(deletionType);
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

        public Routes Get(int id)
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var result = context.Routes.FirstOrDefault(d => d.Id == id);

                    var destinations = context.Destinations.ToList();
                    var arrivalDestination = destinations.FirstOrDefault(x => x.Id == result.ArrivalDestinationId).DestinationName;
                    var departureDestination = destinations.FirstOrDefault(x => x.Id == result.DepartureDestinationId).DestinationName;

                    return result == null ? null : new Routes()
                    {
                        Id = result.Id,
                        RouteNumber = result.RouteNumber,
                        ArrivalDestination = arrivalDestination,
                        DepartureDestination = departureDestination
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<Routes> GetAll()
        {
            using (var context = new FlightsContext())
            {
                try
                {
                    var typesList = context.Routes.Select(x => new Routes()
                    {
                        Id = x.Id,
                        RouteNumber = x.RouteNumber,
                        // check if lazy loading works
                        ArrivalDestination = x.ArrivalDestination.DestinationName,
                        DepartureDestination = x.DepartureDestination.DestinationName
                    }).ToList();

                    return typesList;
                }
                catch
                {
                    return null;
                }
            }
        }

        public (bool updated, string errorMessage) Update(Routes entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            using (var context = new FlightsContext())
            {
                try
                {
                    var destinations = context.Destinations.ToList();

                    var updating = context.Routes.FirstOrDefault(x => x.Id == entity.Id);

                    if (updating == null)
                    {
                        result.errorMessage = "Route with given id not exists";
                    }
                    else
                    {
                        updating.RouteNumber = entity.RouteNumber;
                        updating.ArrivalDestinationId = destinations.FirstOrDefault(x => x.DestinationName.Equals(entity.ArrivalDestination)).Id;
                        updating.DepartureDestinationId = destinations.FirstOrDefault(x => x.DestinationName.Equals(entity.DepartureDestination)).Id;
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
