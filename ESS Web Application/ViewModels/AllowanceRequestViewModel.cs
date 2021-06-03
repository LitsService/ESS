using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class AllowanceRequestViewModel
    {
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public string RequestId { get; set; }
        public string Employee { get; set; }
        public string EmployeeId { get; set; }
        public string History { get; internal set; }
        public bool IsEditViisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public string ReqStatus { get; set; }
        public string AttachmentGuid { get; set; }
        public string RequestLoaclID { get; set; }
        public string CompanyName { get;  set; }
    }
    public class AllowanceRequestListViewModel
    {
        public AllowanceRequestListViewModel()
        {
            History = new List<AllowanceRequestViewModel>();
            Approval = new List<AllowanceRequestViewModel>();
        }
        public bool IsApproval { get; set; }
        public List<AllowanceRequestViewModel> History { get; set; }
        public List<AllowanceRequestViewModel> Approval { get; set; }
    }

}