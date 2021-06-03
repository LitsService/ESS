using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class UserViewModel
    {
        public string EmployeeId { get; set; }
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> CompanyIds { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}