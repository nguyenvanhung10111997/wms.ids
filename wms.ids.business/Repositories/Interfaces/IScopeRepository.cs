using wms.ids.dto;

namespace wms.ids.business.Repositories
{
    public interface IScopeRepository : IDisposable
    {
        Task<IEnumerable<ScopeDto>> ReadAll();
    }
}
