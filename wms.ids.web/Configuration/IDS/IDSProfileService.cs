using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using wms.ids.business.Services.Interfaces;

namespace wms.ids.web.Configuration.IDS
{
    public class IDSProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public IDSProfileService(IUserService userservice)
        {
            _userService = userservice;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userService.FindBySubjectId(context.Subject.GetSubjectId());
            context.IsActive = user == null ? false : user.IsActive;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userService.FindBySubjectId(context.Subject.GetSubjectId());

            if (user != null)
            {
                context.IssuedClaims = await _userService.GetClaimsForUser(user);
            }
        }
    }
}
