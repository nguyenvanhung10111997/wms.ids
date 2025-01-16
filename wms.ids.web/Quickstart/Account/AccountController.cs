// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using wms.ids.business.Configs;
using wms.ids.business.Services.Implement;
using wms.ids.business.Services.Interfaces;
using wms.ids.dto;
using wms.ids.web.Configuration;
using wms.ids.web.Extensions;
using wms.ids.web.Utilities;
using wms.infrastructure.Extensions;
using wms.infrastructure.Helpers;

namespace wms.ids.web
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private const string errorMessageValidCapcha = "Chứng thực capcha từ google không thành công.";
        private const string errorMessageMaxRequest = "Vượt quá số lần truy cập được cho phép.";
        private readonly IUserService _userService;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;
        private readonly IEventService _eventService;
        private readonly CaptchaVerificationService _captchaVerificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserService userService,
            CaptchaVerificationService cachaverification,
            IHttpContextAccessor httpAccesstor,
            IMemoryCache cachememory)
        {
            _identityServerInteractionService = interaction;
            _clientStore = clientStore;
            _authenticationSchemeProvider = schemeProvider;
            _eventService = events;
            _userService = userService;
            _captchaVerificationService = cachaverification;
            _httpContextAccessor = httpAccesstor;
            _cache = cachememory;
        }

        private bool IsMaxRequest(string ip)
        {
            var key = $"{ip}_{Guid.NewGuid()}";

            _cache.Set(key, true, DateTime.Now.AddSeconds(60 / IDSSetting.LoginTimePerMinute));
            int count = 0;
            var keys = _cache.GetKeys<string>();
            count = keys.Cast<string>().ToList().Where(i => i.Contains(ip)).Count();

            if (count > IDSSetting.LoginTimePerMinute)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var username = SessionHelper.GetString(_httpContextAccessor.HttpContext.Session, "LoginUser");
            var loginViewModel = await BuildLoginViewModelAsync(returnUrl);

            if (HttpContext.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(username))
            {
                SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "LoginUser", username == null ? "" : username);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Logout", "Account");
            }

            if (loginViewModel.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = loginViewModel.ExternalLoginScheme, returnUrl });
            }

            return View(loginViewModel);
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string returnUrl)
        {
            var username = SessionHelper.GetString(_httpContextAccessor.HttpContext.Session, "LoginUser");

            var loginViewModel = await BuildLoginViewModelAsync(returnUrl);

            if (string.IsNullOrEmpty(username))
            {
                SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "LoginUser", username == null ? "" : username);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Login", "Account");
            }

            return View(loginViewModel);
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel obj)
        {
            var username = SessionHelper.GetString(_httpContextAccessor.HttpContext.Session, "LoginUser");

            var errorMessage = await _userService.ChangePassword(username, obj.OldPass, obj.NewPass);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Json(new { Status = false, Message = errorMessage });
            }

            return Json(new { Status = true });
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                model.ReturnUrl = AppConfig.Common.DefaultHomePage;
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "LoginUser", model.Username);
                return Redirect(model.ReturnUrl);
            }

            if (IDSSetting.LockedUsers != null && IDSSetting.LockedUsers.FirstOrDefault(i => i.Username == model.Username) != null)
            {
                return Redirect("/PageNotFound.html");
            }

            var context = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);

            LoginViewModel loginViewModel;

            if (AppConfig.Common.IsCapchaRequired)
            {
                var isValid = await _captchaVerificationService.IsCaptchaValid(model.CapchaToken);

                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, errorMessageValidCapcha);
                    loginViewModel = await BuildLoginViewModelAsync(model);

                    return View(loginViewModel);
                }
            }

            var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (IDSSetting.UseLoginTimePerMinute)
            {
                if (IsMaxRequest(remoteIpAddress))
                {
                    ModelState.AddModelError(string.Empty, errorMessageMaxRequest);
                    loginViewModel = await BuildLoginViewModelAsync(model);

                    return View(loginViewModel);
                }
            }

            if (button.ToLower() != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _identityServerInteractionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.FindByUserName(model.Username);

                if (user == null)
                {
                    return View(null);
                }

                if (await _userService.ValidateCredentials(model.Username, model.Password?.ToMD5()))
                {
                    SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "LoginUser", model.Username);

                    if (user.IsRequireVerify)
                    {
                        return RedirectToAction("ChangePassword", "Account", new { ReturnUrl = model.ReturnUrl });
                    }

                    if (IDSSetting.UserLoginFails != null)
                    {
                        IDSSetting.UserLoginFails.RemoveAll(i => i.Username == user.Username);
                    }

                    _httpContextAccessor.HttpContext.Response.SetCookie("IDSLoginFailCount", "0", -1);

                    if (user.Enable2FA)
                    {
                        SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "AllowAccessTwoFactor", "1");
                        return RedirectToAction("Index", "TwoFactor", new { ReturnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        AuthenticationProperties props = null;
                        if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                        {
                            props = new AuthenticationProperties
                            {
                                IsPersistent = false,
                                ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                            };
                        };

                        var isuser = new IdentityServerUser(user.UserID.ToString())
                        {
                            DisplayName = user.Username
                        };

                        await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.UserID.ToString(), user.Username, clientId: context?.Client.ClientId));
                        await HttpContext.SignInAsync(isuser, props);

                        if (context != null)
                        {
                            if (context.IsNativeClient())
                            {
                                // The client is native, so this change in how to
                                // return the response is for better UX for the end user.
                                return this.LoadingPage("Redirect", model.ReturnUrl);
                            }

                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(model.ReturnUrl);
                        }

                        // request for a local page
                        if (string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            return Redirect(model.ReturnUrl);
                        }
                    }

                }
                else
                {
                    var userLoginFail = IDSSetting.UserLoginFails?.SingleOrDefault(i => i.Username == model.Username);
                    var cookie = _httpContextAccessor.HttpContext.Request.Cookies["IDSLoginFailCount"];

                    if (userLoginFail == null)
                    {
                        userLoginFail = new LoginCountUserModel()
                        {
                            Username = model.Username,
                            Count = 1
                        };

                        if (IDSSetting.UserLoginFails == null)
                        {
                            IDSSetting.UserLoginFails = new List<LoginCountUserModel>();
                        }

                        IDSSetting.UserLoginFails.Add(userLoginFail);
                        HttpContext.Response.SetCookie("IDSLoginFailCount", "1", 60);
                    }
                    else
                    {
                        userLoginFail.Count++;
                        HttpContext.Response.SetCookie("IDSLoginFailCount", userLoginFail.Count.ToString(), 60);
                    }

                    await _eventService.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                    ModelState.AddModelError(string.Empty, $"Thông tin đăng nhập không đúng. Số lần đăng nhập sai {userLoginFail.Count}");
                }
            }

            // something went wrong, show form with error
            loginViewModel = await BuildLoginViewModelAsync(model);
            return View(loginViewModel);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logoutViewModel = await BuildLogoutViewModelAsync(logoutId);

            if (!logoutViewModel.ShowLogoutPrompt)
            {
                return await Logout(logoutViewModel);
            }

            return View(logoutViewModel);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var loggedOutViewModel = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();
                SessionHelper.SetString(_httpContextAccessor.HttpContext.Session, "LoginUser", "");

                await _eventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (loggedOutViewModel.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = loggedOutViewModel.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, loggedOutViewModel.ExternalAuthenticationScheme);
            }
            return View("LoggedOut", loggedOutViewModel);
        }

        /// <summary>
        /// AccessDenied
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);

            if (context?.IdP != null && await _authenticationSchemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var isLocal = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                var loginViewModel = new LoginViewModel
                {
                    EnableLocalLogin = isLocal,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!isLocal)
                {
                    loginViewModel.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return loginViewModel;
            }

            var schemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;

            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var logoutViewModel = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                logoutViewModel.ShowLogoutPrompt = false;
                return logoutViewModel;
            }

            var context = await _identityServerInteractionService.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                logoutViewModel.ShowLogoutPrompt = false;
                return logoutViewModel;
            }

            return logoutViewModel;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await _identityServerInteractionService.GetLogoutContextAsync(logoutId);

            var loggedOutViewModel = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);

                    if (providerSupportsSignout)
                    {
                        if (loggedOutViewModel.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            loggedOutViewModel.LogoutId = await _identityServerInteractionService.CreateLogoutContextAsync();
                        }

                        loggedOutViewModel.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return loggedOutViewModel;
        }
    }
}
