using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class DropDownBindViewModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class Company
    {
        public int CompanyID { get; set; }
        public Guid CompanyGuid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}