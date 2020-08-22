using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PtCounterWeb.Models
{
    public class ReportFromClient
    {
        public string Moniker { get; set; }
        public long dateTime { get; set; }

        public int Count { get; set; }

        public long Duration { get; set; }
    }
}
