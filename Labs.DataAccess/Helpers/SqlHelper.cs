using Labs.DataAccess.Connection;
using System.Data.SqlClient;

namespace Labs.DataAccess.Helpers
{
    /// <summary>
    /// Класс формирования запросов к базе данных и получения результатов
    /// </summary>
    public static class SqlHelper
    {
        static SqlHelper()
        {
            _connection = AdoConnection.GetConnection();
        }

        private static SqlConnection _connection;

        /// <summary>
        /// Получение списка результатов на основе запроса
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<TResult> ExecuteWithResult<TResult>(string query)
        {
            var list = new List<TResult>();
            var type = typeof(TResult);

            _connection.Open();

            var sqlCommand = new SqlCommand(query, _connection);
            var dataReader = sqlCommand.ExecuteReader();

            while (dataReader.Read())
            {
                var result = (TResult)Activator.CreateInstance(type);

                foreach (var typeField in type.GetProperties().ToList())
                {
                    typeField.SetValue(result, dataReader[typeField.Name.ToLower()]);
                }

                list.Add(result);
            }

            _connection.Close();

            return list;
        }

        /// <summary>
        /// Выполнение запросов и получение количества изменённых строк
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int ExecuteWithoutResult(string query)
        {
            var effectedRows = 0;

            try
            {
                _connection.Open();

                var sqlCommand = new SqlCommand(query, _connection);
                effectedRows = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex) { }
            finally
            {
                _connection.Close();
            }

            return effectedRows;
        }

        /// <summary>
        /// Получение скалярного значения на основе запроса
        /// </summary>
        /// <typeparam name="TScalar"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static TScalar ExecuteWithScalar<TScalar>(string query)
        {
            _connection.Open();

            var sqlCommand = new SqlCommand(query, _connection);
            var scalar = (TScalar)sqlCommand.ExecuteScalar();

            _connection.Close();

            return scalar;
        }
    }
}
