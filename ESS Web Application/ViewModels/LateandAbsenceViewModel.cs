using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class LateandAbsenceViewModel
    {
        public string RefNo { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Date { get; set; }
        public string Approver { get; set; }
        public string ReqStatus { get; set; }
        public string EmployeeId { get; set; }
        public string Employee { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public string PunchIn { get; set; }
        public string PunchOut { get; set; }
        public string AttachmentGuid { get; set; }
        public string RequestID { get; set; }
        public string CompanyName { get;  set; }
    }
    public class LateandAbsenceListViewModel
    {
        public LateandAbsenceListViewModel()
        {
            History = new List<LateandAbsenceViewModel>();
            Approval = new List<LateandAbsenceViewModel>();
        }
        public bool IsApproval { get; set; }
        public List<LateandAbsenceViewModel> History { get; set; }
        public List<LateandAbsenceViewModel> Approval { get; set; }
    }
}