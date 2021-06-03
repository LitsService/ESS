using ESS_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public interface IAccountService
    {
        DataTable Login(LoginViewModel model);
    }
}