using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NLog;
namespace Persistence
{
    public class DeviceDAO
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string DBConnectionString;

        public DeviceDAO(string ConnectionString)
        {
            this.DBConnectionString = ConnectionString;
        }

        public DataTable GetDeviceDetailsByLaminatorBarcodeReaderMappingID(int LbmID)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(DBConnectionString))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("sp_GetDeviceDetailsByLaminatorBarcodeReaderMappingID", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LaminatorBarcodeReaderMappingID", LbmID);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
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

            return dt;
        }
    }
}
