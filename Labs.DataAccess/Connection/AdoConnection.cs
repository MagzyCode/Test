using System.Data.SqlClient;

namespace Labs.DataAccess.Connection
{
    /// <summary>
    /// Класс подключения к базе данных использующий паттерн Singleton.
    /// Singleton - паттерн, позволяющий иметь в приложении только один объект класса.
    /// </summary>
    public class AdoConnection
    {
        /// <summary>
        /// Подключение к базе данных через ADO.NET
        /// </summary>
        private static SqlConnection _connection;
        /// <summary>
        /// Строка подключения
        /// </summary>
        private static readonly string _connectionString;

        /// <summary>
        /// Статический конструктор, который срабатывает в приложении один раз за всё время,
        /// когда кто-либо обращается к статическим членам класса
        /// </summary>
        static AdoConnection()
        {
            // Получение строки подключения из класса, который отвественен за настройки проекта
            _connectionString = Configuration.ConnectionString;
        }

        private AdoConnection() { }

        /// <summary>
        /// Получаем подключение к базе данных
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            // если _connection == null, тогда создаём новый SqlConnection со строкой подключения,
            // но если _connection != null, тогда возвращаем _connection
            _connection ??= new SqlConnection(_connectionString);

            return _connection;
        }
    }
}
