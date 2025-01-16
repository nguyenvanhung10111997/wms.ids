using Dapper;
using System.Data;
using static Dapper.SqlMapper;

namespace wms.infrastructure
{
    public interface IReadOnlyRepository : IDisposable
    {
        IDbConnection Connection { get; }

        IEnumerable<T> FindAll<T>() where T : class;
        Task<IEnumerable<T>> FindAllAsync<T>() where T : class;

        T GetById<T>(T obj) where T : class;
        Task<T> GetByIdAsync<T>(T obj) where T : class;

        IEnumerable<T> StoreProcedureQuery<T>(string storeProcedureName, object param = null, int timeout = 60);
        Task<IEnumerable<T>> StoreProcedureQueryAsync<T>(string storeProcedureName, object param = null, int timeout = 60);

        Task<GridReader> QueryMultiAsync(string sqlQuery, object param = null);
        Task<GridReader> StoredProcedureQueryMultiAsync(string query, object param = null);

        IEnumerable<T> SQLQuery<T>(string query, object param = null);
        Task<IEnumerable<T>> SQLQueryAsync<T>(string query, object param = null);

        Task<T> SQLQueryFirstorDefaultAsync<T>(string query, object param = null);

        IEnumerable<TReturn> SQLQuery<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> map, object param = null);
        Task<IEnumerable<TReturn>> SQLQueryAsync<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> map, object param = null);
        Task<bool> IsAvaiable();
    }
}
