using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.EventGeneration
{
    public class RowGenerationService
    {
        private readonly ChairGenerationService _chairGeneration;

        public RowGenerationService(ChairGenerationService chairGeneration)
        {
            _chairGeneration = chairGeneration;
        }
        public Row GenerateRow(string rowName, ref int chairsLeft)
        {
            int chairNumber = 1;
            int chairsThisRow = 0;

            Row row = new();
            row.Name = rowName;
            row.Chairs = new List<Chair>();

            for (int i = 0; i < chairsLeft; i++)
            {
                if (chairsThisRow < 5)
                {
                    row.Chairs.Add(_chairGeneration.GenerateChair(row.Name + "-" + chairNumber));
                    chairNumber++;
                    chairsThisRow++;
                }
                else
                {
                    break;
                }
            }
            chairsLeft = chairsLeft - chairsThisRow;

            return row;
        }
    }
}
