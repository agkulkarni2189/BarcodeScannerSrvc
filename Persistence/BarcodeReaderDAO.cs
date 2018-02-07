using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using Domain;

namespace Persistence
{
    public class BarcodeReaderDAO
    {
        private string ConnectionString;
        private Mutex ThreadSync;
        SqlTransaction trans;

        public BarcodeReaderDAO(string ConnectionString)
        {
            this.ConnectionString = ConnectionString+";Connection Timeout=10000";
            ThreadSync = new Mutex();
        }

        public int GetBarcodeReaderLaminatorMappingIDByBarcodeReaderID(int BarcodeReaderID)
        {
            int BarcodeReaderLaminatorMappingID = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetLaminatorBarcodeReaderMappingIDByBarcodeReaderID", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarcodeReaderID", BarcodeReaderID);

                    if (!Int32.TryParse(cmd.ExecuteScalar().ToString(), out BarcodeReaderLaminatorMappingID))
                        throw new FormatException("Can not parse returned object to Int32.");
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

            return BarcodeReaderLaminatorMappingID;
        }

        public DataTable GetAllBarcodeReaders()
        {
            DataTable BarcodeReadersDT = new DataTable("AllDataTables");

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetAllBarcodeReaderDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(BarcodeReadersDT);
                    con.Close();
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

            return BarcodeReadersDT;
        }

        public DataTable GetBarcodeReaderSerialNumberByReaderID(int ReaderID)
        {
            DataTable BarcodeReaderDetails = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetBarcodeReaderDetailsByReaderID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarcodeReaderID", ReaderID);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(BarcodeReaderDetails);
                    con.Close();
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

            return BarcodeReaderDetails;
        }

        public DataTable GetLaminatorDetailsByLaminatorBarcodeMappingID(int LaminatorBarcodeMappingID)
        {
            DataTable LaminatorDetails = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetLaminatorDetailsbyLaminatorBarcodeMappingID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LaminatorBarcodeMappingID", LaminatorBarcodeMappingID);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(LaminatorDetails);
                    con.Close();
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

            return LaminatorDetails;
        }
    }
}
