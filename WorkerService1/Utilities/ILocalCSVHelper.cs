using System.Collections.Generic;
using WorkerService1.Models;

namespace WorkerService1.Utilities
{
    public interface ILocalCSVHelper
    {
        void SaveCSVData(List<CsvRow> records, CsvRow newRow, string olxRentCsv);
        List<CsvRow> ReadCSVOldData(string olxRentCsv);
    }
}