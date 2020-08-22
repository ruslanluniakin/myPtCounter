using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PtCounterWeb.Models
{
    public class Report
    {
        public int Id { get; set; }

        public int IdDevice { get; set; }

        public DateTime dateTime { get; set; }

        public int Count { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
