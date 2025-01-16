using wms.ids.dto;

namespace wms.ids.business.Interface
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRoleDto>> ReadByUserId(int userId);
    }
}
