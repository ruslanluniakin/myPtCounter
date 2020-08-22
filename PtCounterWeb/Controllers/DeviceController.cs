using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PtCounterWeb.Models;

namespace PtCounterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        ApplicationContext db;
        public DeviceController(ApplicationContext context)
        {
            db = context;
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Device>> Post(Device device)
        {
            if (device == null)
            {
                return BadRequest();
            }
            if(db.Devices.FirstOrDefault(x => x.Moniker == device.Moniker) != null)
            {
                return Ok(device);
            }

            db.Devices.Add(device);
            await db.SaveChangesAsync();
            return Ok(device);
        }
    }
}
