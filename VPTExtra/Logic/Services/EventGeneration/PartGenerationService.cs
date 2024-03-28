using Interfaces.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.EventGeneration
{
    public class PartGenerationService
    {
        private readonly RowGenerationService _rowGeneration;

        public PartGenerationService(RowGenerationService rowGenerationService)
        {
            _rowGeneration = rowGenerationService;
        }
        public Part GeneratePart(int amountRowsPerPart, string partName, ref int chairsLeft)
        {
            int rowNumber = 1;
            int defaultAmountOfRows = 6;

            Part part = new();
            part.Name = partName;
            part.Rows = new List<Row>();

            if (amountRowsPerPart == 0)
            {
                for (int i = 0; i < defaultAmountOfRows; i++)
                {
                    if (chairsLeft > 0)
                    {
                        part.Rows.Add(_rowGeneration.GenerateRow(partName + rowNumber, ref chairsLeft));
                        rowNumber++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < amountRowsPerPart; i++)
                {
                    if (chairsLeft > 0)
                    {
                        part.Rows.Add(_rowGeneration.GenerateRow(partName + rowNumber, ref chairsLeft));
                        rowNumber++;
                    }
                }
            }

            return part;
        }
    }
}
