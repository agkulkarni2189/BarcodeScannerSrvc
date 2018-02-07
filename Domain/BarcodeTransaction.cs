using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class BarcodeTransaction : MarshalByRefObject
    {
        public Int64 ID { get; set; }
        public string ModuleSerialNumber { get; set; }
        public int LaminatorBarcodeReaderMappingID { get; set; }
        public DateTime CreationTime { get; set; }
        public string BarcodeReaderSerialNumber { get; set; }
        public string LaminatorNumber { get; set; }
        public int ErrorID { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsErrorResolved { get; set; }
    }
}
