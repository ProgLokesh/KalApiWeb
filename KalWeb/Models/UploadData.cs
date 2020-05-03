namespace KalWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UploadData")]
    public partial class UploadData
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Product { get; set; }

        [StringLength(200)]
        public string ProductType { get; set; }

        [StringLength(200)]
        public string FacilityType { get; set; }

        public int? CustId { get; set; }

        public int? LoanNo { get; set; }

        [StringLength(150)]
        public string CustomerName { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Phone1 { get; set; }

        [StringLength(50)]
        public string Phone2 { get; set; }

        [StringLength(50)]
        public string NewContact { get; set; }

        [StringLength(150)]
        public string GuarantorName { get; set; }

        [StringLength(50)]
        public string GuarnatorMobileNo { get; set; }

        [StringLength(50)]
        public string BillingCycle { get; set; }

        public decimal? OverdueAmt { get; set; }

        public decimal? EMI { get; set; }

        public decimal? TotalEMiDue { get; set; }

        public decimal? CBC_LLP { get; set; }

        public decimal? CBC { get; set; }

        public decimal? LLP { get; set; }

        public decimal? Total_OTSD { get; set; }

        public decimal? Principal_OTSD { get; set; }

        [StringLength(150)]
        public string CUST_EXPO { get; set; }

        public decimal? POS_I_Cr { get; set; }

        [StringLength(150)]
        public string Norms { get; set; }

        [StringLength(50)]
        public string NPA_Stage { get; set; }

        public decimal? DPD { get; set; }

        [StringLength(50)]
        public string Bucket { get; set; }

        [StringLength(50)]
        public string Pool { get; set; }

        public int? User_E_Code { get; set; }

        [StringLength(150)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }

        public int? TBS_TL_EMP_Code { get; set; }

        [StringLength(150)]
        public string TL_Name { get; set; }

        [StringLength(100)]
        public string UserId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(1)]
        public string IsActive { get; set; }

        [StringLength(20)]
        public string Pay_Freq { get; set; }
    }
}
