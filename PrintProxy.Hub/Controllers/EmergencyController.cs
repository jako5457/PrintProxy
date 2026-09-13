using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintProxy.Hub.Services;

namespace PrintProxy.Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin,emergency_trigger")]
    public class EmergencyController : ControllerBase
    {

        private readonly EmergencyManager emergencyManager;

        public EmergencyController(EmergencyManager emergencyManager)
        {
            this.emergencyManager = emergencyManager;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            if (emergencyManager.GetState())
            {
                await emergencyManager.StopAlarmAsync();
                return Ok("Alarm Disabled");
            }
            else
            {
                await emergencyManager.TripAlarmAsync();
                return Ok("Alarm Tripped");
            }
        }
    }
}
