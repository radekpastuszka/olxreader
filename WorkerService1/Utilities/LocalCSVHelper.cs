using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using WorkerService1.Models;

namespace WorkerService1.Utilities
{
    public class LocalCSVHelper : ILocalCSVHelper
    {
        public void SaveCSVData(List<CsvRow> records, CsvRow newRow, string olxRentCsv)
        {
            records.Add(newRow);

            using (var writer = new StreamWriter(olxRentCsv))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }

        public List<CsvRow> ReadCSVOldData(string olxRentCsv)
        {
            List<CsvRow> records = new List<CsvRow>();
            if (File.Exists(olxRentCsv))
            {
                using (var reader = new StreamReader(olxRentCsv))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<CsvRow>().ToList();
                }
            }

            return records;
        }
    }
}
