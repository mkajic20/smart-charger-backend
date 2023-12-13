
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var roles = await _context.Roles
                    .OrderBy(r => r.Id)
                    .ToListAsync();

                var roleDTOs = roles.Select(r => new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();

                return new RoleResponseDTO
                {
                    Success = true,
                    Message = "List of roles.",
                    Roles = roleDTOs
                };
            }
            catch (Exception ex)
            {
                return new RoleResponseDTO
                {
                    Success = false,
                    Message = "An error occurred.",
                    Error = ex.Message,
                    Roles = null
                };
            }
        }
    }
}
