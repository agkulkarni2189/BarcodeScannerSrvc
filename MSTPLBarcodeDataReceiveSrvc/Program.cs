using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace MSTPLBarcodeDataReceiveSrvc
{
    static class Program
    {
        /// <summary>
        /// the main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] servicestorun;
            servicestorun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(servicestorun);

            //new COMPortUtility();
            //new Service1();
        }
    }
}
