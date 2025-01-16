using Dapper;
using Dapper.FastCrud;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using System.Data;
using System.Data.SqlClient;

namespace wms.infrastructure
{
    public sealed class DapperRepository : IRepository
    {
        private readonly IDbConnection _connection;
        private bool disposedValue = false;

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public DapperRepository(IDbConnection Connection)
        {
            _connection = Connection;
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        public T Insert<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
                _connection.Insert<T>(entity);
            else
                _connection.Insert<T>(entity, s => s.AttachToTransaction(transaction));
            return entity;
        }

        public async Task<T> InsertAsync<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
                await _connection.InsertAsync<T>(entity);
            else
                await _connection.InsertAsync<T>(entity, s => s.AttachToTransaction(transaction));
            return entity;
        }

        public IEnumerable<T> Insert<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
            {
                foreach (var entity in entities)
                {
                    _connection.Insert<T>(entity);
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    _connection.Insert<T>(entity, s => s.AttachToTransaction(transaction));
                }
            }
            return entities;
        }

        public async Task<IEnumerable<T>> InsertManyAsync<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
            {
                foreach (var entity in entities)
                {
                    await _connection.InsertAsync<T>(entity);
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    await _connection.InsertAsync<T>(entity, s => s.AttachToTransaction(transaction));
                }
            }
            return entities;
        }

        public T Update<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
                _connection.Update<T>(entity);
            else
                _connection.Update<T>(entity, s => s.AttachToTransaction(transaction));
            return entity;
        }

        public async Task<T> UpdateAync<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
                await _connection.UpdateAsync(entity);
            else
                await _connection.UpdateAsync(entity, s => s.AttachToTransaction(transaction));
            return entity;
        }

        public IEnumerable<T> Update<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
            {
                foreach (var entity in entities)
                {
                    _connection.Update<T>(entity);
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    _connection.Update<T>(entity, s => s.AttachToTransaction(transaction));
                }
            }
            return entities;
        }

        public async Task<IEnumerable<T>> UpdateManyAsync<T>(IEnumerable<T> entities, IDbTransaction transaction = null) where T : class
        {
            if (transaction == null)
            {
                foreach (var entity in entities)
                {
                    await _connection.UpdateAsync<T>(entity);
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    await _connection.UpdateAsync<T>(entity, s => s.AttachToTransaction(transaction));
                }
            }
            return entities;
        }

        public int BulkUpdate<T>(T entity, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null) where T : class
        {
            if (statementOptions == null)
            {
                return _connection.BulkUpdate<T>(entity);
            }
            else
            {
                return _connection.BulkUpdate<T>(entity, statementOptions);
            }
        }

        public IEnumerable<T> Execute<T>(string storeProcedureName, object param = null, IDbTransaction transaction = null, int? timeOut = null)
        {
            return _connection.Query<T>(storeProcedureName, param, transaction, true, timeOut, CommandType.StoredProcedure);
        }

        public int Execute(string storeProcedureName, object param = null, IDbTransaction transaction = null, int? timeOut = null)
        {
            return _connection.Execute(storeProcedureName, param, transaction, timeOut, CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteAsync(string storeProcedureName, object param = null, IDbTransaction transaction = null, int? timeOut = null)
        {
            var result = await _connection.ExecuteAsync(storeProcedureName, param, transaction, timeOut, CommandType.StoredProcedure);
            return result;
        }

        public async Task<T> ExecuteScalarAsync<T>(string storeProcedureName, object param = null, IDbTransaction transaction = null)
        {
            var result = await _connection.ExecuteScalarAsync<T>(storeProcedureName, param, transaction, commandType: CommandType.StoredProcedure);
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

        ~DapperRepository()
        {
            Dispose(false);
        }
    }
}
