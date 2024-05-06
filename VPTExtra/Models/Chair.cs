using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Chair
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        //public User User { get; set; }
        public int Uid { get; set; }


        public Chair(string name, int chairNumber) 
        {
            Name = name;
            Number = chairNumber;
        }
        public Chair() { }
    }
}
