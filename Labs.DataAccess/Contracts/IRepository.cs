namespace Labs.DataAccess.Contracts
{
    /// <summary>
    /// Интерфейс для CRUD операций
    /// </summary>
    /// <typeparam name="T">Тип, над которым будем проводить CRUD операции</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Создаём объект. Если всё хорошо сработало, возращается true + пустая строка с ошибкой.
        /// Если произошла ошибка возвращается false + строка с ошибкой
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public (bool created, string errorMessage) Create(T entity);

        /// <summary>
        /// Получаем все записи из таблицы.
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll();

        /// <summary>
        /// Обновляем объект. Если всё хорошо сработало, возращается true + пустая строка с ошибкой.
        /// Если произошла ошибка возвращается false + строка с ошибкой
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public (bool updated, string errorMessage) Update(T entity);

        /// <summary>
        /// Удаление записи из таблицы по Id. Если всё хорошо сработало, возращается true + пустая строка с ошибкой.
        /// Если произошла ошибка возвращается false + строка с ошибкой
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (bool deleted, string errorMessage) Delete(int id);

        /// <summary>
        /// Получение записи из таблицы по Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id);
    }
}
