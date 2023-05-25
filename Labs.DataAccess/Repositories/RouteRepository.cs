using Labs.DataAccess.Contracts;
using Labs.DataAccess.Helpers;
using Labs.DataAccess.Models;
using System.Data.SqlClient;

namespace Labs.DataAccess.Repositories
{
    public class RouteRepository : IRepository<Routes>
    {
        public (bool created, string errorMessage) Create(Routes entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            if (string.IsNullOrEmpty(entity?.RouteNumber) 
                || string.IsNullOrEmpty(entity?.ArrivalDestination)
                || string.IsNullOrEmpty(entity?.DepartureDestination)) 
            {
                result.errorMessage = "Invalid route properties";
            }
            else
            {
                try
                {
                    var destinations = SqlHelper.ExecuteWithResult<Destinations>(
                        $"SELECT * FROM [Flights].[dbo].[Destinations]");

                    var departureDestinationId = destinations.Where(x => x.DestinationName.Equals(entity.DepartureDestination)).Single().Id;
                    var arrivalDestinationId = destinations.Where(x => x.DestinationName.Equals(entity.ArrivalDestination)).Single().Id;

                    var query = $"INSERT INTO [Flights].[dbo].[Routes] (RouteNumber, DepartureDestinationId, ArrivalDestinationId)" +
                        $"VALUES ('{entity.RouteNumber}', {departureDestinationId}, {arrivalDestinationId})";
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
                var effectedRows = SqlHelper.ExecuteWithoutResult($"DELETE FROM [Flights].[dbo].[Routes] WHERE Id = {id}");

                result.deleted = effectedRows != 0;
                result.errorMessage = result.deleted ? "" : "Internal error";
            }
            catch (SqlException ex)
            {
                result.errorMessage = ex.Message;
            }

            return result;
        }

        public Routes Get(int id)
        {
            var query = @$"
                SELECT r.Id, r.RouteNumber, dd.DestinationName as DepartureDestination, da.DestinationName as ArrivalDestination
                FROM [Flights].[dbo].[Routes] r
                JOIN [Flights].[dbo].[Destinations] dd ON dd.Id = r.DepartureDestinationId
                JOIN [Flights].[dbo].[Destinations] da ON da.Id = r.ArrivalDestinationId
                WHERE r.Id = {id}";

            var result = SqlHelper.ExecuteWithResult<Routes>(query).Single();

            return result;
        }

        public List<Routes> GetAll()
        {
            var query = @$"
                SELECT r.Id, r.RouteNumber, dd.DestinationName as DepartureDestination, da.DestinationName as ArrivalDestination
                FROM [Flights].[dbo].[Routes] r
                JOIN [Flights].[dbo].[Destinations] dd ON dd.Id = r.DepartureDestinationId
                JOIN [Flights].[dbo].[Destinations] da ON da.Id = r.ArrivalDestinationId";

            var list = SqlHelper.ExecuteWithResult<Routes>(query);

            return list;
        }

        public (bool updated, string errorMessage) Update(Routes entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            if (entity?.Id <= 0)
            {
                result.errorMessage = "Cannot update route withot knoledge of it's id.";
            }
            else if (string.IsNullOrEmpty(entity?.DepartureDestination) 
                || string.IsNullOrEmpty(entity?.ArrivalDestination))
            {
                result.errorMessage = "Invalid destination name or names.";
            }
            else
            {
                try
                {
                    var destinations = SqlHelper.ExecuteWithResult<Destinations>(
                        $"SELECT * FROM [Flights].[dbo].[Destinations]");

                    var departureDestinationId = destinations.Where(x => x.DestinationName.Equals(entity.DepartureDestination)).Single().Id;
                    var arrivalDestinationId = destinations.Where(x => x.DestinationName.Equals(entity.ArrivalDestination)).Single().Id;

                    var query = @$"
                        UPDATE [Flights].[dbo].[Routes]
                        SET RouteNumber = '{entity.RouteNumber}', DepartureDestinationId = {departureDestinationId}, ArrivalDestinationId = {arrivalDestinationId} 
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
