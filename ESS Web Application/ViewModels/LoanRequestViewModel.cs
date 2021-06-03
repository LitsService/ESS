using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class LoanRequestViewModel
    {
        public string RefNo { get; set; }
        public string RequestDate { get; set; }
        public string Employee { get; set; }
        public string ReqStatus { get; set; }
        public string EmployeeId { get; set; }
        public string LoanType { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }
        public string ExpectedDate { get; set; }
        public string Installments { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public string History { get; set; }
        public string RequestID { get; set; }
        public string CompanyName { get; set; }
    }
    public class LoanRequestListViewModel
    {
        public LoanRequestListViewModel()
        {
            History = new List<LoanRequestViewModel>();
            Approval = new List<LoanRequestViewModel>();
        }
        public bool IsApproval { get; set; }
        public string ProfileImage { get; set; }
        public List<LoanRequestViewModel> History { get; set; }
        public List<LoanRequestViewModel> Approval { get; set; }
    }
}