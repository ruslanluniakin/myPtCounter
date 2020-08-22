using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PtCounterWeb.Models;

namespace PtCounterWeb.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;

            //var d = db.Devices;

            //db.Devices.RemoveRange(d);

            //var r = db.Reports;

            //db.Reports.RemoveRange(r);

            //db.Devices.Add(new Device() { Name = "Device 1" });
            //db.SaveChanges();
            //var d = db.Devices.ToList();

            //db.Reports.Add(new Report()
            //{
            //    IdDevice = d.Last().Id,
            //    DateTime = new System.DateTime(0),
            //    Duration = new System.TimeSpan(1, 3, 4),
            //    Count = 2
            //});

            //db.Reports.Add(new Report()
            //{
            //    IdDevice = d.Last().Id,
            //    DateTime = new System.DateTime(0),
            //    Duration = new System.TimeSpan(1, 3, 4),
            //    Count = 6
            //});

            //db.Devices.Add(new Device() { Name = "Device 2" });
            //db.SaveChanges();
            //var r = db.Devices.ToList();

            //db.Reports.Add(new Report()
            //{
            //    IdDevice = r.Last().Id,
            //    DateTime = new System.DateTime(0),
            //    Duration = new System.TimeSpan(1, 3, 4),
            //    Count = 3
            //});

            //db.Reports.Add(new Report()
            //{
            //    IdDevice = r.Last().Id,
            //    DateTime = new System.DateTime(0),
            //    Duration = new System.TimeSpan(1, 3, 4),
            //    Count = 2
            //});
            //db.Reports.Add(new Report()
            //{
            //    IdDevice = r.Last().Id,
            //    DateTime = new System.DateTime(0),
            //    Duration = new System.TimeSpan(1, 3, 4),
            //    Count = 7
            //});

            //db.SaveChanges();
        }

        public async Task<IActionResult> Index()
        {
            var devices = await db.Devices.ToListAsync();
            var reports = await db.Reports.ToListAsync();

            var modelDeviceReport = new List<DeviceReportsModel>();


            foreach (var item in devices)
            {
                modelDeviceReport.Add(new DeviceReportsModel(item, reports.Where(z => z.IdDevice == item.Id).ToList()));
            }

            return View(modelDeviceReport);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
