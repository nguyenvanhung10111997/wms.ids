using IdentityServer4.Models;
using IdentityServer4.Validation;
using wms.ids.business.Services.Interfaces;

namespace wms.ids.web
{
    public class IDSPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var userService = serviceScope.ServiceProvider.GetService<IUserService>();
                var user = await userService.FindByUserName(context.UserName);

                if (user != null && user.Password == context.Password)
                {
                    var claims = await userService.GetClaimsForUser(user);

                    context.Result = new GrantValidationResult(
                        subject: user.UserID.ToString(),
                        authenticationMethod: GrantType.ResourceOwnerPassword,
                        claims: claims);
                    return;
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        } 
    }
}