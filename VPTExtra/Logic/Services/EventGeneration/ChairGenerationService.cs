using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.EventGeneration
{
    public class ChairGenerationService
    {
        public Chair GenerateChair(string chairName)
        {
            Chair chair = new();
            chair.Name = chairName;

            return chair;
        }
    }
}
