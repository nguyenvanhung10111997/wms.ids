using System.Security.Claims;
using wms.ids.dto;

namespace wms.ids.business.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<bool> ValidateCredentials(string username, string password);
        Task<UserDto> FindByUserName(string username);
        Task<UserDto> FindBySubjectId(object p);
        Task<List<Claim>> GetClaimsForUser(UserDto user);
        Task<string> ChangePassword(string username, string oldPassword, string newPassword);
    }
}
