using SmartCharger.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharger.Business.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);
    }
}
