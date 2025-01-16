using Dapper;
using Dapper.FastCrud;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace wms.infrastructure
{
    public sealed class DapperReadOnlyRepository : IReadOnlyRepository
    {
        private readonly IDbConnection _connection;
        private bool disposedValue = false;

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public DapperReadOnlyRepository(IDbConnection Connection)
        {
            _connection = Connection;
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        public IEnumerable<T> FindAll<T>() where T : class
        {
            return _connection.Find<T>();
        }

        public Task<IEnumerable<T>> FindAllAsync<T>() where T : class
        {
            var result = _connection.FindAsync<T>();
            return result;
        }

        public T GetById<T>(T obj) where T : class
        {
            return _connection.Get<T>(obj);
        }

        public async Task<T> GetByIdAsync<T>(T obj) where T : class
        {
            var result = await _connection.GetAsync(obj);
            return result;
        }

        public IEnumerable<T> StoreProcedureQuery<T>(string storeProcedureName, object param = null, int timeout = 60)
        {
            return _connection.Query<T>(storeProcedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        public async Task<IEnumerable<T>> StoreProcedureQueryAsync<T>(string storeProcedureName, object param = null, int timeout = 60)
        {
            return await _connection.QueryAsync<T>(storeProcedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        public IEnumerable<T> SQLQuery<T>(string query, object param = null)
        {
            var result = _connection.Query<T>(query, param);
            return result;
        }

        public async Task<IEnumerable<T>> SQLQueryAsync<T>(string SQLQuery, object param = null)
        {
            var result = await _connection.QueryAsync<T>(SQLQuery, param);
            return result;
        }

        public async Task<T> SQLQueryFirstorDefaultAsync<T>(string SQLQuery, object param = null)
        {
            var result = await _connection.QueryFirstOrDefaultAsync<T>(SQLQuery, param);
            return result;
        }

        public IEnumerable<TReturn> SQLQuery<TFirst, TSecond, TReturn>(string SQLQuery, Func<TFirst, TSecond, TReturn> map, object param = null)
        {
            return _connection.Query<TFirst, TSecond, TReturn>(SQLQuery, map, param);
        }

        public async Task<IEnumerable<TReturn>> SQLQueryAsync<TFirst, TSecond, TReturn>(string SQLQuery, Func<TFirst, TSecond, TReturn> map, object param = null)
        {
            return await _connection.QueryAsync<TFirst, TSecond, TReturn>(SQLQuery, map, param);
        }

        public async Task<GridReader> ExecuteMultiAsync(string storeProcedureName, object param = null)
        {
            var result = await _connection.QueryMultipleAsync(storeProcedureName, param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<GridReader> QueryMultiAsync(string query, object param = null)
        {
            var result = await _connection.QueryMultipleAsync(query, param, commandType: CommandType.Text);
            return result;
        }

        public async Task<GridReader> StoredProcedureQueryMultiAsync(string query, object param = null)
        {
            var result = await _connection.QueryMultipleAsync(query, param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<T>> SqlQueryAsync<T>(string query, object param = null)
        {
            var result = await _connection.QueryAsync<T>(query, param, commandType: CommandType.Text);
            return result;
        }

        public async Task<bool> IsAvaiable()
        {
            return await Task.Run(() =>
            {
                try
                {
                    _connection.Open();
                    _connection.Close();
                }
                catch (SqlException)
                {
                    return false;
                }
                return true;
            });
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        if (_connection.State != ConnectionState.Closed)
                            _connection.Close();
                        _connection.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DapperReadOnlyRepository()
        {
            Dispose(false);
        }
    }
}
