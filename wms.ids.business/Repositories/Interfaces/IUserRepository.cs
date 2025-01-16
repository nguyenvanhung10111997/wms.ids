using wms.ids.dto;

namespace wms.ids.business.Interface
{
    public interface IUserRepository: IDisposable
    {
        Task<IEnumerable<UserDto>> ReadAll();
        Task<UserDto> ReadByID(int userId);
        Task<UserDto> ReadByUsername(string username);
        Task<bool> Login(string username, string password);
        Task<bool> ChangePassword(UserChangePasswordDto obj);
    }
}
