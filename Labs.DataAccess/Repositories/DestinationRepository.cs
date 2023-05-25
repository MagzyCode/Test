using Labs.DataAccess.Contracts;
using Labs.DataAccess.Helpers;
using Labs.DataAccess.Models;
using System.Data.SqlClient;

namespace Labs.DataAccess.Repositories
{
    public class DestinationRepository : IRepository<Destinations>
    {
        public (bool created, string errorMessage) Create(Destinations entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            try
            {
                var query = $"INSERT INTO [Flights].[dbo].[Destinations] (DestinationName) " +
                    $"VALUES ('{entity.DestinationName}');";
                var effectedRows = SqlHelper.ExecuteWithoutResult(query);

                result.created = effectedRows != 0;
                result.errorMessage = result.created ? "" : "Internal error";
            }
            catch (SqlException ex)
            {
                result.errorMessage = ex.Message;
            }

            return result;
        }

        public (bool deleted, string errorMessage) Delete(int id)
        {
            var result = (deleted: false, errorMessage: string.Empty);

            try
            {
                var effectedRows = SqlHelper.ExecuteWithoutResult($"DELETE FROM [Flights].[dbo].[Destinations] WHERE Id = {id}");
            
                result.deleted = effectedRows != 0;
                result.errorMessage = result.deleted ? "" : "Internal error";
            }
            catch (SqlException ex)
            {
                result.errorMessage = ex.Message;
            }

            return result;
        }

        public Destinations Get(int id)
        {
            var query = $@"
                SELECT *
                FROM [Flights].[dbo].[Destinations]
                WHERE Id = {id}";

            var result = SqlHelper.ExecuteWithResult<Destinations>(query).Single();

            return result;
        }

        public List<Destinations> GetAll()
        {
            var query = @$"
                SELECT *
                FROM [Flights].[dbo].[Destinations]";

            var list = SqlHelper.ExecuteWithResult<Destinations>(query);

            return list;
        }

        public (bool updated, string errorMessage) Update(Destinations entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            if (entity?.Id <= 0)
            {
                result.errorMessage = "Cannot update destination withot knoledge of it's id.";
            }
            else
            {
                var query = @$"
                UPDATE [Flights].[dbo].[Destinations]
                SET DestinationName = '{entity.DestinationName}'
                WHERE Id = {entity.Id}";

                try
                {
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
