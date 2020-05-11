using KalWeb.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace KalWeb.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = null;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter da;
        DataSet ds;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            Session.Clear();
            string path1 = Server.MapPath("~/UploadedFiles/");
            string path2 = Server.MapPath("~/Write_Log/");
            DirectoryInfo dirInfo1 = new DirectoryInfo(path1);
            DirectoryInfo dirInfo2 = new DirectoryInfo(path2);
            FileInfo[] files1 = dirInfo1.GetFiles("*.*");
            FileInfo[] files2 = dirInfo2.GetFiles("*.*");
           

            List<string> lst = new List<string>(files1.Length);
            foreach (var item in files1)
            {
                if(item.LastWriteTime.Date==DateTime.Today.Date)
                {
                    lst.Add(item.Name);
                }
                
            }
            foreach (var item in files2)
            {
                if (item.LastWriteTime.Date == DateTime.Today.Date)
                {
                    lst.Add(item.Name);
                }

            }

            return View(lst);
            
        }


        public ActionResult DownloadFile(string filename)
        {
            string fullName;
            var file=filename.Split('.');
            if(file[1]!="log")
            {
                 fullName = Server.MapPath("~/UploadedFiles/" + filename);
            }
            else
            {
                 fullName = Server.MapPath("~/Write_Log/" + filename);
            }
            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase fileUpload)
        {
            DataTable dtbl = Session["dtbl"] as DataTable;
           string DataType= Session["DataType"].ToString();
           var ColAgentId= Session["ColAgentId"];
            if (dtbl!=null)
            {
                dtbl.Columns.Add(new DataColumn { ColumnName = "Action", DefaultValue = DataType });
                dtbl.Columns.Add(new DataColumn { ColumnName = "Coll_Agent_Id", DefaultValue = ColAgentId });
                if(DataType=="F")
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
                    cmd = new SqlCommand("USP_SaveUploadedData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "C");
                    cmd.Parameters.AddWithValue("@Coll_Agent_Id", ColAgentId);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    con.Close();
                    da.Dispose();
                }
                UploadSapConsumerDetails(dtbl);
                ViewBag.Message = dtbl.Rows.Count + " Records Uploaded Successfully";
                Session.Clear();
                string path1 = Server.MapPath("~/UploadedFiles/");
                string path2 = Server.MapPath("~/Write_Log/");
                DirectoryInfo dirInfo1 = new DirectoryInfo(path1);
                DirectoryInfo dirInfo2 = new DirectoryInfo(path2);
                FileInfo[] files1 = dirInfo1.GetFiles("*.*");
                FileInfo[] files2 = dirInfo2.GetFiles("*.*");


                List<string> lst = new List<string>(files1.Length);
                foreach (var item in files1)
                {
                    if (item.LastWriteTime.Date == DateTime.Today.Date)
                    {
                        lst.Add(item.Name);
                    }

                }
                foreach (var item in files2)
                {
                    if (item.LastWriteTime.Date == DateTime.Today.Date)
                    {
                        lst.Add(item.Name);
                    }

                }
                return View(lst);
            }
            else
            {
                Session.Clear();
                string path1 = Server.MapPath("~/UploadedFiles/");
                string path2 = Server.MapPath("~/Write_Log/");
                DirectoryInfo dirInfo1 = new DirectoryInfo(path1);
                DirectoryInfo dirInfo2 = new DirectoryInfo(path2);
                FileInfo[] files1 = dirInfo1.GetFiles("*.*");
                FileInfo[] files2 = dirInfo2.GetFiles("*.*");


                List<string> lst = new List<string>(files1.Length);
                foreach (var item in files1)
                {
                    if (item.LastWriteTime.Date == DateTime.Today.Date)
                    {
                        lst.Add(item.Name);
                    }

                }
                foreach (var item in files2)
                {
                    if (item.LastWriteTime.Date == DateTime.Today.Date)
                    {
                        lst.Add(item.Name);
                    }

                }

                return View(lst);
            }
            

           // return View();

        }


        [HttpPost]
        public ActionResult CheckData()
        {
            Session.Clear();
            DataTable dtbl = new DataTable();
            SqlTransaction transaction;
            string constring = ConfigurationManager.ConnectionStrings["Model11"].ToString();
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            transaction = con.BeginTransaction();
            try
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        var DataType = System.Web.HttpContext.Current.Request["DataType"];
                        var ColAgentId = System.Web.HttpContext.Current.Request["ColAgentId"];
                        HttpFileCollectionBase files = Request.Files;
                            HttpPostedFileBase file = files[0];
                            if (file != null)
                            {
                                if (file.ContentType == "application/vnd.ms-excel" || file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                                {
                                    string filename = file.FileName;
                                    string Extension = Path.GetExtension(file.FileName);
                                    if (Extension == ".xlsx" || Extension == ".xls" || Extension == ".csv")
                                    {
                                        string targetpath = Server.MapPath("~/UploadedFiles/");
                                        file.SaveAs(targetpath + filename);
                                        string pathToExcelFile = targetpath + filename;

                                        dtbl = ConvertExcelToDataTable(pathToExcelFile);
                                        var duplicates = dtbl.AsEnumerable().GroupBy(r => r[4]).Where(gr => gr.Count() > 1).ToList();

                                        if(duplicates.Count>0)
                                        {
                                            return Json("Some Duplicate Records Please check file");
                                        }

                                        var cnt1=dtbl.Rows.Count;
                                        string[] columnNames = dtbl.Columns.Cast<DataColumn>()
                                                              .Select(x => x.ColumnName)
                                                              .ToArray();
                                        
                                        ValidExcel(dtbl, columnNames, targetpath + "ExcelLogs_" + DateTime.Now.Day + '_' + DateTime.Now.Month + '_' + DateTime.Now.Year + ".xlsx");
                                       
                                        dtbl.Rows.Cast<DataRow>().ToList().FindAll(Row =>
                                        {
                                            return String.IsNullOrEmpty(String.Join("", Row.ItemArray));
                                        }).ForEach(Row =>
                                        { dtbl.Rows.Remove(Row); });
                                        var cnt2 = dtbl.Rows.Count;
                                        if(cnt1!=cnt2)
                                        {
                                        Session["dtbl"] = dtbl;
                                        Session["DataType"] = DataType;
                                        Session["ColAgentId"] = ColAgentId;
                                        var tbl = "";
                                            if(errorcnt<11)
                                            {
                                                 tbl = "<table class='table-bordered'><thead><tr><th>Error Message</th><th>Row No</th><th>Column No</th></tr></thead><tfoot>" + error + "</tfoot></table>";
                                            }
                                           
                                            return Json("C, "+cnt2+"  Record Valid out of  "+cnt1+"  record,"+ tbl + "");
                                        }
                                        else
                                        {
                                        Session["dtbl"] = dtbl;
                                        Session["DataType"] = DataType;
                                        Session["ColAgentId"] = ColAgentId;
                                        return Json("P,"+dtbl.Rows.Count + " Records Uploaded Successfully");
                                        }
                                        
                                    }
                                    else
                                    {
                                        return Json("Uploading Wrong Excelsheet!");
                                    }
                                }
                                else
                                {
                                    return Json("Choose excel file only");
                                }
                            }
                            else
                            {
                                return Json("Please Select File");

                            }
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json("No files selected.");
                }
            }
            catch (Exception error)
            {
                ViewBag.Message = " Inserting  duplicate  ";

                transaction.Rollback();
                ViewBag.Message = error.Message;
                throw error;
            }
        }


        public void UploadSapConsumerDetails(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                row.SetModified();
            }

            string connectionString = ConfigurationManager.ConnectionStrings["Model11"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                try
                {
                    adapter.UpdateCommand = new SqlCommand { CommandType = CommandType.StoredProcedure, CommandText = "[USP_SaveUploadedData]", Connection = connection };
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Product", DbType = DbType.String, SourceColumn = "PRODUCT" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@ProductType", DbType = DbType.String, SourceColumn = "PRODUCT TYPE" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@FacilityType", DbType = DbType.String, SourceColumn = "FACILITY TYPE" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@CustId", DbType = DbType.Int64, SourceColumn = "CUST ID" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@LoanNo", DbType = DbType.Int64, SourceColumn = "Loan No" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@CustomerName", DbType = DbType.String, SourceColumn = "CUSTOMER NAME" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Address", DbType = DbType.String, SourceColumn = "ADDRESSS" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@City", DbType = DbType.String, SourceColumn = "CITY" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Location", DbType = DbType.String, SourceColumn = "LOCATION" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Phone1", DbType = DbType.String, SourceColumn = "phone 1" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Phone2", DbType = DbType.String, SourceColumn = "phone 2" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@NewContact", DbType = DbType.String, SourceColumn = "New Contact No#" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@GuarantorName", DbType = DbType.String, SourceColumn = "Guarantor Name" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@GuarnatorMobileNo", DbType = DbType.String, SourceColumn = "Guarantor Mno" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Pay_Freq", DbType = DbType.String, SourceColumn = "Frequency / Payment Frequency" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@BillingCycle", DbType = DbType.String, SourceColumn = "Billing Cycle" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@OverdueAmt", DbType = DbType.Decimal, SourceColumn = "Overdue Amt" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@EMI", DbType = DbType.Decimal, SourceColumn = "EMI" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@TotalEMiDue", DbType = DbType.Decimal, SourceColumn = "Total EMI Due" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@CBC_LLP", DbType = DbType.Decimal, SourceColumn = "CBC + LPP" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@CBC", DbType = DbType.Decimal, SourceColumn = "CBC" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@LLP", DbType = DbType.Decimal, SourceColumn = "LPP" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Principal_OTSD", DbType = DbType.Decimal, SourceColumn = "Principal Outstanding (POS)" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Total_OTSD", DbType = DbType.Decimal, SourceColumn = "Total Outstanding(TOS)" });

                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@CUST_EXPO", DbType = DbType.String, SourceColumn = "CUST_EXPO" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@POS_I_Cr", DbType = DbType.Decimal, SourceColumn = "POS in Crore" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Norms", DbType = DbType.String, SourceColumn = "Norms" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@NPA_Stage", DbType = DbType.String, SourceColumn = "NPA STAGE" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@DPD", DbType = DbType.Decimal, SourceColumn = "DPD" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Bucket", DbType = DbType.String, SourceColumn = "BUCKET" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Pool", DbType = DbType.String, SourceColumn = "POOL" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@User_E_Code", DbType = DbType.Int64, SourceColumn = "User E-CODE" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@UserName", DbType = DbType.String, SourceColumn = "User Name" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@ContactNo", DbType = DbType.String, SourceColumn = "Contact no" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@TBS_TL_EMP_Code", DbType = DbType.Int64, SourceColumn = "TBSS TL EMP CODE" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@TL_Name", DbType = DbType.String, SourceColumn = "TL NAME" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@UserId", DbType = DbType.String, SourceColumn = "User id" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Action", DbType = DbType.String, SourceColumn = "Action" });
                    adapter.UpdateCommand.Parameters.Add(new SqlParameter { ParameterName = "@Coll_Agent_Id", DbType = DbType.String, SourceColumn = "Coll_Agent_Id" });
                    adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                    adapter.UpdateCommand.CommandTimeout = 1800;
                    adapter.AcceptChangesDuringUpdate = true;
                    adapter.InsertCommand = null;
                    adapter.DeleteCommand = null;
                    adapter.UpdateCommand.CommandTimeout = 300;
                    adapter.Update(dt);
                }
                catch (Exception error)
                {
                    throw error;
                }
                finally
                {
                    connection.Close();
                    adapter = null;
                }
            }
            Session.Clear();
        }

        public static DataTable ConvertExcelToDataTable(string FileName)
        {
            DataTable dtResult = null;
            int totalSheet = 0;
            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tempDataTable = (from dataRow in dt.AsEnumerable()
                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                         select dataRow).CopyToDataTable();
                    dt = tempDataTable;
                    totalSheet = dt.Rows.Count;
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();
                return dtResult;
            }
        }

        int cnt = 1;
        int rnno = 1;
        string error;
        int errorcnt = 0;
        public void WriteLog(string strMessage, int rowNo, int colNo)
        {
            error += "<tr><td>"+ strMessage + "</td><td>"+ rowNo + "</td><td>"+colNo+"</td></tr>";
            string strLogPath = Server.MapPath("~/Write_Log/");
            try
            {
                if (cnt != rowNo)
                {
                    cnt = rowNo;
                    rnno++;
                }
                else
                {
                    rnno = 0;
                }
                if (!System.IO.Directory.Exists(strLogPath))
                    System.IO.Directory.CreateDirectory(strLogPath);

                string strFileName = "ExcelLogs_" + DateTime.Now.Day +'_'+ DateTime.Now.Month +'_'+ DateTime.Now.Year + ".log";

                FileStream fs = new FileStream(strLogPath + "\\" + strFileName,
                                    FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                if (rowNo > 1 && rnno > 0)
                {
                    m_streamWriter.WriteLine("\n=======================================================================================================================================================");
                }
                m_streamWriter.WriteLine("   RowNo_" + rowNo + ":ColumnNo_" + colNo + "_____" + strMessage + "____________Time:" + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss tt"));
                m_streamWriter.WriteLine("\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            errorcnt++;
        }


        public DataTable ValidExcel(DataTable dt, string[] columnNames,string excelFilePath = null)
        {

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                worksheet.Cells["A1:AK1"].Style.Font.Bold = true;
                worksheet.Cells["A1:AK1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:AK1"].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

//--------------------------------------------Excel Headers--------------------------------------------------//
                int columncnt = 1;
            string[] headernames = { "PRODUCT", "PRODUCT TYPE", "FACILITY TYPE", "CUST ID", "Loan No", "CUSTOMER NAME", "ADDRESSS",
                                    "CITY","LOCATION","phone 1","phone 2","New Contact No#","Guarantor Name","Guarantor Mno","Frequency / Payment Frequency","Billing Cycle",
                                    "Overdue Amt","EMI","Total EMI Due","CBC + LPP","CBC","LPP","Principal Outstanding (POS)","Total Outstanding(TOS)","CUST_EXPO","POS in Crore",
                                    "Norms","NPA STAGE","DPD","BUCKET","POOL","User E-CODE","User Name","Contact no","TBSS TL EMP CODE","TL NAME","User id"};

            var diff1 = columnNames.Except(headernames);
            var diff2 = headernames.Except(columnNames);


            if (diff1 != diff2)
            {
                foreach (var s in diff1)
                {
                    int a = Array.IndexOf(columnNames, s)+1;
                    WriteLog(s + "_Additional Header Column Added", 1, a);
                        worksheet.Cells[1, a].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, a].Style.Fill.BackgroundColor.SetColor(Color.Red);
                }

                foreach (var s in diff2)
                {
                    int a = Array.IndexOf(headernames, s)+1;
                    WriteLog(s + "_Missing Header Column", 1, a);
                        worksheet.Cells[1, a].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, a].Style.Fill.BackgroundColor.SetColor(Color.Red);
                }

            }
            else
            {
                foreach (var column in columnNames)
                {

                    if (headernames.Contains(column))
                    {
                        // WriteLog(column + "Column Name Correct", 1, columncnt);
                    }
                    else
                    {
                        var msg = column;
                        WriteLog(column + "Wrong Header", 1, columncnt);
                            worksheet.Cells[1, columncnt].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[1, columncnt].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    }
                }

                columncnt++;
            }



            //---------------------------------------------------Excel Columns--------------------------------------------------------------------------
            var rowcnt = 2;
            //var columncnt = 1;
           
                List<DataRow> listRowsToDelete = new List<DataRow>();
                try
                {

                    foreach (DataRow dr in dt.Rows)
                    {

                        var flag = true;
                        Int64 n;
                        float f;
                        bool phone1 = Int64.TryParse(Convert.ToString(dr["phone 1"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["phone 1"]), @"\s+", ""), out n);
                        if (phone1 == false)
                        {
                            WriteLog("phone 1 must be numeric/not valid format", rowcnt, 10);
                            worksheet.Cells[rowcnt, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 10].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        else if (Regex.Replace(Convert.ToString(dr["phone 1"]), @"\s+", "").ToString() != "0" && Regex.Replace(Convert.ToString(dr["phone 1"]), @"\s+", "").ToString() != "" && Regex.Replace(Convert.ToString(dr["phone 1"]), @"\s+", "").ToString().Length != 10)
                        {
                            WriteLog("phone 1 must be 10 digit only", rowcnt, 10);
                            worksheet.Cells[rowcnt, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 10].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;

                        }
                        bool phone2 = Int64.TryParse(Convert.ToString(dr["phone 2"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["phone 2"]), @"\s+", ""), out n);
                        if (phone2 == false)
                        {
                            WriteLog("phone 2 must be numeric", rowcnt, 11);
                            worksheet.Cells[rowcnt, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 11].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        else if (Regex.Replace(Convert.ToString(dr["phone 2"]), @"\s+", "").ToString() != "0" && Regex.Replace(Convert.ToString(dr["phone 2"]), @"\s+", "").ToString() != "" && Regex.Replace(Convert.ToString(dr["phone 2"]), @"\s+", "").ToString().Length != 10)
                        {
                            WriteLog("phone 2 must be 10 digit only", rowcnt, 11);
                            worksheet.Cells[rowcnt, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 11].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool newcontactno = Int64.TryParse(Convert.ToString(dr["New Contact No#"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["New Contact No#"]), @"\s+", ""), out n);
                        if (newcontactno == false)
                        {
                            WriteLog("New Contact No must be numeric", rowcnt, 12);
                            worksheet.Cells[rowcnt, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 12].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        else if (Regex.Replace(Convert.ToString(dr["New Contact No#"]), @"\s+", "").ToString() != "0" && Regex.Replace(Convert.ToString(dr["New Contact No#"]), @"\s+", "").ToString() != "" && Regex.Replace(Convert.ToString(dr["New Contact No#"]), @"\s+", "").ToString().Length != 10)
                        {
                            WriteLog("New Contact must be 10 digit only", rowcnt, 12);
                            worksheet.Cells[rowcnt, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 12].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool ovrdueamt = float.TryParse(Convert.ToString(dr["Overdue Amt"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["Overdue Amt"]), @"\s+", ""), out f);
                        if (ovrdueamt == false)
                        {
                            WriteLog("Overdue Amt Due must be numeric/float", rowcnt, 17);
                            worksheet.Cells[rowcnt, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 17].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool emi = float.TryParse(Convert.ToString(dr["EMI"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["EMI"]), @"\s+", ""), out f);
                        if (emi == false)
                        {
                            WriteLog("EMI Due must be numeric/float", rowcnt, 18);
                            worksheet.Cells[rowcnt, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 18].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool ttlemidue = float.TryParse(Convert.ToString(dr["Total EMI Due"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["Total EMI Due"]), @"\s+", ""), out f);
                        if (ttlemidue == false)
                        {
                            WriteLog("Total EMI Due must be numeric/float", rowcnt, 19);
                            worksheet.Cells[rowcnt, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 19].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool cbc = float.TryParse(Convert.ToString(dr["CBC"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["CBC"]), @"\s+", ""), out f);
                        if (cbc == false)
                        {
                            WriteLog("CBC must be numeric/float", rowcnt, 21);
                            worksheet.Cells[rowcnt, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 21].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool llp = float.TryParse(Convert.ToString(dr["LPP"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["LPP"]), @"\s+", ""), out f);
                        if (llp == false)
                        {
                            WriteLog("LPP must be numeric/float", rowcnt, 22);
                            worksheet.Cells[rowcnt, 22].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 22].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool cbcllp = float.TryParse(Convert.ToString(dr["CBC + LPP"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["CBC + LPP"]), @"\s+", ""), out f);
                        if (cbcllp == false)
                        {
                            WriteLog("CBC + LPP must be numeric/float", rowcnt, 20);
                            worksheet.Cells[rowcnt, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 20].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool proutsndg = float.TryParse(Convert.ToString(dr["Principal Outstanding (POS)"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["Principal Outstanding (POS)"]), @"\s+", ""), out f);
                        if (proutsndg == false)
                        {
                            WriteLog("Principal Outstanding (POS) must be numeric/float", rowcnt, 23);
                            worksheet.Cells[rowcnt, 23].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 23].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool ttloutstndg = float.TryParse(Convert.ToString(dr["Total Outstanding(TOS)"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["Total Outstanding(TOS)"]), @"\s+", ""), out f);
                        if (ttloutstndg == false)
                        {
                            WriteLog("Total Outstanding(TOS) must be numeric/float", rowcnt, 24);
                            worksheet.Cells[rowcnt, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 24].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool poscr = float.TryParse(Convert.ToString(dr["POS in Crore"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["POS in Crore"]), @"\s+", ""), out f);
                        if (poscr == false)
                        {
                            WriteLog("POS in Crore must be numeric/float", rowcnt, 26);
                            worksheet.Cells[rowcnt, 26].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 26].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool contactno = Int64.TryParse(Convert.ToString(dr["Contact no"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["Contact no"]), @"\s+", ""), out n);
                        if (contactno == false)
                        {
                            WriteLog("Contact no must be numeric", rowcnt, 34);
                            worksheet.Cells[rowcnt, 34].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 34].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        else if (Regex.Replace(Convert.ToString(dr["Contact no"]), @"\s+", "").ToString() != "0" && Regex.Replace(Convert.ToString(dr["Contact no"]), @"\s+", "").ToString() != "" && Regex.Replace(Convert.ToString(dr["Contact no"]), @"\s+", "").ToString().Length != 10)
                        {
                            WriteLog("Contact no must be 10 digit only", rowcnt, 34);
                            worksheet.Cells[rowcnt, 34].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 34].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool custid = Int64.TryParse(Convert.ToString(dr["CUST ID"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["CUST ID"]), @"\s+", ""), out n);
                        if (custid == false)
                        {
                            WriteLog("CUST ID must be numeric", rowcnt, 4);
                            worksheet.Cells[rowcnt, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 4].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        bool loanno = Int64.TryParse(Convert.ToString(dr["Loan No"]) == "" ? "0" : Regex.Replace(Convert.ToString(dr["Loan No"]), @"\s+", ""), out n);
                        if (loanno == false)
                        {
                            WriteLog("Loan No must be numeric", rowcnt, 5);
                            worksheet.Cells[rowcnt, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[rowcnt, 5].Style.Fill.BackgroundColor.SetColor(Color.Red);
                            flag = false;
                        }
                        if (flag == false)
                        {
                            listRowsToDelete.Add(dr);
                        }
                        rowcnt++;
                        //columncnt++;
                    }

                    foreach (DataRow drowToDelete in listRowsToDelete)
                    {
                        dt.Rows.Remove(drowToDelete);
                    }


                    FileInfo fi = new FileInfo(excelFilePath);
                    excelPackage.SaveAs(fi);
                    return dt;

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

        }


        public ActionResult CaseTransfer()
        {
            return View();
        }

        public ActionResult CaseList()
        {
            return View();
        }

        public ActionResult GetCaseDetails(string ColAgentId1)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
            cmd = new SqlCommand("Get_Assign_Case_Details_List", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", ColAgentId1);
            con.Open();
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            con.Close();
            da.Dispose();

            List<UploadData> lstUploadData = new List<UploadData>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    UploadData objUploadData = new UploadData();
                    objUploadData.Id = Convert.ToInt32(dr["Id"]);
                    objUploadData.LoanNo = Convert.ToInt32(dr["LoanNo"]);
                    objUploadData.CustomerName = Convert.ToString(dr["CustomerName"]);
                    objUploadData.Address = Convert.ToString(dr["Address"]);
                    objUploadData.TL_Name = Convert.ToString(dr["TL_Name"]);
                    objUploadData.UserId = Convert.ToString(dr["UserId"]);
                    objUploadData.ContactNo = Convert.ToString(dr["ContactNo"]);
                    lstUploadData.Add(objUploadData);
                }
            }
            return View("CaseList", lstUploadData);
        }



        [HttpPost]
        public string CaseTransfer(string ColAgentId2,string Ids)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
            cmd = new SqlCommand("CallTransfer", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", ColAgentId2);
            cmd.Parameters.AddWithValue("@Ids", Ids);
            con.Open();
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            con.Close();
            da.Dispose();
            return JsonConvert.SerializeObject(dt);
        }


        public string GetUserData()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Model11"].ToString());
            cmd = new SqlCommand("GetUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            con.Open();
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            da.Dispose();
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Columns["Id"].ColumnName = "Id";
                dt.Columns["Name"].ColumnName = "Name";
                return DataTableToJSONNew(dt);
            }
            else
            {
                return DataTableToJSONNew(dt);
            }
        }

        public static string DataTableToJSONNew(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(list);
        }

    }
}