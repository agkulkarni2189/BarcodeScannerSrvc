using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Configuration;
using System.Threading;
using Domain;
using Persistence;
using System.Data.SqlClient;
using NLog;
using System.Data;
using System.Threading.Tasks;
using System.Messaging;

namespace MSTPLBarcodeDataSendSrvc
{
    public class COMPortUtility
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private string[] COMPortNames;
        private int SerialPortBaudRate;
        private int SerialPortReadTimeOut;
        private Parity SerialPortParity;
        private int SerialPortDataBits;
        private StopBits SerialPortStopBits;
        private Handshake SerialPortHandshake;
        private Mutex SyncLock;
        //private Thread[] COMPortReaderThreads;
        private BarcodeTransactionDAO btdao;
        private string DBServerConnectionString;
        private BarcodeReaderDAO brdao;
        private DataTable AllBarcodeReaderDetails;
        private TaskFactory tfsp;
        private CancellationToken tfct;
        private CancellationTokenSource cts;
        private MessageQueue BarcodeDataQueue;
        private XmlMessageFormatter BarcodeMessageFormatter;

        public COMPortUtility()
        {
            try
            {
                logger.Info("Barcode reading service started successfully.");
                DBServerConnectionString = ConfigurationManager.ConnectionStrings["DBServerConnectionString"].ToString();
                btdao = new BarcodeTransactionDAO(DBServerConnectionString);
                brdao = new BarcodeReaderDAO(DBServerConnectionString);
                AllBarcodeReaderDetails = brdao.GetAllBarcodeReaders();

                if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("SerialPortBaudRate"), out SerialPortBaudRate))
                    SerialPortBaudRate = 9600;

                if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("SerialPortReadTimeout"), out SerialPortReadTimeOut))
                    SerialPortReadTimeOut = 500;

                if (!Enum.TryParse<Parity>(ConfigurationManager.AppSettings.Get("SerialPortParity"), out SerialPortParity))
                    SerialPortParity = Parity.None;

                if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("SerialDataBits"), out SerialPortDataBits))
                    SerialPortDataBits = 8;

                if (!Enum.TryParse<StopBits>(ConfigurationManager.AppSettings.Get("SerialPortStopBits"), out SerialPortStopBits))
                    SerialPortStopBits = StopBits.One;

                if (!Enum.TryParse<Handshake>(ConfigurationManager.AppSettings.Get("SerialPortHandShake"), out SerialPortHandshake))
                    SerialPortHandshake = Handshake.None;

                SyncLock = new Mutex(false, "COMPortDataReadMutex");
                cts = new CancellationTokenSource();
                tfct = cts.Token;
                tfsp = new TaskFactory(tfct);

                //if (!MessageQueue.Exists(@"FormatName:DIRECT=OS:Aniket\private$\BarcodeDataQueue"))
                //    MessageQueue.Create(@"FormatName:DIRECT=OS:Aniket\private$\BarcodeDataQueue");

                BarcodeDataQueue = new MessageQueue(@"FormatName:DIRECT=OS:Aniket\private$\BarcodeDataQueue", true, true, QueueAccessMode.Send);
                BarcodeMessageFormatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            }
            catch (Exception ex)
            {
                logger.Error("Can not start servie, an exception has been thrown: " + ex.Message.ToString());
            }
        }

        public void InitiateCOMPortReaderTasks()
        {
            foreach (string COMPortName in SerialPort.GetPortNames())
            {
                Action action = () => InitializeCOMPort(COMPortName);
                tfsp.StartNew(action, TaskCreationOptions.AttachedToParent);
            }
        }
        private void InitializeCOMPort(object COMPortName)
        {
            SerialPort port = new SerialPort();

            try
            {
                port.PortName = COMPortName.ToString();
                port.BaudRate = SerialPortBaudRate;
                port.ReadTimeout = SerialPortReadTimeOut;
                port.Parity = SerialPortParity;
                port.DataBits = SerialPortDataBits;
                port.StopBits = SerialPortStopBits;
                port.Handshake = SerialPortHandshake;
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

                if (!port.IsOpen)
                    port.Open();

                logger.Info("Reader detected and opened on port: " + port.PortName);
            }
            catch (UnauthorizedAccessException uae)
            {
                logger.Error("Can not access " + port.PortName + ": " + uae.Message.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs s)
        {
            try
            {
                SyncLock.WaitOne();
                Thread.Sleep(SerialPortReadTimeOut);
                BarcodeTransaction bt = new BarcodeTransaction();
                string ReadData = string.Empty;
                SerialPort sp = null;
                sp = (SerialPort)sender;
                ReadData = sp.ReadExisting();
                sp.DiscardInBuffer();
                string ModSerNum = string.Empty;

                //if (ReadData.Length != 2 || ReadData.Length != 16)
                //    throw new FormatException("Scanned data length must be either 2 or 16 chars long");

                Message BarcodeMessage = new Message(ReadData, BarcodeMessageFormatter);
                BarcodeDataQueue.Send(BarcodeMessage);
                logger.Info("Message received and sent to queue: " + BarcodeMessage.Body.ToString());
                SyncLock.ReleaseMutex();
            }
            catch (FormatException fe)
            {
                logger.Error(fe.Message.ToString());
            }
            catch (SqlException se)
            {
                logger.Error(se.Message.ToString());
            }
            catch (ArgumentOutOfRangeException aore)
            {
                logger.Error(aore.Message.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
        }

        public void StopCOMPortDataRead()
        {
            cts.Cancel();
        }
    }
}
