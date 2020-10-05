using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService1.Models
{
    public class CsvRow
    {
        public string Date { get; set; }
        public int WroclawToRent { get; set; }
        public int WroclawToSell { get; set; }
        public int GdanskToRent { get; set; }
        public int GdanskToSell { get; set; }
        public int WarszawaToRent { get; set; }
        public int WarszawaToSell { get; set; }
        public int KrakowToRent { get; set; }
        public int KrakowToSell { get; set; }
        public int PoznanToRent { get; set; }
        public int PoznanToSell { get; set; }
        public int LodzToRent { get; set; }
        public int LodzToSell { get; set; }
        public int TotalToRent { get; set; }
        public int TotalToSell { get; set; }
    }
}
