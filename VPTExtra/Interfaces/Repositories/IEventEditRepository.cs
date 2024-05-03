using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAcces.Repositories
{
    public interface IEventEditRepository
    {
        public Part AddPart(int eventId);
        public Row AddRow(int partId);
        public Chair AddChair(int rowId, int eventId);
    }
}
