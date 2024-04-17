using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces.Logic
{
    public interface IEventGenerationService
    {
        public Event GenerateEvent(Event currentEvent, int amountParts, int amountRows);
    }
}
