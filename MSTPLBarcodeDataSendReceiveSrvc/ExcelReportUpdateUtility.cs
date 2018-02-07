using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Domain;
using System.IO;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.Remoting;
using OfficeOpenXml;

namespace MSTPLBarcodeDataSendSrvc
{
    public class ExcelReportUpdateUtility : MarshalByRefObject
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ExcelPackage BarcodeExcelPkg;
        private ExcelWorksheet xlws;
        private Queue<BarcodeTransaction> FailedAccessTransactions;
        private Queue<BarcodeTransaction> FailedEditTransactions;
        private Task FailedTransactionUpdateTask;
        private FileStream fs; 

        public ExcelReportUpdateUtility()
        {
            try
            {
                FailedAccessTransactions = new Queue<BarcodeTransaction>();
                
                if (!Directory.Exists(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc")))
                {
                    Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc"));
                }

                if (!File.Exists(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc") + ConfigurationManager.AppSettings.Get("ExcelFileName") + ".xlsx"))
                {
                    using (BarcodeExcelPkg = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc") + ConfigurationManager.AppSettings.Get("ExcelFileName") + ".xlsx")))
                    {

                        xlws = BarcodeExcelPkg.Workbook.Worksheets.Add(ConfigurationManager.AppSettings.Get("ExcelWorkSheetName"));

                        xlws.Cells["A1"].Value = "Sr. No";
                        xlws.Cells["B1"].Value = "ID";
                        xlws.Cells["C1"].Value = "Module Serial Number";
                        xlws.Cells["D1"].Value = "Reader Serial Number";
                        xlws.Cells["E1"].Value = "Laminator Number";
                        xlws.Cells["F1"].Value = "Creation Time";

                        xlws.Cells["A1:F1"].Style.Font.Bold = true;
                        xlws.Column(2).Hidden = true;
                        xlws.Protection.IsProtected = false;
                        xlws.Protection.AllowSelectLockedCells = false;

                        BarcodeExcelPkg.Save();
                    }
                }
                FailedTransactionUpdateTask = new Task(new Action(InitiateFailedTransactionUpdateTimer));
                FailedTransactionUpdateTask.Start();
            }
            catch (NullReferenceException nre)
            {
                logger.Error("Null reference exception has occured at ExcelReportUpdateUtility(): " + nre.Message + " with stack trace: " + nre.StackTrace);
            }
            catch (FileNotFoundException fnfe)
            {
                logger.Error("File not found exception has occured at ExcelReportUpdateUtility(): " + fnfe.Message + " with stack trace: " + fnfe.StackTrace);
            }
            catch (Exception ex)
            {
                logger.Error("Exception has occured at ExcelReportUpdateUtility(): " + ex.Message + " with stack trace: " + ex.StackTrace);
            }
        }

        private void InitiateFailedTransactionUpdateTimer()
        {
            Timer t = new Timer(10000);
            t.Elapsed += new ElapsedEventHandler(WritePendingBarcodeTransactionToExcel);
            t.Enabled = true;
            t.Start();
        }
        public void UpdateExcel(BarcodeTransaction bt)
        {
            try
            {
                using (var XlPkg = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc") + ConfigurationManager.AppSettings.Get("ExcelFileName") + ".xlsx")))
                {
                    ExcelWorksheet xws = XlPkg.Workbook.Worksheets[ConfigurationManager.AppSettings["ExcelWorkSheetName"]];
                    int WorksheetRowCount = xws.Dimension.End.Row;
                    int reportRowCount = 0;

                    xws.InsertRow((WorksheetRowCount + reportRowCount + 1), 1);
                    xws.Cells[(WorksheetRowCount + reportRowCount + 1), 1].Value = WorksheetRowCount + reportRowCount + 1;
                    xws.Cells[(WorksheetRowCount + reportRowCount + 1), 2].Value = bt.ID.ToString();
                    xws.Cells[(WorksheetRowCount + reportRowCount + 1), 3].Value = bt.ModuleSerialNumber.ToString();
                    xws.Cells[(WorksheetRowCount + reportRowCount + 1), 4].Value = bt.BarcodeReaderSerialNumber.ToString();
                    xws.Cells[(WorksheetRowCount + reportRowCount + 1), 5].Value = bt.LaminatorNumber.ToString();
                    xws.Cells[(WorksheetRowCount + reportRowCount + 1), 6].Value = bt.CreationTime.ToString();

                    xws.Column(1).AutoFit();
                    xws.Column(2).AutoFit();
                    xws.Column(3).AutoFit();
                    xws.Column(4).AutoFit();
                    xws.Column(5).AutoFit();
                    xws.Column(6).AutoFit();
                    xws.Column(2).Hidden = true;

                    File.WriteAllBytes(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc") + ConfigurationManager.AppSettings.Get("ExcelFileName") + ".xlsx", XlPkg.GetAsByteArray());

                    logger.Info("Excel report updated successfully.");
                }
            }
            catch (IOException ioe)
            {
                logger.Error("I/O exception has occured at UpdateExcel(): " + ioe);
                FailedAccessTransactions.Enqueue(bt);
            }
            catch (Exception ex)
            {
                logger.Error("Exception has occured at UpdateExcel(): " + ex.Message+" with stack trace: "+ex.StackTrace);
            }
        }

        private void WritePendingBarcodeTransactionToExcel(object o, ElapsedEventArgs e)
        {
            if (FailedAccessTransactions.Count > 0)
            {
                UpdateExcel(FailedAccessTransactions.Dequeue());
            }

            if (FailedEditTransactions.Count > 0)
            {
                EditExcelEntriesUsingBarcodeTrans(FailedEditTransactions.Dequeue());
            }
        }

        public void EditExcelEntries(string BarcodeTransID = "", string ModSerNum = "", string ReaderSerialNum = "", string LaminatorNum = "")
        {
            try
            {
                if (Int64.Parse(BarcodeTransID) > 0)
                {
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc") + ConfigurationManager.AppSettings.Get("ExcelFileName") + ".xlsx")))
                        {
                            ExcelWorksheet ew = ep.Workbook.Worksheets[ConfigurationManager.AppSettings.Get("ExcelWorkSheetName")];

                            for (int i = ew.Dimension.Start.Row + 1; i <= ew.Dimension.End.Row; i++)
                            {
                                if (Int64.Parse(ew.Cells[i, 2].Value.ToString()) == Int64.Parse(BarcodeTransID))
                                {
                                    if ((!string.IsNullOrEmpty(ModSerNum)) && (!ew.Cells[i, 3].Text.ToString().Equals(ModSerNum)))
                                        ew.Cells[i, 3].Value = ModSerNum.ToString();

                                    if ((!string.IsNullOrEmpty(ReaderSerialNum)) && (!ew.Cells[i, 4].Text.ToString().Equals(ReaderSerialNum)))
                                        ew.Cells[i, 4].Value = ReaderSerialNum.ToString();

                                    if ((!string.IsNullOrEmpty(LaminatorNum)) && (!ew.Cells[i, 5].Text.ToString().Equals(LaminatorNum)))
                                        ew.Cells[i, 5].Value = LaminatorNum;

                                    break;
                                }
                            }

                            File.WriteAllBytes(ConfigurationManager.AppSettings.Get("ExcelWorksheetDirLoc") + ConfigurationManager.AppSettings.Get("ExcelFileName") + ".xlsx", ep.GetAsByteArray());
                            logger.Info("Entries updated of Transaction ID: " + BarcodeTransID);
                        }
                }
            }
            catch (FormatException fe)
            {
                logger.Error("Format exception has occured at EditExcelEntries(): " + fe.Message);

                BarcodeTransaction bt = new BarcodeTransaction();
                bt.ID = Int64.Parse(BarcodeTransID);
                bt.ModuleSerialNumber = ModSerNum;
                bt.BarcodeReaderSerialNumber = ReaderSerialNum;
                bt.LaminatorNumber = LaminatorNum;

                FailedEditTransactions.Enqueue(bt);
                throw fe;
            }
            catch (InvalidOperationException ioe)
            {
                logger.Error("Invalid operation exception has occured at EditExcelEntries(): " + ioe.Message + " with Stack Trace: " + ioe.StackTrace);

                BarcodeTransaction bt = new BarcodeTransaction();
                bt.ID = Int64.Parse(BarcodeTransID);
                bt.ModuleSerialNumber = ModSerNum;
                bt.BarcodeReaderSerialNumber = ReaderSerialNum;
                bt.LaminatorNumber = LaminatorNum;

                FailedEditTransactions.Enqueue(bt);

                throw ioe;
            }
            catch (Exception ex)
            {
                logger.Error("Exception has occured at EditExcelEntries(): " + ex.Message);

                BarcodeTransaction bt = new BarcodeTransaction();
                bt.ID = Int64.Parse(BarcodeTransID);
                bt.ModuleSerialNumber = ModSerNum;
                bt.BarcodeReaderSerialNumber = ReaderSerialNum;
                bt.LaminatorNumber = LaminatorNum;

                FailedEditTransactions.Enqueue(bt);
                throw ex;
            }
        }

        public void UpdateExcelFromExternalApplications(long BarcodeTransID, string ModSerNum, string ReaderSerialNum, string LamNum, string CreationTime)
        {
            BarcodeTransaction bt = new BarcodeTransaction();
            bt.ID = BarcodeTransID;
            bt.ModuleSerialNumber = ModSerNum;
            bt.BarcodeReaderSerialNumber = ReaderSerialNum;
            bt.LaminatorNumber = LamNum;
            bt.CreationTime = DateTime.Parse(CreationTime);

            this.UpdateExcel(bt);
        }

        public void EditExcelEntriesUsingBarcodeTrans(BarcodeTransaction bt)
        {
            try
            {
                this.EditExcelEntries(bt.ID.ToString(), bt.ModuleSerialNumber, bt.BarcodeReaderSerialNumber, bt.LaminatorNumber);
            }
            catch (Exception ex)
            {
                logger.Error("Exception has occured at EditExcelEntriesUsingBarcodeTrans(): " + ex.Message);
            }
        }
    }
}
