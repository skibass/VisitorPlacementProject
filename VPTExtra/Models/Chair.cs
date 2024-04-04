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
        public User User { get; set; }

        public Chair(string name) 
        {
            Name = name;
        }
        public Chair() { }
    }
}
