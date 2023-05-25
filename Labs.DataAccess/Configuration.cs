namespace Labs.DataAccess
{
    /// <summary>
    /// Класс для хранения настроек проекта
    /// </summary>
    internal static class Configuration
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return @"Data Source=localhost;Initial Catalog=Flights;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            }
        }
    }
}
