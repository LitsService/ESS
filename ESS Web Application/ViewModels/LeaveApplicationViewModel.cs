using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class LeaveApplicationViewModel
    {
        public bool isResumeDuty { get; set; }

        public string RefNo { get; set; }
        public string LeaveType { get; set; }
        public string RequestDate { get; set; }
        public string Employee { get; set; }
        public string Approver { get; set; }
        public string ReqStatus { get; set; }
        public string EmployeeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string NoofDays { get; set; }
        public string Leavebalance { get; set; }
        public string Remarks { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public string ReplacementEmployee { get; set; }
        public string CalendarDays { get; set; }
        public string Description { get; set; }
        public string AttachmentGuid { get; set; }
        public string RequestID { get; set; }
        public string CompanyName { get; set; }
    }
    public class LeaveApplicationListViewModel
    {
        public LeaveApplicationListViewModel()
        {
            History = new List<LeaveApplicationViewModel>();
            Approval = new List<LeaveApplicationViewModel>();
        }
        public bool IsApproval { get; set; }
        public string ProfileImage { get; set; }
        public List<LeaveApplicationViewModel> History { get; set; }
        public List<LeaveApplicationViewModel> Approval { get; set; }
    }
}