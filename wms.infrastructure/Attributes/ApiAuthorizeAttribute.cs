using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using wms.infrastructure.Configurations;
using wms.infrastructure.Models;

namespace wms.infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContext;

        private bool IsAuthorizeAction { get; set; }

        public ApiAuthorizeAttribute(bool AuthorizeAction = false)
        {
            IsAuthorizeAction = AuthorizeAction;
            _httpContext = Engine.ContainerManager.Resolve<IHttpContextAccessor>();
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            StringValues token = string.Empty;

            if (filterContext == null)
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }

            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out token))
            {
                token = token.ToString().Replace("Bearer ", "");
            }

            var claim = ValidateToken(token);// (filterContext.HttpContext.User.Identity as ClaimsIdentity).Claims.ToList();

            if (claim.Count == 0)
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }


            var userPrincipal = new UserPrincipal(claim);
            filterContext.HttpContext.User = userPrincipal;

            if (IsAuthorizeAction)
            {
                return;
            }

            var obj = (HttpMethodActionConstraint)filterContext.ActionDescriptor.ActionConstraints.FirstOrDefault();
            string method = obj.HttpMethods.FirstOrDefault();
            string actionName = filterContext.ActionDescriptor.RouteValues["action"] ?? "";
            string controllerName = filterContext.ActionDescriptor.RouteValues["controller"] ?? "";

            if (!AppCoreConfig.Common.DisableAuthen)
            {
                if (!IsAuthorize(controllerName, actionName, method, userPrincipal.RoleIDs.ToList()))
                {
                    filterContext.Result = new ForbidResult();
                    return;
                }
            }
        }

        public bool IsAuthorize(string controller, string action, string method, List<int> RoleIDs)
        {
            if (RoleIDs == null) return false;

            controller = controller.ToLower();
            action = action.ToLower();
            method = method.ToLower();
            //var roleFunctionResult = from r in AppPermission.Data
            //                         where RoleIDs.Contains(r.RoleID) && r.APIAction.ToLower() == action && r.APIController.ToLower() == controller && r.APIMethod.ToLower() == method
            //                         select r;
            //return roleFunctionResult.Any();

            return true;
        }

        private List<Claim> ValidateToken(string token)
        {
            try
            {
                var cert = new X509Certificate2(Convert.FromBase64String(AppCoreConfig.JWT.Base64PublicKey));
                var parameters = new TokenValidationParameters
                {
                    //ValidAudiences = new List<string> { AppCoreConfig.Common.ClientName },
                    AudienceValidator = (a, b, c) => true,
                    ValidIssuer = AppCoreConfig.JWT.Issuer,
                    IssuerSigningKeyResolver = (string token1, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) => new List<X509SecurityKey> { new X509SecurityKey(cert) }
                };

                SecurityToken jwt;
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out jwt);

                return principal.Claims.ToList();
            }
            catch (Exception ex)
            {
                return new List<Claim>();
            }

        }

    }
}
