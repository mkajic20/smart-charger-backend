using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IUserService
    {
        Task<UsersResponseDTO> GetAllUsers(int page, int pageSize, string search);
        Task<SingleUserResponseDTO> GetUserById(int userId);
        Task<SingleUserResponseDTO> UpdateRole(int userId, int roleId);
        Task<SingleUserResponseDTO> UpdateActiveStatus(int userId);
    }
}
