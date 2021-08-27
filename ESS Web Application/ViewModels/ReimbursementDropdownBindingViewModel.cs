using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class ReimbursementDropdownBindingViewModel
    {
        public List<DropDownBindViewModel> Countries { get; set; }
        public List<DropDownBindViewModel> ReimbursementType { get; set; }
        public List<DropDownBindViewModel> CurrencyCode { get; set; }
        public List<DropDownBindViewModel> ActivityTypes { get; set; }


    }
    public class ReimbursementListViewModel
    {
        public ReimbursementListViewModel()
        {
            History = new List<ReimbursementDataListViewModel>();
            Approval = new List<ReimbursementDataListViewModel>();
        }
        public bool IsApproval { get; set; }
        public string ProfileImage { get; set; }
        public List<ReimbursementDataListViewModel> History { get; set; }
        public List<ReimbursementDataListViewModel> Approval { get; set; }
    }
    public class ReimbursementDataListViewModel
    {
        public string ReimbursementStatusID { get; set; }
        public string Ref { get; set; }
        public string ReimbursementType { get; set; }
        public string SubmittedBy { get; set; }
        public string EmployeeId { get; set; }
        public string RequestedDate { get; set; }
        public string TrxId { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public string AttachmentGuid { get; set; }
        public string RequestID { get; set; }
        public string CompanyName { get;  set; }
    }
    public class ReimbursementEditViewModel
    {
        public string ImbursementId { get; set; }
        public string RMTYPE { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ActivityType { get; set; }
        public string ActivityId { get; set; }
        public string Reciept { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string SubmittedBy { get; set; }
        public string Remarks { get; set; }
        public string DESIGNATION { get; set; }
        public string DEPRTMNT { get; set; }
        public ReimbursementDropdownBindingViewModel dropdowns { get; set; }
    }
    public class LeaveViewModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DEPRTMNT { get; set; }
        public string DESIGNATION { get; set; }
        public string LeaveType { get; set; }
        public string TotalLeaves { get; set; }
        public string AvailLeaves { get; set; }
        public string RemainingLeaves { get; set; }

    }
}