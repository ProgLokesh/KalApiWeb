using KalWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;






namespace KalWeb.Controllers
{
    public class TransactionFeedAPIController : Controller
    {
        SqlConnection con = null;
        SqlCommand cmd;
        //DataTable dt;
        //SqlDataAdapter da;
        //DataSet ds;
        public class jsAndr
        {
            public string responseMessage { get; set; }
            public string ResponseCode { get; set; }
            public string result { get; set; }

            public string TransactionId { get; set; }
        }
       
        public JsonResult SaveFeedBackTransactionDetails(FeedbackTransaction feedbackTransaction)
        {
            string result = "";
            jsAndr objAndr = null;

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
                cmd = new SqlCommand("Usp_Save_FeedbackTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Uid", feedbackTransaction.Uid == 0 ? (object)DBNull.Value : feedbackTransaction.Uid);
                cmd.Parameters.AddWithValue("@Username", feedbackTransaction.Username == null ? (object)DBNull.Value : feedbackTransaction.Username);
                cmd.Parameters.AddWithValue("@AuthCode", feedbackTransaction.AuthCode == null ? (object)DBNull.Value : feedbackTransaction.AuthCode);
                cmd.Parameters.AddWithValue("@latlong", feedbackTransaction.latlong == null ? (object)DBNull.Value : feedbackTransaction.latlong);
                cmd.Parameters.AddWithValue("@caseid", feedbackTransaction.caseid == null ? (object)DBNull.Value : feedbackTransaction.caseid);
                cmd.Parameters.AddWithValue("@loanno", feedbackTransaction.loanno == null ? (object)DBNull.Value : feedbackTransaction.loanno);
                cmd.Parameters.AddWithValue("@Transactionid", feedbackTransaction.Transactionid == null ? (object)DBNull.Value : feedbackTransaction.Transactionid);
                cmd.Parameters.AddWithValue("@ActionCode", feedbackTransaction.ActionCode == 0 ? (object)DBNull.Value : feedbackTransaction.ActionCode);
                cmd.Parameters.AddWithValue("@ActionFdback", feedbackTransaction.ActionFdback == 0 ? (object)DBNull.Value : feedbackTransaction.ActionFdback);
                cmd.Parameters.AddWithValue("@PersonContact", feedbackTransaction.PersonContact == 0 ? (object)DBNull.Value : feedbackTransaction.PersonContact);
                cmd.Parameters.AddWithValue("@PlaceOfContact", feedbackTransaction.PlaceOfContact == 0 ? (object)DBNull.Value : feedbackTransaction.PlaceOfContact);
                cmd.Parameters.AddWithValue("@MobileNo", feedbackTransaction.MobileNo == null ? (object)DBNull.Value : feedbackTransaction.MobileNo);
                cmd.Parameters.AddWithValue("@VisitRemark", feedbackTransaction.VisitRemark == null ? (object)DBNull.Value : feedbackTransaction.VisitRemark);
                cmd.Parameters.AddWithValue("@amt", feedbackTransaction.amt == 0 ? (object)DBNull.Value : feedbackTransaction.amt);
                cmd.Parameters.AddWithValue("@PayMode", feedbackTransaction.PayMode == 0 ? (object)DBNull.Value : feedbackTransaction.Username);
                cmd.Parameters.AddWithValue("@BankName", feedbackTransaction.BankName == null ? (object)DBNull.Value : feedbackTransaction.BankName);
                cmd.Parameters.AddWithValue("@ChqNo", feedbackTransaction.ChqNo == null ? (object)DBNull.Value : feedbackTransaction.ChqNo);
                cmd.Parameters.AddWithValue("@ChqDate", feedbackTransaction.ChqDate == null ? (object)DBNull.Value : feedbackTransaction.ChqDate);
                cmd.Parameters.AddWithValue("@IFSCCode", feedbackTransaction.IFSCCode == null ? (object)DBNull.Value : feedbackTransaction.IFSCCode);
                cmd.Parameters.AddWithValue("@RefNo", feedbackTransaction.RefNo == null ? (object)DBNull.Value : feedbackTransaction.RefNo);
                cmd.Parameters.AddWithValue("@RefPaydate", feedbackTransaction.RefPaydate == null ? (object)DBNull.Value : feedbackTransaction.RefPaydate);
                cmd.Parameters.AddWithValue("@PTPDate", feedbackTransaction.PTPDate == "{1/1/0001 12:00:00 AM}" ? (object)DBNull.Value : feedbackTransaction.PTPDate);
                cmd.Parameters.AddWithValue("@PTPTime", feedbackTransaction.PTPTime == "{1/1/0001 12:00:00 AM}" ? (object)DBNull.Value : feedbackTransaction.PTPTime);
                cmd.Parameters.AddWithValue("@PTPStatus", feedbackTransaction.PTPStatus == 0 ? (object)DBNull.Value : feedbackTransaction.PTPStatus);
                cmd.Parameters.AddWithValue("@TotalVisitNo", feedbackTransaction.TotalVisitNo == 0 ? (object)DBNull.Value : feedbackTransaction.TotalVisitNo);
                cmd.Parameters.AddWithValue("@AgentVisitNo", feedbackTransaction.AgentVisitNo == 0 ? (object)DBNull.Value : feedbackTransaction.AgentVisitNo);
                cmd.Parameters.AddWithValue("@TransactionDate", feedbackTransaction.TransactionDate == "{1/1/0001 12:00:00 AM}" ? (object)DBNull.Value : feedbackTransaction.TransactionDate);
                cmd.Parameters.AddWithValue("@TransactionTime", feedbackTransaction.TransactionTime == "{1/1/0001 12:00:00 AM}" ? (object)DBNull.Value : feedbackTransaction.TransactionTime);
                cmd.Parameters.AddWithValue("@Status", feedbackTransaction.Status == 0 ? (object)DBNull.Value : feedbackTransaction.Status);
                cmd.Parameters.AddWithValue("@Photo", feedbackTransaction.Photo == null? (object)DBNull.Value : SaveImage(feedbackTransaction.Photo, feedbackTransaction.Transactionid));
                cmd.Parameters.AddWithValue("@ReturnVal", SqlDbType.NVarChar);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@ReturnVal"].Value.ToString();
                if (result == "1")
                {
                    objAndr = new jsAndr { responseMessage = "Success", ResponseCode = "200", result = "Saved Successfully",TransactionId = feedbackTransaction.Transactionid };
                    return Json(objAndr, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objAndr = new jsAndr { responseMessage = "Error Occured", ResponseCode = "500", result = "Error" };
                    return Json(objAndr, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objAndr = new jsAndr { responseMessage = "Error Occured", ResponseCode = "500", result = "Error" };
                return Json(objAndr, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                con.Close();
            }


        }


        public string SaveImage(string ImgStr, string TransId)
        {
            string ImagePath;
            string fileName = TransId;
            string extension = Path.GetExtension(fileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            String path = Server.MapPath("~/Images/"); //Path
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var crDate = DateTime.Now.ToString("ddMMyyyymmssfff");
            string imageName = path + TransId + crDate + ".jpg";
            ImagePath = "Images/" + TransId + crDate + ".jpg";
            string imgPath = Path.Combine(path, imageName);
            byte[] imageBytes = Convert.FromBase64String(ImgStr);
            System.IO.File.WriteAllBytes(imgPath, imageBytes);
            return ImagePath;
        }
    }
}