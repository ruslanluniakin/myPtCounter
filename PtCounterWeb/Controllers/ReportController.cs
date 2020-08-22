using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PtCounterWeb.Models;

namespace PtCounterWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        ApplicationContext db;
        public ReportController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<ActionResult<ReportFromClient>> Post(ReportFromClient value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var d = await db.Devices.FirstOrDefaultAsync(x => x.Moniker == value.Moniker);

            if(d == null)
            {
                return NotFound();
            }

            db.Reports.Add(new Report()
            {
                IdDevice = d.Id,
                Count = value.Count,
                Duration = new TimeSpan(value.Duration),
                dateTime = new DateTime(value.dateTime)
            });

            await db.SaveChangesAsync();

            return Ok(value);
        }
    }
}
