using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class StaffExpenseViewModel
    {
        public string Ref { get; set; }
        public string RequestDate { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Remarks { get; set; }
    }
}