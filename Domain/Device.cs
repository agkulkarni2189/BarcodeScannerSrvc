using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Device
    {
        public int ID { get; set; }
        public string IPV4Address { get; set; }
        public string MACAddress { get; set; }
    }
}
