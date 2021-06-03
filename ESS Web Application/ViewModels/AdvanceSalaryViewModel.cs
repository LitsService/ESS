using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class AdvanceSalaryViewModel
    {
        public string CompanyName { get; set; }
        public string RefNo { get; set; }
        public string RequestDate { get; set; }
        public string Employee { get; set; }
        public string PayPeriod { get; set; }
        public string ReqStatus { get; set; }
        public string EmployeeId { get; set; }
        public string LoanType { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public string RequqestId { get; internal set; }
    }
    public class AdvanceSalaryListViewModel
    {
        public AdvanceSalaryListViewModel()
        {
            History = new List<AdvanceSalaryViewModel>();
            Approval = new List<AdvanceSalaryViewModel>();
        }
        public bool IsApproval { get; set; }
        public string ProfileImage { get; set; }
        public List<AdvanceSalaryViewModel> History { get; set; }
        public List<AdvanceSalaryViewModel> Approval { get; set; }
    }


}