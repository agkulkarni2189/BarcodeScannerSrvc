using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace MSTPLBarcodeDataSendSrvc
{
    public partial class Service1 : ServiceBase
    {
        private COMPortUtility cpu;

        public Service1()
        {
            InitializeComponent();
            cpu = new COMPortUtility();
        }

        protected override void OnStart(string[] args)
        {
            cpu.InitiateCOMPortReaderTasks();
        }

        protected override void OnStop()
        {
            cpu.StopCOMPortDataRead();
        }
    }
}
