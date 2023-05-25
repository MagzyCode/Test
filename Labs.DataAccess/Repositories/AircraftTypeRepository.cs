using Labs.DataAccess.Contracts;
using Labs.DataAccess.Helpers;
using Labs.DataAccess.Models;
using System.Data.SqlClient;

namespace Labs.DataAccess.Repositories
{
    public class AircraftTypeRepository : IRepository<AircraftTypes>
    {
        /// <summary>
        /// Создаёт тип самолёта в БД
        /// </summary>
        /// <param name="entity">Создаваемая модель</param>
        /// <returns>Возвращает true, если запись в бд создана и false если не создана + задаётся сообщение для ошибки</returns>
        public (bool created, string errorMessage) Create(AircraftTypes entity)
        {
            var result = (created: false, errorMessage: string.Empty);

            try
            {
                var query = $"INSERT INTO [Flights].[dbo].[AircraftTypes] (AircraftTypeName) " +
                    $"VALUES ('{entity.AircraftTypeName}');";
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
                var effectedRows = SqlHelper.ExecuteWithoutResult($"DELETE FROM [Flights].[dbo].[AircraftTypes] WHERE Id = {id}");

                result.deleted = effectedRows != 0;
                result.errorMessage = result.deleted ? "" : "Internal error";
            }
            catch (SqlException ex)
            {
                result.errorMessage = ex.Message;
            }

            return result;
        }

        public AircraftTypes Get(int id)
        {
            var query = $@"
                SELECT *
                FROM [Flights].[dbo].[AircraftTypes]
                WHERE Id = {id}";

            var result = SqlHelper.ExecuteWithResult<AircraftTypes>(query).Single();

            return result;
        }

        public List<AircraftTypes> GetAll()
        {
            var query = @$"
                SELECT *
                FROM [Flights].[dbo].[AircraftTypes]";

            var list = SqlHelper.ExecuteWithResult<AircraftTypes>(query);

            return list;
        }

        public (bool updated, string errorMessage) Update(AircraftTypes entity)
        {
            var result = (updated: false, errorMessage: string.Empty);

            if (entity?.Id <= 0)
            {
                result.errorMessage = "Cannot update aircraft type withot knoledge of it's id.";
            }
            else
            {
                var query = @$"
                UPDATE [Flights].[dbo].[AircraftTypes]
                SET AircraftTypeName = '{entity.AircraftTypeName}'
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
