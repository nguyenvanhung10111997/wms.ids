using Newtonsoft.Json;
using System.Security.Claims;
using wms.ids.business.Interface;
using wms.ids.business.Services.Interfaces;
using wms.ids.dto;
using wms.infrastructure.Extensions;

namespace wms.ids.business.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(IUserRepository userRepository,
            IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IEnumerable<UserDto>> GetUsers() => await _userRepository.ReadAll();

        public async Task<bool> ValidateCredentials(string username, string password) => await _userRepository.Login(username, password);

        public async Task<UserDto> FindByUserName(string username) => await _userRepository.ReadByUsername(username);

        public async Task<UserDto> FindBySubjectId(object p) => await _userRepository.ReadByID(int.Parse(p.ToString()));

        public async Task<List<Claim>> GetClaimsForUser(UserDto user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(CustomClaimType.UserID, user.UserID.ToString()));
            claims.Add(new Claim(CustomClaimType.Username, user.Username));
            claims.Add(new Claim(CustomClaimType.FullName, user.FullName));
            claims.Add(new Claim(CustomClaimType.Avatar, string.IsNullOrWhiteSpace(user.Avatar) ? "" : user.Avatar));
            claims.Add(new Claim(CustomClaimType.UserTypeID, user.UserTypeID == null ? "0" : user.UserTypeID.Value.ToString()));

            var userRoles = await _userRoleRepository.ReadByUserId(user.UserID);

            if (userRoles != null && userRoles.Any())
            {
                claims.Add(new Claim(CustomClaimType.RoleIDs, JsonConvert.SerializeObject(userRoles.Select(i => i.RoleID))));
            }

            return claims;
        }

        public async Task<string> ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = await _userRepository.ReadByUsername(username);

            if (user == null)
            {
                return "Thông tin đăng nhập không đúng";
            }

            if (!user.IsActive || user.IsLocked || user.Password != oldPassword)
            {
                return "Thông tin đăng nhập không đúng";
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8 || !newPassword.Any(char.IsUpper) || !newPassword.Any(char.IsLower) || !newPassword.Any(char.IsDigit))
            {
                return "Mật khẩu mới không hợp lệ";
            }

            var executeResult = await _userRepository.ChangePassword(new UserChangePasswordDto
            {
                UserID = user.UserID,
                OldPassword = oldPassword,
                NewPassword = newPassword.ToMD5()
            });

            if (!executeResult)
            {
                return "Lỗi cập nhật mật khẩu";
            }

            return string.Empty;
        }
    }
}
