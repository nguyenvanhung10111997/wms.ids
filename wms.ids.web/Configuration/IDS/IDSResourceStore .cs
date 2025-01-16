using IdentityServer4.Models;
using IdentityServer4.Stores;
using wms.ids.business.Services;

namespace wms.ids.web
{
    public class IDSResourceStore : IResourceStore
    {
        private readonly IClientService _clientService;

        public IDSResourceStore(IClientService clientService)
        {
            _clientService = clientService;
        }

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            return Task.FromResult(new ApiResource
            {
            });
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {          
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var result = new List<ApiResource>();
            result.Add(new ApiResource("api", "scope-api"));
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {

            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var result = new List<ApiResource>();
            result.Add(new ApiResource("api", "scope-api"));
            return Task.FromResult(result.AsEnumerable());
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null)
            {
                throw new NullReferenceException();
            }

            var scopes = await _clientService.ReadScope();
            return scopes.Where(i => scopeNames.Contains(i.Name));
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {

            var result = new List<IdentityResource>();
            result.Add(new IdentityResource("api", new List<string> { "claim1", "claim2" }));
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(_clientService.GetIdentityResources().Where(i => scopeNames.Contains(i.Name)));
            
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResources = new List<ApiResource>();
            apiResources.Add(new ApiResource { Name = "name", DisplayName = "displayname" });
            var identityResources = new List<IdentityResource>();
            identityResources.Add(new IdentityResource { Name = "idenname", DisplayName = "displayname" });
            var apiScopes = new List<ApiScope>();
            apiScopes.Add(new ApiScope { Name = "name1", DisplayName = "displayname" });

            var result = new Resources(identityResources, apiResources, apiScopes);
            return Task.FromResult(result);
        }
    }
}
