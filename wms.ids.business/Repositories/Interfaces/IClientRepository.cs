using wms.ids.dto;

namespace wms.ids.business.Repositories
{
    public interface IClientRepository : IDisposable
    {
        Task<IEnumerable<ClientDto>> ReadAll();
    }
}
