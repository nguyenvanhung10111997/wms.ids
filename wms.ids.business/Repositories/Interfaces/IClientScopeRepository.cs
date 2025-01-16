using wms.ids.dto;

namespace wms.ids.business.Repositories
{
    public interface IClientScopeRepository : IDisposable
    {
        Task<IEnumerable<ClientScopeDto>> ReadAll();
    }
}
