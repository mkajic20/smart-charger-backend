
using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IRoleService
    {
       Task<RoleResponseDTO> GetAllRoles();
    }
}
