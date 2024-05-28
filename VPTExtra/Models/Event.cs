using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Location { get; set; }
        public int VisitorLimit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ChairsReserved { get; set; }
        public string ChairNames { get; set; }
        public string EventQr { get; set; }
        public List<Part> Parts { get; set; }

        public Event(DateTime? startDate, DateTime? endDate, List<Part> parts, int visitorLimit) 
        {
            Random rand = new();

            Location = "Locatie " + rand.Next(0, 10000);
            StartDate = startDate;
            EndDate = endDate;
            Parts = parts;
            VisitorLimit = visitorLimit;
        }
        public Event ()
        {

        }
    }
}
