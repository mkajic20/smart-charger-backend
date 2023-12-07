
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;

namespace SmartCharger.Business.Services
{
    public class RoleService : GenericService<Role>,IRoleService
    {
        public RoleService(SmartChargerContext context) : base(context)
        {
        }

        public async Task<RoleResponseDTO> GetAllRoles()
        {
            throw new NotImplementedException();
        }
    }
}
