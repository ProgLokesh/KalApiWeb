using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KalWeb.Models
{
    //public class DBHelpers
    //{
       
    //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
    //    SqlCommand cmd;
    //    SqlDataAdapter da;
    //    SqlTransaction transaction;
    //    DataTable dt;
    //    DataSet ds;
    //    public string SavePartialTVOUT(TVOut objTVOutHDR)
    //    {
    //        con.Open();
    //        string result = "";
    //        transaction = con.BeginTransaction();
    //        try
    //        {
    //            cmd = new SqlCommand("USP_SaveTVOUT", con, transaction);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@GodownID", objTVOutHDR.GODOWN_CODE == null ? (object)DBNull.Value : objTVOutHDR.GODOWN_CODE);
    //            cmd.Parameters.AddWithValue("@ProductID", objTVOutHDR.ProductID == null ? (object)DBNull.Value : objTVOutHDR.ProductID);
    //            cmd.Parameters.AddWithValue("@ConsumerNo", objTVOutHDR.Consumer_No == null ? (object)DBNull.Value : objTVOutHDR.Consumer_No);
    //            cmd.Parameters.AddWithValue("@TvInvoiceCode", objTVOutHDR.TV_Invoice_code == null ? (object)DBNull.Value : objTVOutHDR.TV_Invoice_code);
    //            cmd.Parameters.AddWithValue("@No_of_Cyl", objTVOutHDR.No_of_Cyl == null ? (object)DBNull.Value : objTVOutHDR.No_of_Cyl);
    //            cmd.Parameters.AddWithValue("@Updated_By", HttpContext.Current.Session["ID_user"]);
    //            cmd.Parameters.AddWithValue("@Updated_Date", objTVOutHDR.Updated_Date == null ? (object)DBNull.Value : objTVOutHDR.Updated_Date);
    //            cmd.ExecuteNonQuery();
    //            transaction.Commit();
    //            return result = "success";
    //        }
    //        catch (Exception e)
    //        {
    //            transaction.Rollback();
    //            return result = e.Message;

    //        }
    //        finally
    //        {
    //            con.Close();
    //        }

    //    }
    //}
}