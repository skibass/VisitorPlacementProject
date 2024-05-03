using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces.Logic
{
    public interface IEventEditService
    {
        public Part AddPart(int eventId); 
        public Row AddRow(int partId);
        public Chair AddChair(int rowId, int eventId);
    }
}
