using SmartCharger.Business.DTOs;

namespace SmartCharger.Business.Interfaces
{
    public interface IUserService
    {
        Task<UsersResponseDTO> GetAllUsers(int page, int pageSize);
        Task<SingleUserResponseDTO> GetUserById(int userId);
        Task<SingleUserResponseDTO> UpdateRole(int userId);
        Task<SingleUserResponseDTO> UpdateActiveStatus(int userId);
    }
}
