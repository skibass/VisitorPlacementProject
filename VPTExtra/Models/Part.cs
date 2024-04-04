using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Row> Rows { get; set; }

        public Part(string name, List<Row> rows) 
        { 
            Name = name;
            Rows = rows;
        }
        public Part() { }
    }
}
