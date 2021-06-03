using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class ReimbursementSaveViewModel
    {
        public string ReimbursementType { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string City { get; set; }
        public string ActivityType { get; set; }
        public string ActivityId { get; set; }
        public string Reciept { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }

    }
   
}