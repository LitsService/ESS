using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class WorkFlowViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormTypeId { get; set; }
        public string FormType { get; set; }
        public bool IsActive { get; set; }
        public string HrEmployeeId { get; set; }
    }
}