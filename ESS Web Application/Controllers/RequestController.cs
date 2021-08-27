using ESS_Web_Application.Entity;
using ESS_Web_Application.Helper;
using ESS_Web_Application.Infrastructure;
using ESS_Web_Application.Services;
using ESS_Web_Application.ViewModels;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ESS_Web_Application.Controllers
{
    [AuthorizeActionFilter]
    public class RequestController : Controller
    {
        IRequestService _requestService = new RequestService();
        // GET: Request
        [HttpPost]
        public ActionResult SaveProfileImage(string FormName)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    HttpFileCollectionBase files = Request.Files;

                    Guid guid = Guid.NewGuid();
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        // Checking for Internet Explorer      
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var pic = System.Web.HttpContext.Current.Request.Files[fname];
                        HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                        UploadFiles.UploadFile(filebase, FormName, guid.ToString());
                    }
                    return Json(guid.ToString());
                }
                else
                {
                    return Json("No File Saved.");
                }
            }
            catch (Exception ex)
            {
                return Json("Error While Saving.");
            }
        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult DownloadReport(string PrintId)
        //{
        //    DownloadDetails data = new DownloadDetails();
        //    if (!String.IsNullOrEmpty(PrintId))
        //    {
        //        data = UploadFiles.DownloadFile(PrintId);
        //    }

        //    return File(Encoding.ASCII.GetBytes(data.Bytes == null ? "" : data.Bytes.ToString()), data.ContentType == null ? "abac" : data.ContentType, data.Name == null ? "" : data.Name);
        //}
        [HttpPost]
        [ValidateInput(false)]
        public FileResult DownloadReport(FormCollection form)
        {
            string PrintId = form["DocName"].ToString();
            DownloadDetails data = new DownloadDetails();
            if (!String.IsNullOrEmpty(PrintId))
            {
                data = UploadFiles.DownloadFile(PrintId);
            }

            return File(Encoding.ASCII.GetBytes(data.Bytes == null ? "" : data.Bytes.ToString()), data.ContentType == null ? "abac" : data.ContentType, data.Name == null ? "" : data.Name);
        }
        public JsonResult LoadUploadedFiles(string DocType, string DocNumber)
        {
            string str = DocType;
            string str1 = DocNumber;
            DataTable fileList = GetFileList(str, str1);
            List<AttachmentModel> attch = new List<AttachmentModel>();
            if (fileList != null && fileList.Rows.Count > 0)
            {
                //double num = Convert.ToDouble(fileList.Compute("SUM(Size)", ""));
                //if (num > 0)
                //{
                // this.lblTotalSize.Text = this.CalculateFileSize(num);
                //}
                foreach (DataRow item in fileList.Rows)
                {
                    AttachmentModel att = new AttachmentModel()
                    {
                        ID = item["ID"].ToString(),
                        Name = item["Name"].ToString(),
                        Remarks = item["Remarks"].ToString(),
                        DOCID = item["DOCID"].ToString(),
                    };
                    attch.Add(att);
                }
            }
            return Json(attch);
        }

        public static DataTable GetFileList(string DocType, string DocID)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandText = "SELECT ID,DOCID, Name, ContentType, Size , Remarks FROM TBL_UploadFiles WHERE  DOCID = @DOCID AND DocType= @DocType";
                sqlCommand.Parameters.Add("@DOCID", SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add("@DocType", SqlDbType.VarChar, 50);
                sqlCommand.Parameters["@DOCID"].Value = DocID;
                sqlCommand.Parameters["@DocType"].Value = DocType;
                sqlCommand.CommandType = CommandType.Text;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }
            return dataTable;
        }

        #region LeaveApplication
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LeaveApplicationForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "LeaveAppForm"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "LeaveAppForm", Session["UserCompanyID"].ToString());
                ViewBag.LeaveTypes = _requestService.GetLeaveTypes(Session["UserID"].ToString());
                ViewBag.ReplacementEmployee = _requestService.GetReplacementEmployee(Session["UserCompanyID"].ToString());
                ViewBag.Countries = _requestService.GetCountries();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetCities(string CountryId)
        {
            var data = _requestService.GetCities(CountryId);
            return Json(data);
        }
        public JsonResult GetEmployeeDetails(string SelectUserId)
        {
            var data = _requestService.GetEmployeeDeatils(SelectUserId);
            return Json(data);
        }
        public JsonResult GetEmployeeLeavesData(string EmployeeId, string LeaveTypeId, string StartDate, string EndDate, string LeaveType)
        {
            string Enddate = string.IsNullOrEmpty(EndDate) ? "" : DateTime.Parse(EndDate).Date.AddDays(-1).ToString();
            var data = _requestService.FillCalculatedFields(EmployeeId, LeaveTypeId, StartDate, Enddate, LeaveType);
            return Json(data);
        }
        public JsonResult SaveLeaveRequest(string AtchGuid, string EmployeeID, string LeaveType, string Remarks, string Airticket, string ReplacementId, string StartDate, string ReturnToWork, string LeaveTypeName, string Noofdays, string Leavebalance, string TravelTo, string TravelFrom, string DateofTravel, string DateofReturn, string Accomodation)
        {
            string Enddate = DateTime.Parse(ReturnToWork).Date.AddDays(-1).ToString();
            string data = _requestService.SaveLeaveRequest(AtchGuid, Session["UserCompanyID"].ToString(), EmployeeID, LeaveType, Remarks, Airticket, ReplacementId, StartDate, Enddate, ReturnToWork, LeaveTypeName, Session["UserID"].ToString(), Noofdays, Leavebalance, TravelTo, TravelFrom, DateofTravel, DateofReturn, Accomodation);
            return Json(data);
        }
        public JsonResult GetLeaveApplicationData(string EmpId)
        {
            var data = _requestService.GetLeaveApplicationData(EmpId, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
            if (bytes.Length > 1)
            {
                string Profileimg = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                data.ProfileImage = Profileimg;
            }
            return Json(data);
        }
        public JsonResult GetEmployeeLeavesEditData(string Id)
        {
            var data = _requestService.GetEmployeeLeavesEditData(Id);
            return Json(data);
        }
        public JsonResult SaveEditLeaveRequest(string Reqid, string EmployeeID, string LeaveType, string Remarks, string ReplacementId, string StartDate, string EndDate, string Noofdays, string Leavebalance)
        {
            string data = _requestService.SaveEditLeaveRequest(Reqid, EmployeeID, LeaveType, Remarks, ReplacementId, StartDate, EndDate, Session["UserID"].ToString(), Session["UserCompanyID"].ToString(), Noofdays, Leavebalance);
            return Json(data);
        }
        public ActionResult GetLeaveApproval(string Id)
        {
            var data = _requestService.GetLeaveApproval(Id);
            return Json(data);
        }
        public ActionResult SaveLeaveApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveLeaveApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public ActionResult GetLeaveDetails(string Id)
        {
            var data = _requestService.GetLeaveDetails(Id);
            return Json(data);
        }
        public ActionResult SaveResumption(string Id, string Date, string Remarks)
        {
            var result = _requestService.SaveResumptions(Id, Date, Remarks);
            return Json(result);
        }

        #endregion Employee Detail Update Request Form
        public ActionResult EmployeeDetailRequestForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "LeaveAppForm"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "LeaveAppForm", Session["UserCompanyID"].ToString());
                ViewBag.LeaveTypes = _requestService.GetLeaveTypes(Session["UserID"].ToString());
                ViewBag.ReplacementEmployee = _requestService.GetReplacementEmployee(Session["UserCompanyID"].ToString());
                ViewBag.Countries = _requestService.GetCountries();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult SaveEmployeeDetail(string AtchGuid, string EmployeeID, string ContactDetail, string LastName, string MatrialStatus, string EmployeeAddress)
        {
            string ErrorMsg = "";
            ErrorMsg = _requestService.SaveEmployeeDetail(AtchGuid, Session["UserCompanyID"].ToString(), Session["UserID"].ToString(), EmployeeID, ContactDetail, LastName, MatrialStatus, EmployeeAddress);

            return Json(ErrorMsg);
        }
        public JsonResult GetEmployeeDetailData()
        {
            var data = _requestService.GetEmployeeDetailData(Session["UserID"].ToString(),"", Session["UserCompanyID"].ToString());
            byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
            if (bytes.Length > 1)
            {
                string Profileimg = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                data.ProfileImage = Profileimg;
            }
            return Json(data);
        }
        public JsonResult GetEmployeeDetailEditData(string Id)
        {
            var data = _requestService.GetEmployeeDetailEditData(Id, Session["UserID"].ToString());
            return Json(data);
        }
        public JsonResult SaveEditEmployeeDetail(string Id, string EmployeeID, string ContactDetail, string LastName, string MatrialStatus, string EmployeeAddress)
        {
            string ErrorMsg = _requestService.SaveEditEmployeeDetail(2, Id, EmployeeID, ContactDetail, LastName, MatrialStatus, EmployeeAddress, Session["UserID"].ToString());
            return Json(ErrorMsg);
        }
        public JsonResult GetEmployeeDetailApproval(string Id)
        {
            var data = _requestService.GetEmployeeDetailApproval(Id);
            return Json(data);
        }
        public JsonResult SaveEmployeeDetailApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveEmployeeDetailApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public ActionResult GetEmployeeDetailDetails(string Id)
        {
            var data = _requestService.GetEmployeeDetailDetails(Id);
            return Json(data);
        }
        #region

        #endregion
        #region ProfileUploadImage
        [HttpPost]
        public JsonResult UploadFile()
        {
            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var ProfileImg = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (ProfileImg.ContentLength > 0)
                {
                    byte[] imageData = GetImageBytes(ProfileImg.InputStream);
                    var result = _requestService.SaveImageToDB(imageData, Session["UserID"].ToString());
                    byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
                    _imgname = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);

                }
            }
            return Json(_imgname);
        }
        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public byte[] GetImageBytes(Stream stream)
        {
            byte[] buffer;
            using (Bitmap image = ResizeImage(stream))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    ms.Position = 0;
                    buffer = new byte[ms.Length];
                    ms.Read(buffer, 0, (int)ms.Length);
                    return buffer;
                }
            }
        }
        public Bitmap ResizeImage(Stream stream)
        {
            System.Drawing.Image originalImage = Bitmap.FromStream(stream);
            int height = 500;
            int width = 500;
            double ratio = Math.Min(originalImage.Width, originalImage.Height) / (double)Math.Max(originalImage.Width, originalImage.Height);
            if (originalImage.Width > originalImage.Height)
            {
                height = Convert.ToInt32(height * ratio);
            }
            else
            {
                width = Convert.ToInt32(width * ratio);
            }
            Bitmap scaledImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(scaledImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, width, height);
                return scaledImage;
            }
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
        #endregion
        #region Reimbursement
        public ActionResult ReimbursementForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "ReimburseForm"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "ReimburseForm", Session["UserCompanyID"].ToString());

                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult GetDataBind()
        {
            ReimbursementDropdownBindingViewModel rvm = new ReimbursementDropdownBindingViewModel()
            {
                Countries = _requestService.GetCountries(),
                //Cities = _requestService.GetCities(""),
                ReimbursementType = _requestService.GetReimbursementType(),
                CurrencyCode = _requestService.GetCurrencyCode(),
                ActivityTypes = _requestService.GetActivityTypes()
            };
            return Json(rvm);
        }
        public ActionResult SaveReimbursement(string AtchGuid, string Type, List<ReimbursementSaveViewModel> RModle)
        {
            string ErrorMsg = validateOvertimeRecords(RModle);
            if (string.IsNullOrEmpty(ErrorMsg))
                ErrorMsg = _requestService.Reimbursement(AtchGuid, Type, RModle, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            else
            {
            }
            return Json(ErrorMsg);
        }
        private string validateOvertimeRecords(List<ReimbursementSaveViewModel> RModle)
        {
            string ErrorMessage = "";
            foreach (var dr in RModle)
            {
                if (string.IsNullOrEmpty(dr.ReimbursementType))
                {
                    ErrorMessage = "Reimbursement Type must be selected.";
                    break;
                }
                else if (string.IsNullOrEmpty(dr.From) || string.IsNullOrEmpty(dr.To))
                {
                    ErrorMessage = "One or more dates have not been specified. Please specify appropriate dates.";
                    break;
                }
                else if (DateTime.Parse(dr.From).Date > DateTime.Parse(dr.To).Date)
                {
                    ErrorMessage = "One or more dates found inappropriate. \"From Date\" should be less than the \"To Date\"";
                    break;
                }
                else if ((string.IsNullOrEmpty(dr.City)) &&
                    (string.IsNullOrEmpty(dr.ActivityType)) &&
                    (string.IsNullOrEmpty(dr.Reciept)) &&
                    (string.IsNullOrEmpty(dr.Currency)) &&
                    (string.IsNullOrEmpty(dr.Amount)))
                {
                    ErrorMessage = "One or more records found inappropriate as no overtime hours has been specified.";
                    break;
                }
                else
                {
                    ErrorMessage = "";
                }
            }
            return ErrorMessage;
        }
        public ActionResult GetReimbursementList()
        {
            var result = _requestService.GetReimbursementList(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
            if (bytes.Length > 1)
            {
                string Profileimg = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                result.ProfileImage = Profileimg;
            }
            return Json(result);
        }
        public ActionResult GetEditDataBind(string Reqid)
        {
            var data = _requestService.GetEditDatabind(Reqid, Session["UserID"].ToString());
            return Json(data);
        }
        public ActionResult SaveEditReimbursement(string Type, List<ReimbursementSaveViewModel> RModle, string Id, string Recall)
        {
            string ErrorMsg = validateOvertimeRecords(RModle);
            if (string.IsNullOrEmpty(ErrorMsg))
                ErrorMsg = _requestService.EditSaveReimbursement(Type, RModle, Session["UserID"].ToString(), Id, Recall == "true" ? true : false);
            else
            {
            }
            return Json(ErrorMsg);
        }
        public ActionResult GetDetails(string Reqid)
        {
            var data = _requestService.GetDetails(Reqid, Session["UserID"].ToString());           
            
            return Json(data);
        }
        public ActionResult GetApproval(string Id)
        {
            var data = _requestService.GetApproval(Id);
            var split = data.Split('_');
            return Json(new { Employee = split[0], History = split[1], CompanyName = split[2] });
        }
        public ActionResult SaveApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }

        #endregion

        #region AdvanceSalary
        public ActionResult AdvanceSalaryApplicationFrom()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "AdvSalaryForm"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "AdvSalaryForm", Session["UserCompanyID"].ToString());

                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetAdvanceSalaryData()
        {
            var data = _requestService.GetAdvanceSalaryData(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
            if (bytes.Length > 1)
            {
                string Profileimg = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                data.ProfileImage = Profileimg;
            }
            return Json(data);
        }
        public JsonResult SaveAdvanceSalary(string EmployeeID, string PayPeriod, string Remarks, string Amount)
        {
            string data = _requestService.SaveAdvanceSalary(Session["UserCompanyID"].ToString(), EmployeeID, Remarks, Session["UserID"].ToString(), PayPeriod, Amount);
            return Json(data);
        }
        public JsonResult GetAdvanceSalaryEditData(string Id)
        {
            var data = _requestService.GetAdvanceSalaryEditData(Id);
            return Json(data);
        }
        public JsonResult SaveEditAdvanceSalary(string Reqid, string EmployeeID, string Remarks, string Amount)
        {
            string data = _requestService.SaveEditAdvanceSalary(Reqid, EmployeeID, Remarks, Amount, Session["UserID"].ToString(), 2);
            return Json(data);
        }
        public ActionResult GetAdvanceSalaryApproval(string Id)
        {
            var data = _requestService.GetAdvanceSalaryApproval(Id);
            return Json(data);
        }
        public ActionResult SaveAdvanceSalaryApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveAdvanceSalaryApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }

        #endregion

        #region LoanApplication
        public ActionResult LoanApplicationForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "LoanAppForm"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "LoanAppForm", Session["UserCompanyID"].ToString());
                ViewBag.LoanTypes = _requestService.GetLoanTypes();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetInstallments(string LoanType)
        {
            var data = _requestService.GetInstallments(LoanType);
            return Json(data);
        }
        public JsonResult GetLoanData()
        {
            var data = _requestService.GetLoanData(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            byte[] bytes = GetImageFromDB(Session["UserID"].ToString());
            if (bytes.Length > 1)
            {
                string Profileimg = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                data.ProfileImage = Profileimg;
            }
            return Json(data);
        }
        public JsonResult SaveLoanApplication(string Emp, string Type, string Amount, string Installments, List<LoanSaveViewModel> LModle)
        {
            string ErrorMsg = validateOvertimeRecords(LModle);
            if (string.IsNullOrEmpty(ErrorMsg))
                ErrorMsg = _requestService.SaveLoanApplication(Emp, Type, Amount, Installments, LModle, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            else
            {
            }
            return Json(ErrorMsg);
        }
        private string validateOvertimeRecords(List<LoanSaveViewModel> RModle)
        {
            string ErrorMessage = "";

            foreach (var item in RModle)
            {
                if (hasDuplicateEmployeeRecords(RModle, Convert.ToDecimal(item.Amount)))
                {
                    ErrorMessage = "Header Total Amount must be equal to Lines Amount .";
                    break;
                }
                else if (string.IsNullOrEmpty(item.Amount))
                {
                    ErrorMessage = "Amount must be entered.";
                    break;
                }

                else
                {
                    ErrorMessage = "";
                }
            }

            return ErrorMessage;
        }
        private bool hasDuplicateEmployeeRecords(List<LoanSaveViewModel> RModle, Decimal TotalAmount)
        {
            bool hasDuplicate = true;
            decimal lineAmount = 0;
            foreach (var item in RModle)
            {
                lineAmount = lineAmount + Convert.ToDecimal(item.Amount);

            }

            if (lineAmount == TotalAmount)
            {
                hasDuplicate = false;
            }

            return hasDuplicate;
        }
        public JsonResult GetLoanEditData(string Id)
        {
            var data = _requestService.GetLoanEditDatabind(Id, Session["UserID"].ToString());
            return Json(data);
        }
        public JsonResult SaveEditLoan(string Id, string User, string Type, string Amount, string Installment, string Date, string Remarks)
        {
            string ErrorMsg = _requestService.EditSaveLoan(2, Id, User, Type, Amount, Installment, Date, Remarks, Session["UserID"].ToString());
            return Json(ErrorMsg);
        }
        public JsonResult GetLoanApproval(string Id)
        {
            var data = _requestService.GetLoanApproval(Id);
            return Json(data);
        }
        public JsonResult SaveLoanApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveLoanApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }

        #endregion

        #region TicketRequest
        public ActionResult TicketRequestForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "TicketReqForm"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "TicketReqForm", Session["UserCompanyID"].ToString());
                ViewBag.Type = _requestService.GetTicketTypes();
                ViewBag.Countries = _requestService.GetCountries();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult SaveTicket(string AtchGuid, string TicketBalance, string Emp, string Type, string Remarks, string FromCountry, string FromCity, string ToCountry, string ToCity, string TravelDate, string ReturnDate)
        {
            string ErrorMsg = "";

            ErrorMsg = _requestService.SaveTicket(AtchGuid, TicketBalance, Emp, Type, Remarks, FromCountry, FromCity, ToCountry, ToCity, TravelDate, ReturnDate, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());

            return Json(ErrorMsg);
        }
        public JsonResult GetTicketData()
        {
            var data = _requestService.GetTicketData(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public JsonResult GetTicketEditData(string Id)
        {
            var data = _requestService.GetTicketEditDatabind(Id, Session["UserID"].ToString());
            return Json(data);
        }
        public JsonResult SaveEditTicket(string Id, string Emp, string Type, string FromCity, string ToCity, string TravelDate, string ReturnDate, string TicketBalance, string Remarks)
        {
            string ErrorMsg = _requestService.EditSaveTicket(2, Id, Emp, Type, FromCity, ToCity, TravelDate, ReturnDate, TicketBalance, Remarks, Session["UserID"].ToString());
            return Json(ErrorMsg);
        }
        public JsonResult GetTicketApproval(string Id)
        {
            var data = _requestService.GetTicketApproval(Id);
            return Json(data);
        }
        public JsonResult SaveTicketApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveTicketApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public ActionResult GetTicketDetails(string Id)
        {
            var data = _requestService.GetTicketDetails(Id);
            return Json(data);
        }

        #endregion

        #region MiscellaneousApplication
        public ActionResult MiscellaneousApplicationForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "MiscellaneousForm"))
            {
                ViewBag.Type = _requestService.GetMiscellaneousTypes();
                ViewBag.Language = _requestService.GetPreferredLanguage();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult SaveMiscellaneous(string AtchGuid, string MiscellaneousType, string PreferredType, string Opening, string Salary, string Department, string Report, string Address, string Remarks)
        {
            string ErrorMsg = "";

            ErrorMsg = _requestService.SaveMiscellaneous(AtchGuid, MiscellaneousType, PreferredType, Opening, Salary, Department, Report, Address, Remarks, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());

            return Json(ErrorMsg);
        }
        public JsonResult GetMiscellaneousData()
        {
            var data = _requestService.GetMiscellaneousData(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public JsonResult GetMiscellaneousEditData(string Id)
        {
            var data = _requestService.GetMiscellaneousEditData(Id, Session["UserID"].ToString());
            return Json(data);
        }
        public JsonResult SaveEditMiscellaneous(string Id, string MiscellaneousType, string PreferredType, string Address, string Remarks)
        {
            string ErrorMsg = _requestService.SaveEditMiscellaneous(2, Id, MiscellaneousType, PreferredType, Address, Remarks, Session["UserID"].ToString());
            return Json(ErrorMsg);
        }
        public JsonResult GetMiscellaneousApproval(string Id)
        {
            var data = _requestService.GetMiscellaneousApproval(Id);
            return Json(data);
        }
        public JsonResult SaveMiscellaneousApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveMiscellaneousApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public ActionResult GetMiscellaneousDetails(string Id)
        {
            var data = _requestService.GetMiscellaneousDetails(Id);
            return Json(data);
        }


        #endregion

        #region LateandAbsenceJustification
        public ActionResult LateandAbsenceJustApplicationForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "Late And Absence Justifications Form"))
            {
                ViewBag.Employee = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "Late And Absence Justifications Form", Session["UserCompanyID"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetLateandAbsenceApplicationData(string EmpId)
        {
            var data = _requestService.GetLateandAbsenceJustificationData(EmpId, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public JsonResult SaveLateandLeaveRequest(string AtchGuid, string EmployeeID, string Category, string Date, string PunchIn, string PunchOut, string SubCategory, string Remarks)
        {
            string data = _requestService.SaveLateandLeaveRequest(AtchGuid, 2, Session["UserCompanyID"].ToString(), EmployeeID, Category, Date, PunchIn, PunchOut, SubCategory, Remarks, Session["UserID"].ToString());
            return Json(data);
        }
        public ActionResult GetLateandLeaveDetails(string Id)
        {
            var data = _requestService.GetLateandLeaveDetails(Id);
            return Json(data);
        }
        public JsonResult GetEmployeeLateandAbsenceEditData(string Id)
        {
            var data = _requestService.GetLateandAbsenceEditData(Id);
            return Json(data);
        }
        public JsonResult EditSaveLateandLeaveRequest(string Id, string EmployeeID, string Category, string Date, string PunchIn, string PunchOut, string SubCategory, string Remarks)
        {
            string data = _requestService.saveLateandAnsenceEdit(Id, 2, Category,
            Remarks, PunchIn, PunchOut, SubCategory, Date);
            return Json(data);
        }
        public ActionResult GetLateandAbsenceApproval(string Id)
        {
            var data = _requestService.GetLateandAbsenceApproval(Id);
            var split = data.Split('_');
            return Json(new { Employee = split[0], History = split[1], CompanyName = split[2] });
        }
        public ActionResult SaveLeaveandAbsenceApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveLeaveandAbsenceApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }

        #endregion

        #region AllownceRequest
        public ActionResult AllownceRequestForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "AllowanceRequest"))
            {
                ViewBag.AllownceTypes = _requestService.GetAllownceTypes();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult SaveAllowanceRequest(string AtchGuid, string Date, string Amount, string Type, string Remarks)
        {
            if (validateForm(Date))
            {
                string data = _requestService.SaveAllowanceRequest(AtchGuid, 2, Date, Amount, Type, Remarks, Session["UserCompanyID"].ToString(), Session["UserID"].ToString());
                return Json(data);
            }
            else
            {
                string data = "Sorry! some error has occurred. Please select the 1st Day of the month .";
                return Json(data);
            }

        }
        public bool validateForm(string Date)
        {

            if (Convert.ToDateTime(Date).Day == 1)
            {

                return true;
            }
            else
            {
                return false;
            }

        }
        public JsonResult GetAllowanceRequestData()
        {
            var data = _requestService.GetAllowanceRequestData(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public JsonResult GetAllowanceRequestEditData(string Id)
        {
            var data = _requestService.GetAllowanceRequestEditData(Id);
            return Json(data);
        }
        public JsonResult EditSaveAllowanceRequest(string Id, string Amount, string Type, string Remarks, string Date)
        {
            string data = _requestService.saveAllowanceRequestEdit(Id, 2, Amount,
            Type, Remarks, Date);
            return Json(data);
        }
        public ActionResult GetAllowanceRequestApproval(string Id)
        {
            var data = _requestService.GetAllowanceRequestApproval(Id);
            return Json(data);
        }
        public ActionResult SaveAllowanceRequestApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveAllowanceRequestApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }

        #endregion

        #region AssetManagementRequest
        public ActionResult AssetManagementReqForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "AssetManagement"))
            {
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "AssetManagement", Session["UserCompanyID"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult GetAssetDataBind()
        {
            AssetDropdownBindingViewModel rvm = new AssetDropdownBindingViewModel()
            {
                Type = _requestService.GetAssetType(),
            };
            return Json(rvm);
        }
        public JsonResult GetAssetData()
        {
            var data = _requestService.GetAssetData(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public JsonResult GetAssetDetails(string Id)
        {
            var data = _requestService.GetAssetDetails(Id);
            return Json(data);
        }
        public JsonResult SaveAssetRequest(string EmpFor, string RequestDate, List<AssetViewModel> List, string Remarks)
        {
            string data = _requestService.SaveAssetRequest(EmpFor, RequestDate, List, Remarks, Session["UserID"].ToString(), Session["UserCompanyID"].ToString());
            return Json(data);
        }
        public JsonResult GetAssetRequestEditData(string Id)
        {
            var data = _requestService.GetAssetEditDatabind(Id);
            return Json(data);
        }
        public JsonResult EditSaveAssetRequest(string Id, string EmpFor, string RequestDate, List<AssetViewModel> List, string Remarks)
        {
            string data = _requestService.SaveEditAsset(2, Id, EmpFor, RequestDate, List, Remarks, Session["UserID"].ToString());
            return Json(data);
        }
        public ActionResult GetAssetRequestApproval(string Id)
        {
            var data = _requestService.GetAssetApproval(Id);
            return Json(data);
        }
        public ActionResult SaveAssetApprove_Reject(string Type, string Id, string StatusId, string Remarks)
        {
            var data = _requestService.SaveAssetApprove_Reject(Type, Id, StatusId, Session["UserID"].ToString(), Remarks, Session["UserCompanyID"].ToString());
            return Json(data);
        }


        #endregion

        #region EmployeeAttendanceDeatils        
        //public JsonResult GetAttendance(string StartDate, string EndDate, string Emp)
        //{
        //    var data = _requestService.GetEmployeeAttendance(StartDate, EndDate, Emp, Session["UserCompanyID"].ToString(), Session["UserID"].ToString());
        //    return Json(data);
        //}
        #endregion
        #region StaffExpenseRequest
        public ActionResult StaffExpenseForm()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "Staff Expense Form"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult GetStaffExpenseDataBind()
        {
            var a = _requestService.GetStaffExpenseList(Session["UserID"].ToString(), Session["UserCompanyID"].ToString());

            return Json("");
        }
        #endregion
        public JsonResult GetUserByDepartment(string DepId)
        {
            var data = _requestService.GetLeaveAppUsersbyDepId(Session["UserID"].ToString(), DepId, Session["UserCompanyID"].ToString());
            return Json(data);
        }
        #region Rdlc Report
        public ActionResult EmployeeAttendanceDetail()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "EmployeeAttendanceReport"))
            {
                ViewBag.Type = _requestService.GetDepartmentTypes(Session["UserID"].ToString());
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "EmployeeAttendanceReport", Session["UserCompanyID"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult EmployeeAttendanceReport(FormCollection form)
        {
            ESS_Web_Application.Report.DataSet1 ds = new ESS_Web_Application.Report.DataSet1();
            string StartDate = form["StartDate"].ToString();
            string EndDate = form["EndDate"].ToString();
            string Emp = form["EmpId"].ToString();
            string Depid = form["DepId"].ToString();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            var data = _requestService.GetEmployeeAttendance(StartDate, EndDate, Depid, Emp, Session["UserCompanyID"].ToString(), Session["UserID"].ToString());
            var dt = new DataTable();
            dt = ToDataTable(data);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report\EmpAttendanceReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));



            ViewBag.ReportViewer = reportViewer;

            return View();
        }
        public ActionResult EmployeeDetail()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "EmployeeAttendanceReport"))
            {
                ViewBag.Type = _requestService.GetDepartmentTypes(Session["UserID"].ToString());
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "EmployeeAttendanceReport", Session["UserCompanyID"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }       
        public ActionResult EmployeeDetailReport(FormCollection form)
        {
            ESS_Web_Application.Report.EmpDetailDataSet empds = new ESS_Web_Application.Report.EmpDetailDataSet();
            string Emp = form["EmpId"].ToString();
            string Depid = form["DepId"].ToString();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            var data = _requestService.GetEmployeeDetailData(Session["UserID"].ToString(), Depid, Session["UserCompanyID"].ToString());
            var test = data.History.ToList();
            var dt = new DataTable();
            dt = ToDataTable(test);

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report\EmpDetailReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));



            ViewBag.ReportViewer = reportViewer;

            return View();
        }
        public ActionResult ReimbursementDetail()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "EmployeeAttendanceReport"))
            {
                ViewBag.Type = _requestService.GetDepartmentTypes(Session["UserID"].ToString());
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "EmployeeAttendanceReport", Session["UserCompanyID"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult ReimbursementDetailReport(FormCollection form)
        {
            ESS_Web_Application.Report.ReimbursementDataSet Reimds = new ESS_Web_Application.Report.ReimbursementDataSet();
            string EmpId = form["EmpId"].ToString();
            string Depid = form["DepId"].ToString();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            var data = _requestService.GetDetailReport(EmpId, Depid, Session["UserID"].ToString());          
            var dt = new DataTable();
            dt = ToDataTable(data);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report\ReimbursementReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        public ActionResult LeaveDetail()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "EmployeeAttendanceReport"))
            {
                ViewBag.Type = _requestService.GetDepartmentTypes(Session["UserID"].ToString());
                ViewBag.User = _requestService.GetLeaveAppUsers(Session["UserID"].ToString(), "EmployeeAttendanceReport", Session["UserCompanyID"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public ActionResult LeaveDetailReport(FormCollection form)
        {
            ESS_Web_Application.Report.LeaveDataSet Reimds = new ESS_Web_Application.Report.LeaveDataSet();
            string EmpId = form["EmpId"].ToString();
            string Depid = form["DepId"].ToString();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);
            var data = _requestService.GetLeaveDetailReport(EmpId, Session["UserID"].ToString());
            var dt = new DataTable();
            dt = ToDataTable(data);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Report\LeaveDetailReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            ViewBag.ReportViewer = reportViewer;
            return View();
        }
        #endregion


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

    public class AttachmentModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string DOCID { get; set; }
    }

}