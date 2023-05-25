using Labs.DataAccess.Contracts;
using Labs.DataAccess.Helpers;
using Labs.DataAccess.Models;
using System.Data.SqlClient;

namespace Labs.DataAccess.Repositories
{
    public class FlightRepository : IRepository<Flights>
    {
        public (bool created, string errorMessage) Create(Flights entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            if (string.IsNullOrEmpty(entity?.RouteNumber)
                || entity?.DepartureDate == default
                || entity?.ArrivalDate == default
                || string.IsNullOrEmpty(entity?.AircraftType)
                )
            {
                result.errorMessage = "Invalid flight properties";
            }
            else
            {
                try
                {
                    var routes = SqlHelper.ExecuteWithResult<Routes>(
                        $"SELECT r.Id, r.RouteNumber, dd.DestinationName as DepartureDestination, da.DestinationName as ArrivalDestination\r\n" +
                        $"FROM [Flights].[dbo].[Routes] r\r\n" +
                        $"JOIN [Flights].[dbo].[Destinations] dd ON dd.Id = r.DepartureDestinationId\r\n" +
                        $"JOIN [Flights].[dbo].[Destinations] da ON da.Id = r.ArrivalDestinationId");
                    var routeId = routes.Single(x => x.RouteNumber.Equals(entity.RouteNumber)).Id;

                    var aircraftTypes = SqlHelper.ExecuteWithResult<AircraftTypes>(
                        $"SELECT * FROM [Flights].[dbo].[AircraftTypes]");
                    var aircraftTypeId = aircraftTypes.Single(x => x.AircraftTypeName.Equals(entity.AircraftType)).Id;

                    var query = $"INSERT INTO [Flights].[dbo].[Flights] (RouteId, DepartureDate, ArrivalDate, AircraftTypeId)" +
                        $" VALUES ({routeId}, '{entity.DepartureDate.ToString("yyyy-MM-dd")}', '{entity.ArrivalDate.ToString("yyyy-MM-dd")}', {aircraftTypeId})";
                    var effectedRows = SqlHelper.ExecuteWithoutResult(query);

                    result.created = effectedRows != 0;
                    result.errorMessage = result.created ? "" : "Internal error";
                }
                catch (SqlException ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }

        public (bool deleted, string errorMessage) Delete(int id)
        {
            var result = (deleted: false, errorMessage: string.Empty);

            try
            {
                var effectedRows = SqlHelper.ExecuteWithoutResult($"DELETE FROM [Flights].[dbo].[Flights] WHERE Id = {id}");

                result.deleted = effectedRows != 0;
                result.errorMessage = result.deleted ? "" : "Internal error";
            }
            catch (SqlException ex)
            {
                result.errorMessage = ex.Message;
            }

            return result;
        }

        public Flights Get(int id)
        {
            var query = @$"
                SELECT f.Id, r.RouteNumber, f.DepartureDate, f.ArrivalDate, a.AircraftTypeName as AircraftType
                FROM [Flights].[dbo].[Flights] f
                JOIN [Flights].[dbo].[Routes] r ON f.RouteId = r.Id
                JOIN [Flights].[dbo].[AircraftTypes] a ON f.AircraftTypeId = a.Id
                WHERE f.Id = {id}";

            var result = SqlHelper.ExecuteWithResult<Flights>(query).Single();

            return result;
        }

        public List<Flights> GetAll()
        {
            var query = @$"
                SELECT f.Id, r.RouteNumber, f.DepartureDate, f.ArrivalDate, a.AircraftTypeName as AircraftType
                FROM [Flights].[dbo].[Flights] f
                JOIN [Flights].[dbo].[Routes] r ON f.RouteId = r.Id
                JOIN [Flights].[dbo].[AircraftTypes] a ON f.AircraftTypeId = a.Id";

            var list = SqlHelper.ExecuteWithResult<Flights>(query);

            return list;
        }

        public (bool updated, string errorMessage) Update(Flights entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            if (entity?.Id <= 0)
            {
                result.errorMessage = "Cannot update flight withot knoledge of it's id.";
            }
            else if (string.IsNullOrEmpty(entity?.RouteNumber)
                || string.IsNullOrEmpty(entity?.AircraftType)
                || entity?.ArrivalDate == default
                || entity?.DepartureDate == default)
            {
                result.errorMessage = "Invalid entity.";
            }
            else
            {
                try
                {
                    var routes = SqlHelper.ExecuteWithResult<Routes>(
                        $"SELECT r.Id, r.RouteNumber, dd.DestinationName as DepartureDestination, da.DestinationName as ArrivalDestination\r\n" +
                        $"FROM [Flights].[dbo].[Routes] r\r\n" +
                        $"JOIN [Flights].[dbo].[Destinations] dd ON dd.Id = r.DepartureDestinationId\r\n" +
                        $"JOIN [Flights].[dbo].[Destinations] da ON da.Id = r.ArrivalDestinationId");
                    var routeId = routes.Single(x => x.RouteNumber.Equals(entity.RouteNumber)).Id;

                    var airctaftTypes = SqlHelper.ExecuteWithResult<AircraftTypes>(
                        $"SELECT * FROM [Flights].[dbo].[AircraftTypes]");
                    var airctaftTypeId = airctaftTypes.Single(x => x.AircraftTypeName.Equals(entity.AircraftType)).Id;

                    var query = @$"
                        UPDATE [Flights].[dbo].[Flights]
                        SET RouteId = {routeId}, DepartureDate = '{entity.DepartureDate.ToString("yyyy-MM-dd")}', ArrivalDate = '{entity.ArrivalDate.ToString("yyyy-MM-dd")}', AircraftTypeId = {airctaftTypeId} 
                        WHERE Id = {entity.Id}";

                    var effectedRows = SqlHelper.ExecuteWithoutResult(query);

                    result.updated = effectedRows != 0;
                    result.errorMessage = result.updated ? "" : "Internal error";
                }
                catch (SqlException ex)
                {
                    result.errorMessage = ex.Message;
                }
            }

            return result;
        }
    }
}
