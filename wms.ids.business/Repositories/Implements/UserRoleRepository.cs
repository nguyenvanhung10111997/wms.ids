using wms.ids.business.Interface;
using Dapper;
using System.Data;
using wms.business;
using wms.ids.dto;
using wms.infrastructure;

namespace wms.ids.business.Implement
{
    internal class UserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public UserRoleRepository(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<IEnumerable<UserRoleDto>> ReadByUserId(int userId)
        {
            try
            {
                var query = $"SELECT * FROM dbo.UserRole WHERE IsDeleted = 0 AND UserID = {userId};";
                var result = await ReadRepository.Connection.QueryAsync<UserRoleDto>(query, commandType: CommandType.Text);

                return result;
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
