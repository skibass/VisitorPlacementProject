using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPTEventApi.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Row> Rows { get; set; }
    }
}
