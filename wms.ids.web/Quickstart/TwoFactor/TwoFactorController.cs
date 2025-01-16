using Google.Authenticator;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using wms.ids.business;
using wms.ids.business.Configs;
using wms.ids.business.Services.Interfaces;
using wms.ids.dto;
using wms.ids.web;
using wms.ids.web.Configuration;
using wms.infrastructure.Helpers;

namespace CoC.IDS.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class TwoFactorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public TwoFactorController(IHttpContextAccessor context, IUserService userService)
        {
            _httpContextAccessor = context;
            _userService = userService;


        }
        public IActionResult Index(string returnUrl = null)
        {
            var userIdentity = _httpContextAccessor.HttpContext.User;

            if (userIdentity == null)
            {
                return Redirect(Url.Content("/"));
            }

            if (userIdentity.Identity.IsAuthenticated)
            {
                return RedirectToAction("Account", "Logout");
            }

            ViewBag.ReturnURL = returnUrl;
            return View("Index");
        }

        [HttpPost]
        [Route("identity/TwoFactor/Verify")]
        public async Task<IActionResult> Verify()
        {

            var loginUser = SessionHelper.GetString(_httpContextAccessor.HttpContext.Session, "LoginUser");

            if (string.IsNullOrEmpty(loginUser))
            {
                return Redirect(Url.Content("~/account/login"));
            }

            var user = await _userService.FindByUserName(loginUser);

            if (user == null)
            {
                return Redirect(Url.Content("/"));
            }

            var token = _httpContextAccessor.HttpContext.Request.Form["passcode"];
            var returnURL = _httpContextAccessor.HttpContext.Request.Form["returnurl"];

            if (string.IsNullOrEmpty(returnURL))
            {
                returnURL = AppConfig.Common.DefaultHomePage;
            }

            var twoFactorAuthenticator = new TwoFactorAuthenticator();

            var isValid = twoFactorAuthenticator.ValidateTwoFactorPIN(user.Secret2FAKey, token, new TimeSpan(0, 0, IDSSetting.TFATimeTolerance));

            if (isValid)
            {
                var isuser = new IdentityServerUser(user.UserID.ToString())
                {
                    DisplayName = user.Username
                };

                SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "TwoFAAttempts", "");
                await _httpContextAccessor.HttpContext.SignInAsync(isuser, null);

                return Redirect(returnURL);
            }
            else
            {
                var count = string.IsNullOrEmpty(SessionHelper.GetString(_httpContextAccessor.HttpContext.Session, "TwoFAAttempts")) 
                    ? 5 : Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("TwoFAAttempts")) - 1;
                
                if (count == 0)
                {
                    if (IDSSetting.LockedUsers == null)
                    {
                        IDSSetting.LockedUsers = new List<LockedUserModel>();
                    }
                    var lockedUser = IDSSetting.LockedUsers.SingleOrDefault(i => i.UserID == user.UserID);

                    if (lockedUser == null)
                    {
                        IDSSetting.LockedUsers.Add(new LockedUserModel()
                        {
                            UserID = user.UserID,
                            Username = user.Username,
                            LockedDate = DateTime.Now
                        });
                    }
                    else
                    {
                        lockedUser.LockedDate = DateTime.Now;
                    }

                    SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "TwoFAAttempts", "");
                    var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                    return Redirect("/PageNotFound.html");

                }
                else
                {
                    SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "TwoFAAttempts", count.ToString());
                }
            }
            return RedirectToAction("Index", new { ReturnUrl = returnURL });
        }
    }
}