using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Row
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<Chair> Chairs { get; set; }

        public Row(string name, List<Chair> chairs, int rowNumber) 
        { 
            Name = name;
            Chairs = chairs;
            Number = rowNumber;
        }
        public Row() { }
    }
}
