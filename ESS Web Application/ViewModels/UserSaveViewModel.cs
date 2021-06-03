using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class UserSaveViewModel
    {
        public string Password { get; set; }
        public string EmployeeID { get; set; }
        public int ID { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string DBMessage { get; set; }
        public HttpPostedFileBase ImgBody { get; set; }
    }
}