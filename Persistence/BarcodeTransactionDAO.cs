using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using System.Data.SqlClient;
using System.Data;

namespace Persistence
{
    public class BarcodeTransactionDAO
    {
        private string ConnectionString;


        public BarcodeTransactionDAO(string ConnectionString)
        {
            this.ConnectionString = ConnectionString+";Connection Timeout=10000";
        }

        public int InsertNewBarcodeReaderTransaction(BarcodeTransaction bt)
        {
            int NumRowsAffected = 0;
            SqlTransaction transaction;

                
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    transaction = con.BeginTransaction();

                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_SaveBarcodeTransaction", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Transaction = transaction;

                        if(bt.ID > 0)
                            cmd.Parameters.AddWithValue("@BarcodeTransactionID", bt.ID);

                        cmd.Parameters.AddWithValue("@ModSerNum", bt.ModuleSerialNumber);
                        cmd.Parameters.AddWithValue("@LaminatorBarcodeReaderMappingID", bt.LaminatorBarcodeReaderMappingID);
                        cmd.Parameters.AddWithValue("@CreationTime", bt.CreationTime);
                        if (bt.ErrorID > 0)
                        {
                            cmd.Parameters.AddWithValue("@ErrorID", bt.ErrorID);
                            cmd.Parameters.AddWithValue("@IsErrorResolved", bt.IsErrorResolved);
                        }

                        NumRowsAffected = Int32.Parse(cmd.ExecuteScalar().ToString());
                        transaction.Commit();
                    }
                    catch (SqlException se)
                    {
                        transaction.Rollback();
                        throw se;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            

            return NumRowsAffected;
        }

        public DataTable GetAllBarcodeTransactions(string BarcodeTransID = "", string ModuleSerialNumber = "", string BarcodeReaderSerialNumber = "", string LaminatorNumber = "", string ScanDate = "",bool IsSearchOp = false, int StartLamBarcodeMappingID = 0, int EndLamBarcodeMappingID = 0)
        {
            DateTime BarcodeScanDate = DateTime.Now;
            Int64 BarcodeTransactionID = 0;
            //DateTime BarcodeScanTime = DateTime.Now;

            DataTable BarcodeTransactions = new DataTable("AllBarcodeTransactions");
            try
            {
                if (ScanDate != "")
                {
                    if (!DateTime.TryParseExact(ScanDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out BarcodeScanDate))
                        throw new FormatException("Can not parse barcode scanning date");
                }

                if (!string.IsNullOrEmpty(BarcodeTransID))
                {
                    if (!Int64.TryParse(BarcodeTransID, out BarcodeTransactionID))
                        throw new FormatException("Can not parse barcode transaction id, please contact admin");
                }

                using (SqlConnection con = new SqlConnection(this.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetAllBarcodeTransactions", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (BarcodeTransactionID > 0)
                        cmd.Parameters.AddWithValue("@BarcodeTransID", BarcodeTransactionID);
                    cmd.Parameters.AddWithValue("@Module_Serial_Number", ModuleSerialNumber == "" ? null : ModuleSerialNumber.ToString());
                    cmd.Parameters.AddWithValue("@Barcode_Reader_Serial_Number", BarcodeReaderSerialNumber == "" ? null : BarcodeReaderSerialNumber.ToString());
                    cmd.Parameters.AddWithValue("@Laminator_Number", LaminatorNumber == "" ? null : LaminatorNumber.ToString());
                    if(ScanDate != "")
                        cmd.Parameters.AddWithValue("@Barcode_Scan_Date", BarcodeScanDate);
                    cmd.Parameters.AddWithValue("@IsSearchOp", IsSearchOp ? 1 : 0);

                    if (StartLamBarcodeMappingID > 0 && EndLamBarcodeMappingID > 0)
                    {
                        cmd.Parameters.AddWithValue("@StartLaminatorBarcodeReaderMappingID", StartLamBarcodeMappingID);
                        cmd.Parameters.AddWithValue("@EndLaminatorBarcodeReaderMappingID", EndLamBarcodeMappingID);
                    }

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(BarcodeTransactions);

                    con.Close();
                }
            }
            catch (SqlException se)
            {
                throw se;
            }
            catch (FormatException fe)
            {
                throw fe;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return BarcodeTransactions;
        }

        public int MarkBarcodeTransDisplayed(DataTable BarcodeTransIds)
        {
            int NumRowsAffected = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_MarkBarcodeTransactionsDisplayed", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarcodeTransIds", BarcodeTransIds);
                    NumRowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException se)
            {
                throw se;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return NumRowsAffected;
        }
    }
}
