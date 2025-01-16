using Dapper;
using System.Data;
using wms.business;
using wms.ids.dto;
using wms.infrastructure;

namespace wms.ids.business.Repositories
{
    internal class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<IEnumerable<ClientDto>> ReadAll()
        {
            try
            {
                var query = @"SELECT * FROM dbo.Client WHERE IsDeleted = 0 AND IsInit = 0";
                var data = await ReadRepository.Connection.QueryAsync<ClientDto>(query, commandType: CommandType.Text);

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
