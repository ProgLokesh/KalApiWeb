using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KalWeb.Models
{
    public class FeedbackTransaction
    {
        public int Fbid { get; set; }
        public int Uid { get; set; }
        public string Username { get; set; }
        public string AuthCode { get; set; }
        public string latlong { get; set; }
        public string caseid { get; set; }
        public string loanno { get; set; }
        public string Transactionid { get; set; }
        public int ActionCode { get; set; }
        public int ActionFdback { get; set; }
        public int PersonContact { get; set; }
        public int PlaceOfContact { get; set; }
        public string MobileNo { get; set; }
        public string VisitRemark { get; set; }
        public int amt { get; set; }
        public int PayMode { get; set; }
        public string BankName { get; set; }
        public string ChqNo { get; set; }
        public string ChqDate { get; set; }
        public string IFSCCode { get; set; }
        public string RefNo { get; set; }

        public string RefPaydate { get; set; }
        public string PTPDate { get; set; }
        public string PTPTime { get; set; }
        public int PTPStatus { get; set; }
        public int TotalVisitNo { get; set; }
        public int AgentVisitNo { get; set; }

        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }

        public int Status { get; set; }
        public string Photo { get; set; }

    }
}