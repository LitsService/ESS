using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class WorkFlowSubListViewModel
    {
        public string ID { get; set; }
        public string ApproverName { get; set; }
        public string ApproverLevel { get; set; }
        public string ApproverGraceDays { get; set; }
        public string ProxyApproverName { get; set; }
        public string ApproverId { get; set; }
        public string ProxyApproverId { get; set; }
        public string WorkFlowSubId { get; set; }
    }
}