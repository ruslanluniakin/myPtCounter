using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PtCounterWeb.Models
{
    public class DeviceReportsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Report> Reports { get; set; }

        public int Count { get; set; }
        public DeviceReportsModel(Device device, List<Report> reports)
        {
            Id = device.Id;
            Name = device.Name;
            Reports = reports;
            Count = 0;
            reports.ForEach(x => Count += x.Count);
        }
    }
}
