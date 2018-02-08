using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Domain;
using System.Data;
using Persistence;
using System.Net;
namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelReportUpdateUtility eru = new ExcelReportUpdateUtility();
            BarcodeTransactionDAO btdao = new BarcodeTransactionDAO(ConfigurationManager.ConnectionStrings["DBServerConnectionString"].ToString());
            DataTable dt = btdao.GetAllBarcodeTransactions(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, true);

            foreach (DataRow dr in dt.Rows)
            {
                BarcodeTransaction bt = new BarcodeTransaction();
                bt.ID = Int64.Parse(dr["ID"].ToString());
                bt.ModuleSerialNumber = dr["Module_Serial_Number"].ToString();
                bt.BarcodeReaderSerialNumber = dr["BarcodeReaderSerialNumber"].ToString();
                bt.LaminatorNumber = dr["LaminatorNumber"].ToString();
                bt.CreationTime = DateTime.Parse(dr["CreationTime"].ToString());

                eru.UpdateExcel(bt);
                bt = null;
            }
            //ExcelReportUpdateUtility eru = new ExcelReportUpdateUtility();
            //BarcodeTransaction bt = new BarcodeTransaction();
            //bt.ID = 170;
            //bt.ModuleSerialNumber = "AS1706154B0450";
            //bt.BarcodeReaderSerialNumber = "12345678910";
            //bt.LaminatorNumber = "L-5";
            //bt.CreationTime = System.DateTime.Now;

            //eru.UpdateExcel(bt);
            Console.ReadKey();
        }
    }
}
