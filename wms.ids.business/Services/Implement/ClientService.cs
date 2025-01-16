using IdentityServer4;
using IdentityServer4.Models;
using System.Data;
using wms.ids.business.Repositories;
using wms.ids.business.Ultilities;
using wms.ids.dto;

namespace wms.ids.business.Services.Implement
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IScopeRepository _scopeRepository;
        private readonly IClientScopeRepository _clientScopeRepository;

        public ClientService(IClientRepository clientRepository, IScopeRepository scopeRepository, IClientScopeRepository clientScopeRepository)
        {
            _clientRepository = clientRepository;
            _scopeRepository = scopeRepository;
            _clientScopeRepository = clientScopeRepository;
        }

        public async Task<IEnumerable<Client>> ReadClient()
        {
            var clients = await _clientRepository.ReadAll();
            var scopes = await _scopeRepository.ReadAll();
            var clientScopes = await _clientScopeRepository.ReadAll();

            var result = (from c in clients
                          select new Client
                          {
                              AccessTokenLifetime = c.AccessTokenLifetime == null ? 3600 : c.AccessTokenLifetime.Value,
                              AccessTokenType = c.AccessTokenType == null ? AccessTokenType.Jwt : (AccessTokenType)c.AccessTokenType,
                              AllowAccessTokensViaBrowser = c.AllowAccessTokensViaBrowser == null ? false : c.AllowAccessTokensViaBrowser.Value,
                              AllowedScopes = AddDefaultScopeToListScopes(GetScopeByClientID(c.ID, scopes, clientScopes)),
                              AllowRememberConsent = c.AllowRememberConsent == null ? true : c.AllowRememberConsent.Value,
                              AlwaysSendClientClaims = c.AlwaysSendClientClaims == null ? false : c.AlwaysSendClientClaims.Value,
                              AuthorizationCodeLifetime = c.AuthorizationCodeLifetime == null ? 300 : c.AuthorizationCodeLifetime.Value,
                              ClientId = c.ClientID.ToString(),
                              ClientName = c.ClientName,
                              ClientSecrets = c.ClientSecret == null ? null : c.ClientSecret.Split(';').Select(i => new Secret(i.Sha256())).ToList(),
                              ClientUri = c.ClientUri,
                              Enabled = c.Enable == null ? true : c.Enable.Value,
                              EnableLocalLogin = c.EnableLocalLogin == null ? true : c.EnableLocalLogin.Value,
                              AllowedGrantTypes = GetGrantTypeViaFlowID(c.Flow),
                              IdentityTokenLifetime = c.IdentityTokenLifetime == null ? 300 : c.IdentityTokenLifetime.Value,
                              IncludeJwtId = c.IncludeJwtId == null ? false : c.IncludeJwtId.Value,
                              LogoUri = c.LogoUri,
                              PostLogoutRedirectUris = c.PostLogoutRedirectUri == null ? null : c.PostLogoutRedirectUri.Split(';').Select(i => i.Trim()).ToList(),
                              RedirectUris = c.RedirectUri == null ? null : c.RedirectUri.Split(';').Select(i => i.Trim()).ToList(),
                              RefreshTokenExpiration = c.RefreshTokenExpiration == null ? TokenExpiration.Sliding : (TokenExpiration)c.RefreshTokenExpiration,
                              RefreshTokenUsage = c.RefreshTokenUsage == null ? TokenUsage.ReUse : (TokenUsage)c.RefreshTokenUsage,
                              RequireConsent = c.RequireConsent == null ? true : c.RequireConsent.Value,
                              SlidingRefreshTokenLifetime = c.SlidingRefreshTokenLifetime == null ? 1296000 : c.SlidingRefreshTokenLifetime.Value,
                              AbsoluteRefreshTokenLifetime = c.AbsoluteRefreshTokenLifetime == null ? 2592000 : c.AbsoluteRefreshTokenLifetime.Value,
                              UpdateAccessTokenClaimsOnRefresh = c.UpdateAccessTokenClaimsOnRefresh == null ? false : c.UpdateAccessTokenClaimsOnRefresh.Value,
                              RequirePkce = false,
                              AllowOfflineAccess = true,
                              AlwaysIncludeUserClaimsInIdToken = true
                          }).ToList();

            foreach (var item in result)
            {
                item.AllowedScopes.ToList().AddRange(PropertyHelper.GetConstantProperties(typeof(IdentityServerConstants.StandardScopes)).Select(i => i.GetValue(null).ToString()));
            }

            return result;
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public async Task<IEnumerable<ApiScope>> ReadScope()
        {
            var scopesToReturn = new List<ApiScope>();

            var scopes = await _scopeRepository.ReadAll();

            scopes.ToList().ForEach(s =>
            {
                scopesToReturn.Add(new ApiScope
                {
                    Description = s.Description,
                    DisplayName = s.DisplayName,
                    Enabled = s.Enable,
                    Name = s.ScopeName,
                    UserClaims = PropertyHelper.GetConstantProperties(typeof(CustomClaimType)).Select(i => i.GetValue(null).ToString()).ToList()
                });
            });

            return scopesToReturn;
        }

        private ICollection<string> GetGrantTypeViaFlowID(int flowId)
        {
            switch (flowId)
            {
                case 0:
                    return GrantTypes.Code;
                case 1:
                    return GrantTypes.Implicit;
                case 2:
                    return GrantTypes.Hybrid;
                case 3:
                    return GrantTypes.ClientCredentials;
                case 4:
                    return GrantTypes.ResourceOwnerPassword;
                case 5:
                    return GrantTypes.ImplicitAndClientCredentials;
                case 6:
                    return GrantTypes.CodeAndClientCredentials;
                default:
                    return GrantTypes.DeviceFlow;
            }

        }

        private ICollection<string> AddDefaultScopeToListScopes(IEnumerable<string> scopes)
        {
            var list = scopes.ToList();
            list.AddRange(new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess
            });
            return list;
        }

        private IEnumerable<string> GetScopeByClientID(int clientId, IEnumerable<ScopeDto> scopes, IEnumerable<ClientScopeDto> clientScopes)
        {
            var result = from cs in clientScopes
                         join s in scopes on cs.ScopeID equals s.ScopeID
                         where cs.ClientID == clientId
                         select s.ScopeName;

            return result;
        }

        #region Destructor
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _clientRepository?.Dispose();
                    _scopeRepository?.Dispose();
                    _clientScopeRepository?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ClientService()
        {
            Dispose(false);
        }

        #endregion
    }
}
