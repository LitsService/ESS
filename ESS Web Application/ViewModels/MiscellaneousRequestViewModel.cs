using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class MiscellaneousRequestViewModel
    {
        public string RefNo { get; set; }
        public string RequestDate { get; set; }
        public string Employee { get; set; }
        public string ReqStatus { get; set; }
        public string EmployeeId { get; set; }
        public string MType { get; set; }
        public string PType { get; set; }
        public string Language { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public string Opening { get; set; }
        public string Salary { get; set; }
        public string Department { get; set; }
        public string Report { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public string History { get; set; }
        public string Description { get; internal set; }
        public string Approver { get; internal set; }
        public string ApprovedBy { get; internal set; }
        public string AttachmentGuid { get; set; }
        public string RequestId { get; set; }
        public string CompanyName { get;  set; }
    }
    public class MiscellaneousRequestListViewModel
    {
        public MiscellaneousRequestListViewModel()
        {
            History = new List<MiscellaneousRequestViewModel>();
            Approval = new List<MiscellaneousRequestViewModel>();
        }
        public bool IsApproval { get; set; }
        public List<MiscellaneousRequestViewModel> History { get; set; }
        public List<MiscellaneousRequestViewModel> Approval { get; set; }
    }
}