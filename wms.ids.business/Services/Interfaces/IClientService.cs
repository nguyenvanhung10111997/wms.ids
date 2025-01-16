using IdentityServer4.Models;

namespace wms.ids.business.Services
{
    public interface IClientService : IDisposable
    {
        Task<IEnumerable<Client>> ReadClient();
        Task<IEnumerable<ApiScope>> ReadScope();
        IEnumerable<IdentityResource> GetIdentityResources();
    }
}
