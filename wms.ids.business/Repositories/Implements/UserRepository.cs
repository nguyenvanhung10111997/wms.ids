using wms.ids.business.Interface;
using Dapper;
using System.Data;
using wms.business;
using wms.ids.dto;
using wms.infrastructure;

namespace wms.ids.business.Implement
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<IEnumerable<UserDto>> ReadAll()
        {
            try
            {
                var query = @"SELECT * FROM dbo.[User] WHERE IsDeleted = 0 AND IsInit = 0";
                var data = await ReadRepository.Connection.QueryAsync<UserDto>(query, commandType: CommandType.Text);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDto> ReadByID(int userId)
        {
            try
            {
                var query = @$"SELECT * FROM dbo.[User] WHERE IsDeleted = 0 AND IsInit = 0 AND UserID = {userId}";
                var result = await ReadRepository.Connection.QueryFirstOrDefaultAsync<UserDto>(query, commandType: CommandType.Text);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDto> ReadByUsername(string username)
        {
            try
            {
                var query = @$"SELECT * FROM dbo.[User] WHERE IsDeleted = 0 AND IsInit = 0 AND Username = '{username}'";
                var result = await ReadRepository.Connection.QueryFirstOrDefaultAsync<UserDto>(query, commandType: CommandType.Text);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            var user = await ReadByUsername(username);

            if (user == null)
            {
                return false;
            }

            return user.Password.ToLower() == password.ToLower();
        }

        public async Task<bool> ChangePassword(UserChangePasswordDto obj)
        {
            try
            {
                var query = @$"UPDATE dbo.[User] 
                    SET [Password] = {obj.NewPassword}, 
                        LastChangePassword = GETDATE(), 
                        UpdatedUser = 1, 
                        UpdatedDate = GETDATE(), 
                        IsRequireVerify = 0 
                    WHERE UserID = {obj.UserID}";

                var executeResult = await Repository.Connection.ExecuteAsync(query, commandType: CommandType.Text);

                return executeResult > 0;
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
