using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESS_Web_Application.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Payslip()
        {
            return View();
        }
    }
}