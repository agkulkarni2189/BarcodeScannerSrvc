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

namespace MSTPLBarcodeDataReceiveSrvc
{
    public class COMPortUtility
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MessageQueue BarcodeDataQueue;
        private XmlMessageFormatter BarcodeDF;
        private ExcelReportUpdateUtility eru;
        private MSTPLFixedBarcodeReaderDBEntities2 entities2;
        private Mutex mutex;

        public COMPortUtility()
        {
            try
            {
                entities2 = new MSTPLFixedBarcodeReaderDBEntities2();
                mutex = new Mutex();

                if (!MessageQueue.Exists(@".\Private$\BarcodeDataQueue"))
                    MessageQueue.Create(@".\Private$\BarcodeDataQueue");

                BarcodeDataQueue = new MessageQueue(@".\private$\BarcodeDataQueue", QueueAccessMode.Receive);
                BarcodeDF = new XmlMessageFormatter(new Type[] { typeof(string) });
                BarcodeDataQueue.Formatter = BarcodeDF;
                BarcodeDataQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(BarcodeDataQueue_ReceiveCompleted);
                BarcodeDataQueue.BeginReceive();
                logger.Info("Barcode reading service started successfully.");
            }
            catch (MessageQueueException mqe)
            {
                logger.Error("Can not create or access the message queue on the destination server: " + mqe.Message.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("Can not start servie, an exception has been thrown: " + ex.Message.ToString());
            }
        }

        private void BarcodeDataQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs rcea)
        {
            try
            {
                mutex.WaitOne();
                BarcodeTransaction bt = new BarcodeTransaction();
                string ReadData = string.Empty;

                MessageQueue RootQueue = sender as MessageQueue;
                Message msg = RootQueue.EndReceive(rcea.AsyncResult);
                ReadData = msg.Body.ToString();
                string ModSerNum = string.Empty;

                if (ReadData.Substring(0, 2) == "NR")
                {
                    bt.ErrorID = 1;
                    bt.IsErrorResolved = false;
                }
                else
                    ModSerNum = ReadData.Substring(0, 14);

                int ReaderID;

                if (!Int32.TryParse(ReadData.Substring(14, 2), out ReaderID))
                    throw new FormatException("Can not parse Barcode Reader ID " + ReadData.Substring(14, 2).ToString());

                bt.ModuleSerialNumber = ModSerNum;
                bt.LaminatorBarcodeReaderMappingID = entities2.sp_GetLaminatorBarcodeReaderMappingIDByBarcodeReaderID(ReaderID).Select(s => s.Value).First();
                bt.CreationTime = System.DateTime.Now;
                bt.ID = entities2.sp_SaveBarcodeTransaction(0, ModSerNum, System.DateTime.Now, bt.LaminatorBarcodeReaderMappingID, bt.ErrorID, bt.IsErrorResolved).Select(s => s.Value).First();
                var BarcodeReaderDetails = entities2.sp_GetBarcodeReaderDetailsByReaderID(ReaderID);
                logger.Info("Laminator barcode reader mapping ID: " + bt.LaminatorBarcodeReaderMappingID);
                var DeviceDetails = entities2.sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID(bt.LaminatorBarcodeReaderMappingID);
                eru = new ExcelReportUpdateUtility();
                eru.SetClientSystemIP(DeviceDetails.First().IPV4Address.ToString());
               
                bt.BarcodeReaderSerialNumber = BarcodeReaderDetails.First().SerialNumber.ToString();
                bt.LaminatorNumber = entities2.sp_GetLaminatorDetailsbyLaminatorBarcodeMappingID(bt.LaminatorBarcodeReaderMappingID).First().LaminatorNumber.ToString();
                eru.UpdateExcel(bt);
                eru = null;

                if (string.IsNullOrEmpty(ModSerNum))
                    logger.Info("Unable to read the data from reader " + entities2.sp_GetAllBarcodeReaderDetails(entities2.sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID(bt.LaminatorBarcodeReaderMappingID).First().IPV4Address.ToString()).Where(r => r.ID == ReaderID).Select(r => r.SerialNumber).First());
                else
                    logger.Info("Data read from reader " + entities2.sp_GetAllBarcodeReaderDetails(entities2.sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID(bt.LaminatorBarcodeReaderMappingID).First().IPV4Address.ToString()).Where(r => r.ID == ReaderID).Select(r => r.SerialNumber).First() + " at laminator "+bt.LaminatorNumber.ToString() + ":" + ModSerNum);

                RootQueue.BeginReceive();
                mutex.ReleaseMutex();
            }
            catch (FormatException fe)
            {
                logger.Error("Format exception has been thrown at BarcodeDataQueue_ReceiveCompleted(): "+fe.Message.ToString());
            }
            catch (MessageQueueException mqe)
            {
                logger.Error("Message queue exception has been thrown at BarcodeDataQueue_ReceiveCompleted(): " + mqe.Message.ToString());
            }
            catch (SqlException se)
            {
                logger.Error("SQL exception has been thrown at BarcodeDataQueue_ReceiveCompleted(): "+se.Message.ToString());
            }
            catch (ArgumentOutOfRangeException aore)
            {
                logger.Error("Argument out of range exception has been thrown at BarcodeDataQueue_ReceiveCompleted(): "+aore.Message.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("Exception has been thrown at BarcodeDataQueue_ReceiveCompleted(): "+ex.Message.ToString());
            }
        }

        public void StopCOMPortDataRead()
        {

        }
    }
}
