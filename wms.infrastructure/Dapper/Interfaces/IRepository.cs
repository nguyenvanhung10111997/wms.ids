using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using System.Data;

namespace wms.infrastructure
{
    public interface IRepository : IDisposable
    {
        IDbConnection Connection { get; }

        T Insert<T>(T entity, IDbTransaction transaction = null) where T : class;
        IEnumerable<T> Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class;
        Task<T> InsertAsync<T>(T entity, IDbTransaction transaction = null) where T : class;
        Task<IEnumerable<T>> InsertManyAsync<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class;

        T Update<T>(T entity, IDbTransaction transaction = null) where T : class;
        Task<T> UpdateAync<T>(T entity, IDbTransaction transaction = null) where T : class;
        IEnumerable<T> Update<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class;
        Task<IEnumerable<T>> UpdateManyAsync<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class;
        int BulkUpdate<T>(T entity, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null) where T : class;

        int Execute(string storeProcedureName, object param = null, IDbTransaction transaction = null, int? timeOut = null);
        Task<int> ExecuteAsync(string storeProcedureName, object param = null, IDbTransaction transaction = null, int? timeOut = null);
        Task<T> ExecuteScalarAsync<T>(string storeProcedureName, object param = null, IDbTransaction transaction = null);
        IEnumerable<T> Execute<T>(string storeProcedureName, object param = null, IDbTransaction transaction = null, int? timeOut = null);
    }
}
