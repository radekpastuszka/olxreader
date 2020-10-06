using System;

namespace OlxReaderHost.Model
{
    public class OlxDataDto
    {
        public DateTime Date { get; set; }
        public DataTypeEnum DataType { get; set; }
        public int ToSell { get; set; }
        public int ToRent { get; set; }
    }
}
