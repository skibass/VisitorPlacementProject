using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public interface IEventGenerationService
    {
        public Event GenerateEvent(Event currentEvent, int amountParts, int amountRows);


    }
}
