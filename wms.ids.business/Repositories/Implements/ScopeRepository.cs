using Dapper;
using System.Data;
using wms.business;
using wms.ids.dto;
using wms.infrastructure;

namespace wms.ids.business.Repositories
{
    internal class ScopeRepository : BaseRepository, IScopeRepository
    {
        public ScopeRepository(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<IEnumerable<ScopeDto>> ReadAll()
        {
            try
            {
                var query = @"SELECT * FROM dbo.Scope WHERE IsDeleted = 0";
                var data = await ReadRepository.Connection.QueryAsync<ScopeDto>(query, commandType: CommandType.Text);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
