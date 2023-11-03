
using Microsoft.EntityFrameworkCore;
using SmartCharger.Business.DTOs;
using SmartCharger.Business.Interfaces;
using SmartCharger.Data;
using SmartCharger.Data.Entities;

namespace SmartCharger.Business.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(SmartChargerContext context) : base(context)
        {
        }
        public async Task<UsersResponseDTO> GetAllUsers()
        {
            try
            {
                var users = await _context.Users
                  .OrderBy(u => u.Id)
                  .Select(u => new UserDTO
                  {
                      Id = u.Id,
                      FirstName = u.FirstName,
                      LastName = u.LastName,
                      Email = u.Email,
                      Active = u.Active,
                      RoleId = u.RoleId
                  }).ToListAsync();

                return new UsersResponseDTO
                {
                    Success = true,
                    Message = "List of users.",
                    Users = users
                };
            }
            catch (Exception ex)
            {
                return new UsersResponseDTO
                {
                    Success = false,
                    Message = "An error occurred: " + ex.Message + ".",
                    Users = null
                };
            }
        }

        public async Task<SingleUserResponseDTO> GetUserById(int userId)
        {
            try
            {

                var userEntity = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
                if (userEntity == null)
                {
                    return new SingleUserResponseDTO
                    {
                        Success = false,
                        Message = "User not found.",
                        User = null
                    };
                }
                var userDTO = MakeUserDTO(userEntity);
                return new SingleUserResponseDTO
                {
                    Success = true,
                    Message = $"User found.",
                    User = userDTO
                };
            }
            catch (Exception ex)
            {
                return new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "An error occurred: " + ex.Message,
                    User = null
                };
            }
        }



        public async Task<SingleUserResponseDTO> UpdateActiveStatus(int userId)
        {
            try
            {
                var userEntity = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

                if (userEntity == null)
                {
                    return new SingleUserResponseDTO
                    {
                        Success = false,
                        Message = "User not found.",
                        User = null
                    };
                }

                userEntity.Active = !userEntity.Active;

                await _context.SaveChangesAsync();
                var userDTO = MakeUserDTO(userEntity);

                return new SingleUserResponseDTO
                {
                    Success = true,
                    Message = (bool)userEntity.Active
                        ? $"User {userEntity.FirstName} {userEntity.LastName} is activated."
                        : $"User {userEntity.FirstName} {userEntity.LastName} is deactivated.",
                    User = userDTO
                };
            }
            catch (Exception ex)
            {
                return new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "An error occurred: " + ex.Message,
                    User = null
                };
            }
        }

        private UserDTO MakeUserDTO(User userEntity)
        {
            return new UserDTO
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Active = userEntity.Active,
                Email = userEntity.Email,
                RoleId = userEntity.RoleId
            };
        }

        public async Task<SingleUserResponseDTO> UpdateRole(int userId)
        {
            try
            {
                var userEntity = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

                if (userEntity == null)
                {
                    return new SingleUserResponseDTO
                    {
                        Success = false,
                        Message = "User not found.",
                        User = null
                    };
                }

                userEntity.RoleId = (userEntity.RoleId == 1) ? 2 : 1;

                await _context.SaveChangesAsync();

                var userDTO = MakeUserDTO(userEntity);
                return new SingleUserResponseDTO
                {
                    Success = true,
                    Message = $"User {userEntity.FirstName} {userEntity.LastName}'s role has been updated.",
                    User = userDTO
                };
            }
            catch (Exception ex)
            {
                return new SingleUserResponseDTO
                {
                    Success = false,
                    Message = "An error occurred: " + ex.Message,
                    User = null
                };
            }
        }

    }
}
