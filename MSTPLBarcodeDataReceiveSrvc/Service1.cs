using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Ipc;
using System.Collections;
using System.Net;

namespace MSTPLBarcodeDataReceiveSrvc
{
    public partial class Service1 : ServiceBase
    {
        private COMPortUtility cpu;
        private HttpChannel httpchannel;
        private IpcServerChannel ipcservchannel;
        private IDictionary idict;
        private ChannelSrvc IpcChan;
        private ChannelSrvc httpchan;

        public Service1()
        {
            InitializeComponent();

            idict = new Hashtable();
            idict["name"] = "myDict";
            idict["priority"] = 2;
            httpchannel = new HttpChannel(5678);
            //ipcservchannel = new IpcServerChannel("ExcelReportUpdateIpcChannel"); 
            //IpcChan = new ChannelSrvc(ipcservchannel, false);

            httpchan = new ChannelSrvc(httpchannel, false);
            ChannelServices.RegisterChannel(httpchan, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ExcelReportUpdateUtility), "ExcelReportUpdateUtility", WellKnownObjectMode.Singleton);
        }

        protected override void OnStart(string[] args)
        {
            cpu = new COMPortUtility();
        }

        protected override void OnStop()
        {
        }
    }

    class ChannelSrvc : IChannel, ISecurableChannel
    {
        //private IpcServerChannel ipcservchan;
        private HttpChannel httpchan;
        private bool IsChannelSecurable;

        //public ChannelSrvc(IpcServerChannel isc, bool IsChannelSecured)
        //{
        //    this.ipcservchan = isc;
        //    this.IsChannelSecurable = IsChannelSecured;
        //}

        public ChannelSrvc(HttpChannel httpchannel, bool IsChannelSecured)
        {
            this.httpchan = httpchannel;
            this.IsChannelSecurable = IsChannelSecured;
        }
        public string ChannelName
        {
            get { return this.httpchan.ChannelName; }
        }

        public int ChannelPriority
        {
            get { return this.httpchan.ChannelPriority; }
        }

        public string Parse(string url, out string objectURI)
        {
            httpchan.CreateMessageSink("http://"+Dns.GetHostEntry(Dns.GetHostName()).AddressList.AsEnumerable().Where(r => r.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(r => r).First().ToString()+":"+"5678", null, out objectURI);
            return objectURI;
        }

        public bool IsSecured
        {
            get
            {
                return httpchan.IsSecured;
            }
            set
            {
                httpchan.IsSecured = IsChannelSecurable;
            }
        }
    }
}
