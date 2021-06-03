using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class TicketRequestViewModel
    {
        public string RefNo { get; set; }
        public string RequestDate { get; set; }
        public string Employee { get; set; }
        public string ReqStatus { get; set; }
        public string EmployeeId { get; set; }
        public string TicketType { get; set; }
        public string TicketBalance { get; set; }
        public string Remarks { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public string TravelFrom { get; set; }
        public string TravelTo { get; set; }
        public string DateReturn { get; set; }
        public string DateTravel { get; set; }
        public string TravelFromCountry { get; set; }
        public string TravelToCountry { get; set; }
        public string History { get; set; }
        public string Description { get; set; }
        public string Approver { get; set; }
        public string ApprovedBy { get; set; }
        public string AttachmentGuid { get; set; }
        public string RequestID { get; set; }
        public string CompanyName { get; set; }
    }
    public class TicketRequestListViewModel
    {
        public TicketRequestListViewModel()
        {
            History = new List<TicketRequestViewModel>();
            Approval = new List<TicketRequestViewModel>();
        }
        public bool IsApproval { get; set; }
        public List<TicketRequestViewModel> History { get; set; }
        public List<TicketRequestViewModel> Approval { get; set; }
    }
}