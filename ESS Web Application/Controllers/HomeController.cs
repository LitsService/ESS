using ESS_Web_Application.Entity;
using ESS_Web_Application.Helper;
using ESS_Web_Application.Infrastructure;
using ESS_Web_Application.Repository;
using ESS_Web_Application.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESS_Web_Application.Controllers
{
    [AuthorizeActionFilter]
    public class HomeController : Controller
    {
        enum Gender { Male, Female };
        enum Employee { New, All };
        IWorkflowsService _workflow = new WorkflowsService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                DataTable dt = new DataTable();

                dt = this.GetAllEmployeesWithCompany(Session["UserCompanyID"].ToString());

                ViewBag.maleCount = this.GetEmployeeGenderCount(dt, Gender.Male);
                ViewBag.femaleCount = this.GetEmployeeGenderCount(dt, Gender.Female);
                ViewBag.NewEmpCount = this.GetEmployeeCount(dt, Employee.New);
                ViewBag.AllEmpCount = this.GetEmployeeCount(dt, Employee.All);
                ViewBag.SearchFilterData = _workflow.GetFormName();
                ViewBag.NotificationCount = GetNotifications().Count();
                return View();
            }
        }

        private double GetEmployeeGenderCount(DataTable _dt, Gender _gender)
        {
            double numberOfRecords = 0;
            double employeeAll = 0;
            double numberOfRecordsPercent = 0.00;
            if (_gender == Gender.Male)
            {
                numberOfRecords = _dt.AsEnumerable().Where(x => x["GENDER"].ToString() == "1").ToList().Count;
            }
            else
            {
                numberOfRecords = _dt.AsEnumerable().Where(x => x["GENDER"].ToString() == "0").ToList().Count;
            }

            employeeAll = this.GetEmployeeCount(_dt, Employee.All);
            numberOfRecordsPercent = (numberOfRecords / employeeAll) * 100;
            return Math.Round(numberOfRecordsPercent, 1);
        }
        private int GetEmployeeCount(DataTable _dt, Employee _employee)
        {
            int numberOfRecords = 0;

            if (_employee == Employee.New)
            {
                DateTime date = Convert.ToDateTime(DateTime.Now.AddDays(-30).ToShortDateString());

                foreach (DataRow row in _dt.Rows)
                {
                    if (Convert.ToDateTime(row["STRTDATE"].ToString()) >= date)
                    {
                        numberOfRecords++;
                    }
                }
            }
            else
            {
                numberOfRecords = _dt.AsEnumerable().ToList().Count;
            }

            return numberOfRecords;
        }
        private DataTable GetAllEmployeesWithCompany(string CompanyId)
        {
            ESS_Web_Application.Repository.Repository repository = new Repository.Repository();

            Hashtable ht = new Hashtable();
            ht.Add("@DATAAREAID", CompanyId);

            return repository.GetAllEmployeesWithCompany(ht);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ReimbursementForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AdvanceSalaryApplicationFrom()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LoanApplicationForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TicketRequestForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MiscellaneousApplicationForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LateandAbsenceJustApplicationForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AllownceRequestForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AssetManagementReqForm()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult EmployeeAttendanceDeatils()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetNotificationData()
        {
            var DocList = GetEmployeeDocList();
            var notification = GetNotifications();
            NotificationsViewModel list = new NotificationsViewModel();
            byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
            if (bytes.Length > 1)
            {
                string imgname = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                list.ProfileImg = imgname;
            }
            list.Notification = notification;
            list.DocumentList = DocList;
            return Json(list);
        }
        public List<EmpDocument> GetEmployeeDocList()
        {
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", Session["UserID"].ToString());
            ht.Add("@CompanyId", Session["UserCompanyID"].ToString());
            DataTable dt = DBContext.GetDataSet("sp_User_GetEmployeeDocument", ht).Tables[0];

            List<EmpDocument> Outstanding = new List<EmpDocument>();
            foreach (DataRow item in dt.Rows)
            {
                EmpDocument single = new EmpDocument()
                {
                    ID = item["ID"].ToString(),
                    DOCID = item["DOCID"].ToString(),
                    DocType = item["DocType"].ToString(),
                    LeaveType = item["LeaveType"].ToString(),
                    Name = item["Name"].ToString(),
                };
                Outstanding.Add(single);
            }
            return Outstanding;
        }
        public static byte[] GetImageFromDB(string UserID)
        {
            byte[] ImageByteArray = null;

            Hashtable ht = new Hashtable();
            ht.Add("@UserID", UserID);

            DataTable dt = DBContext.GetDataSet("sp_GetEmployeeImage", ht).Tables[0];

            foreach (DataRow RW in dt.Rows)
            {
                ImageByteArray = (byte[])RW["ImgBody"];
            }
            return ImageByteArray;
        }
        public List<Notifications> GetNotifications()
        {
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", Session["UserID"].ToString());

            DataTable dt = DBContext.GetDataSet("sp_User_Get_Notifications", ht).Tables[0];

            List<Notifications> History = new List<Notifications>();
            DataTable dt2 = dt.AsEnumerable().GroupBy(row => row.Field<Int64>("ID")).Select(group => group.Last()).CopyToDataTable();
            foreach (DataRow item in dt2.Rows)
            {
                Notifications single = new Notifications()
                {
                    Date = item["RequestDate"].ToString(),
                    EmpId = item["EMPLOYID"].ToString(),
                    EmpName = item["EMPNAME"].ToString(),
                    ReqType = item["RequestType"].ToString(),
                    Ref = item["RedirectURL"].ToString(),
                    App = item["ApproveRejectURL"].ToString(),
                    GraceDays = item["ApprovalGraceDays"].ToString(),
                    DaysAfterRequest = item["DaysExpend"].ToString()

                };
                History.Add(single);
            }
            return History;
        }
        public JsonResult GetRequestCounts()
        {
            var Data = GetNotifications();
            var Request = Data.Where(x => x.ReqType.Contains("Leave")).Count();
            var Expiry = Data.Where(x => x.ReqType.Contains("Leave") && ((DateTime.Now.Date - DateTime.Parse(x.Date).Date).TotalDays /*- int.Parse(x.GraceDays)*/) <= 1).Count();
            Request r = new Request();
            r.LeaveRequest = new RequestCount();
            r.LeaveRequest.PendingApproval = Request.ToString();
            r.LeaveRequest.Expiring = Expiry.ToString();

            Request = Data.Where(x => x.ReqType.Contains("Reimbursement")).Count();
            Expiry = GetNotifications().Where(x => x.ReqType.Contains("Reimbursement") && ((DateTime.Now.Date - DateTime.Parse(x.Date).Date).TotalDays /*- int.Parse(x.GraceDays)*/) <= 1 /*int.Parse(x.GraceDays)*/).Count();
            r.Reimbursement = new RequestCount();
            r.Reimbursement.PendingApproval = Request.ToString();
            r.Reimbursement.Expiring = Expiry.ToString();

            Request = Data.Where(x => x.ReqType.Contains("Miscellaneous")).Count();
            Expiry = GetNotifications().Where(x => x.ReqType.Contains("Miscellaneous") && ((DateTime.Now.Date - DateTime.Parse(x.Date).Date).TotalDays /*- int.Parse(x.GraceDays)*/) <= 1 /*int.Parse(x.GraceDays)*/).Count();
            r.Miscellaneous = new RequestCount();
            r.Miscellaneous.PendingApproval = Request.ToString();
            r.Miscellaneous.Expiring = Expiry.ToString();

            Request = Data.Where(x => x.ReqType.Contains("Late & Absence")).Count();
            Expiry = GetNotifications().Where(x => x.ReqType.Contains("Late & Absence") && ((DateTime.Now.Date - DateTime.Parse(x.Date).Date).TotalDays /*- int.Parse(x.GraceDays)*/) <= 1 /*int.Parse(x.GraceDays)*/).Count();
            r.Late_and_Absence = new RequestCount();
            r.Late_and_Absence.PendingApproval = Request.ToString();
            r.Late_and_Absence.Expiring = Expiry.ToString();

            Request = Data.Where(x => x.ReqType.Contains("Allowance")).Count();
            Expiry = GetNotifications().Where(x => x.ReqType.Contains("Allowance") && ((DateTime.Now.Date - DateTime.Parse(x.Date).Date).TotalDays/* - int.Parse(x.GraceDays)*/) <= 1 /*int.Parse(x.GraceDays)*/).Count();
            r.Allowance = new RequestCount();
            r.Allowance.PendingApproval = Request.ToString();
            r.Allowance.Expiring = Expiry.ToString();
            return Json(r);
        }
        public JsonResult GetApproved_UnApprovedRequestCounts(string FormName)
        {
            RequestCount r = new RequestCount();
            if (!string.IsNullOrEmpty(FormName))
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", Session["UserID"].ToString());
                ht.Add("@FormType", FormName);
                DataTable dt = DBContext.GetDataSet("sp_Approved_NotApproved_ByRequestType", ht).Tables[0];
               
                r.PendingApproval = dt.Rows[0]["Approved"].ToString();
                r.Expiring = dt.Rows[0]["NotApproved"].ToString();
            }

            
            return Json(r);
        }
    }
    public class EmpDocument
    {
        public string ID { get; set; }
        public string DOCID { get; set; }
        public string DocType { get; set; }
        public string LeaveType { get; set; }
        public string Name { get; set; }

    }
    public class NotificationsViewModel
    {
        public NotificationsViewModel()
        {
            History = new List<ViewModels.LeaveApplicationViewModel>();
            Notification = new List<Notifications>();
            DocumentList = new List<EmpDocument>();
        }
        public List<EmpDocument> DocumentList { get; set; }
        public string ProfileImg { get; set; }
        public List<ViewModels.LeaveApplicationViewModel> History { get; set; }
        public List<Notifications> Notification { get; set; }
    }
    public class Notifications
    {
        public string Date { get; set; }
        public string ReqType { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string Ref { get; set; }
        public string App { get; set; }
        public string GraceDays { get; set; }
        public string DaysAfterRequest { get; set; }
    }
    public class Request
    {
        public RequestCount LeaveRequest { get; set; }
        public RequestCount Reimbursement { get; set; }
        public RequestCount Miscellaneous { get; set; }
        public RequestCount Late_and_Absence { get; set; }
        public RequestCount Allowance { get; set; }
    }
    public class RequestCount
    {
        public string PendingApproval { get; set; }
        public string Expiring { get; set; }
    }
}