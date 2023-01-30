namespace metaapp.DataAccess.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISqlDataAccess
    {
        /// <summary>
        /// Used when loading ALL data with a stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> LoadDataAsync<T>(
            string storedProcedure,
            string connectionId = "sql");

        /// <summary>
        /// Used when loading SOME data with a specific parameter to pass to the invoked stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> LoadDataAsync<T, U>(
            string storedProcedure,
            U parameters,
            string connectionId = "sql");

        /// <summary>
        /// Used when inserting, updating, upserting data with a stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        Task SaveDataAsync<T>(
            string storedProcedure,
            T parameters,
            string connectionId = "sql");
    }
}
