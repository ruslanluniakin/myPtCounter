using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtCounter.model
{
    public class ReportFromClient
    {
        public string Moniker { get; set; }

        public long dateTime { get; set; }

        public int Count { get; set; }

        public long Duration { get; set; }
    }
}
