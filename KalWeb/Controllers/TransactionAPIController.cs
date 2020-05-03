using KalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace KalWeb.Controllers
{
    public class TransactionAPIController : ApiController
    {
        SqlConnection con = null;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter da;
        DataSet ds;
        public class jsAndr
        {
            public string responseMessage { get; set; }
            public string ResponseCode { get; set; }
            public string result { get; set; }

        }
        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("Transaction/FeedbackDetails")]
        //public JsonResult SaveFeedBackTransactionDetails(FeedbackTransaction feedbackTransaction)
        //{
        //    string result = "";
        //    jsAndr objAndr = null;
           
        //    try
        //    {
        //        con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
        //        cmd = new SqlCommand("Usp_Save_FeedbackTransaction", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Uid", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
        //        cmd.Parameters.AddWithValue("@Username", feedbackTransaction.Username == null ? (object)DBNull.Value : feedbackTransaction.Username);
        //        cmd.Parameters.AddWithValue("@AuthCode", feedbackTransaction.AuthCode == null ? (object)DBNull.Value : feedbackTransaction.AuthCode);
        //        cmd.Parameters.AddWithValue("@latlong", feedbackTransaction.latlong == null ? (object)DBNull.Value : feedbackTransaction.latlong);
        //        cmd.Parameters.AddWithValue("@caseid", feedbackTransaction.caseid == null ? (object)DBNull.Value : feedbackTransaction.caseid);
        //        cmd.Parameters.AddWithValue("@loanno", feedbackTransaction.loanno == null ? (object)DBNull.Value : feedbackTransaction.loanno);
        //        cmd.Parameters.AddWithValue("@Transactionid", feedbackTransaction.Transactionid == null ? (object)DBNull.Value : feedbackTransaction.Transactionid);
        //        cmd.Parameters.AddWithValue("@ActionCode", feedbackTransaction.ActionCode == 0 ? (object)DBNull.Value : feedbackTransaction.ActionCode);
        //        cmd.Parameters.AddWithValue("@ActionFdback", feedbackTransaction.ActionFdback == 0 ? (object)DBNull.Value : feedbackTransaction.ActionFdback);
        //        cmd.Parameters.AddWithValue("@PersonContact", feedbackTransaction.PersonContact == 0 ? (object)DBNull.Value : feedbackTransaction.PersonContact);
        //        cmd.Parameters.AddWithValue("@PlaceOfContact", feedbackTransaction.PlaceOfContact == 0 ? (object)DBNull.Value : feedbackTransaction.PlaceOfContact);
        //        cmd.Parameters.AddWithValue("@MobileNo", feedbackTransaction.MobileNo == null ? (object)DBNull.Value : feedbackTransaction.MobileNo);
        //        cmd.Parameters.AddWithValue("@VisitRemark", feedbackTransaction.VisitRemark == null ? (object)DBNull.Value : feedbackTransaction.VisitRemark);
        //        //cmd.Parameters.AddWithValue("@areacode", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
        //        //cmd.Parameters.AddWithValue("@areacode", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
        //        //cmd.Parameters.AddWithValue("@areacode", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
        //        //cmd.Parameters.AddWithValue("@areacode", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
        //        //cmd.Parameters.AddWithValue("@areacode", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
              
        //        //cmd.Parameters.AddWithValue("@OrderNo", objAndrCashMemo.OrderNumber);
        //        //cmd.Parameters.AddWithValue("@ConsumerNo", objAndrCashMemo.ConsumerNo == null ? (object)DBNull.Value : objAndrCashMemo.ConsumerNo);
        //        //cmd.Parameters.AddWithValue("@CardNo", objAndrCashMemo.Card_No == null ? (object)DBNull.Value : objAndrCashMemo.Card_No);
        //        //cmd.Parameters.AddWithValue("@BookingDate", objAndrCashMemo.BookingDate == null ? (object)DBNull.Value : objAndrCashMemo.BookingDate);
        //        //cmd.Parameters.AddWithValue("@ProductId", objAndrCashMemo.Id_Product);
        //        //cmd.Parameters.AddWithValue("@PaymentMode", objAndrCashMemo.PaymentMode);
        //        //cmd.Parameters.AddWithValue("@NoOfCyl", objAndrCashMemo.NoOfCyl);
        //        //cmd.Parameters.AddWithValue("@DelManId", objAndrCashMemo.DelManId);
        //        //cmd.Parameters.AddWithValue("@ReceivedAmt", objAndrCashMemo.Amount == null ? (object)DBNull.Value : objAndrCashMemo.Amount);
        //        //cmd.Parameters.AddWithValue("@TipAmt", objAndrCashMemo.TipAmt == null ? (object)DBNull.Value : objAndrCashMemo.TipAmt);
        //        //cmd.Parameters.AddWithValue("@is21Days", objAndrCashMemo.is21Days == null ? (object)DBNull.Value : objAndrCashMemo.is21Days);
        //        //cmd.Parameters.AddWithValue("@BookStatus", objAndrCashMemo.BookStatus == null ? (object)DBNull.Value : objAndrCashMemo.BookStatus);


        //        cmd.Parameters.AddWithValue("@ReturnVal", SqlDbType.NVarChar);
        //        cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.Output;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        result = cmd.Parameters["@ReturnVal"].Value.ToString();

        //        if (result == "1")
        //        {
        //            objAndr = new jsAndr { responseMessage = "Success", ResponseCode = "200", result = "Saved Successfully" };
        //            return Json(objAndr, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            objAndr = new jsAndr { responseMessage = "Success", ResponseCode = "200", result = "Saved Successfully" };
        //            return Json(objAndr, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objAndr = new jsAndr { responseMessage = "Success", ResponseCode = "200", result = "Saved Successfully" };
        //        return Json(objAndr, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }

          
        //}
    }
}
