using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class BarcodeReader
    {
        public int ID { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
