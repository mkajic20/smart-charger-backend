using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCharger.Business.Interfaces;

namespace SmartCharger.Controllers
{
    [Route("api/admin/")]
    [ApiController]
    public class ChargerController : ControllerBase
    {
        private readonly IChargerService _chargerService;

        public ChargerController(IChargerService chargerService)
        {
            _chargerService = chargerService;
        }
    }
}
