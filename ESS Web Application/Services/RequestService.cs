using ESS_Web_Application.Entity;
using ESS_Web_Application.Helper;
using ESS_Web_Application.Repository;
using ESS_Web_Application.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public class RequestService : IRequestService
    {
        IRequestRepository _requestRepo = new RequestRepository();

        public int GetFormTypeID(string FormName)
        {
            string qry = "select FormTypeID from tbl_UserAction where SystemKeyword='" + FormName + "'";
            int Id = _requestRepo.GetFromTypeID(qry);
            return Id;
        }
        public int GenerateID(string CompanyId, string TableName)
        {
            var LastIdquery = "select max(RequestID) from " + TableName + " where CompanyId='" + CompanyId + "'";
            int id = 0;
            var a = DBContext.ExecuteReaderWithCommand(LastIdquery);
            while (a.Read())
            {
                id = int.Parse(string.IsNullOrEmpty(a[0].ToString()) ? "0" : a[0].ToString());
            }
            id = id == 0 ? 1001 : id + 1;
            return id;
        }
        public int SaveImageToDB(byte[] ImageByteArray, string UserID)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserId", UserID);
                ht.Add("@EmpImage", ImageByteArray);
                int ReqID = _requestRepo.InsertUpdateEmployeeImage(ht);
                return ReqID;
            }
            catch (Exception ex)
            {
                throw new Exception(" sp_InsertUpdateEmployeeImage :::::" + ex.Message);
            }
        }
        public List<DropDownBindViewModel> GetDepartmentTypes(string UserID)
        {
        
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", int.Parse(UserID));
            //ht.Add("@CompanyId", CompanyId);
            var dt = _requestRepo.GetDepartmentTypes(ht);

            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["DEPRTMNT"].ToString(),
                    Value = item["DEPRTMNT"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        #region LeaveApplication
        public List<DropDownBindViewModel> GetLeaveAppUsers(string UserID, string FormTypeID, string CompanyId)
        {
            int FormId = GetFormTypeID(FormTypeID);
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", int.Parse(UserID));
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);
            var dt = _requestRepo.GetLeaveAppUsers(ht);
            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["EmployeeFullNameWithID"].ToString(),
                    Value = item["UserID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public List<DropDownBindViewModel> GetLeaveAppUsersbyDepId(string UserID, string DepId, string CompanyId)
        {

            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", int.Parse(UserID));
            ht.Add("@DepID", DepId);
            ht.Add("@CompanyId", CompanyId);
            var dt = _requestRepo.GetLeaveAppUsersByDepId(ht);
            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["EmployeeFullNameWithID"].ToString(),
                    Value = item["UserID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }

        
        public EmployeeDeatilViewModel GetEmployeeDeatils(string UserID)
        {
            Hashtable ht1 = new Hashtable();
            ht1.Add("@userid", int.Parse(UserID));
            DataTable dt = _requestRepo.GetEmployeeDeatils(ht1);
            EmployeeDeatilViewModel emplouee = new EmployeeDeatilViewModel();
            if ((null != dt) && dt.Rows.Count > 0)
            {
                emplouee = new EmployeeDeatilViewModel()
                {
                    Name = (string)dt.Rows[0]["UserFullName"],
                    Designation = (string)dt.Rows[0]["JobTitle"],
                    EmpID = (string)dt.Rows[0]["EmployeeID"],
                    Nationality = (string)dt.Rows[0]["Nationality"],
                    Department = (string)dt.Rows[0]["Department"],
                    Address = (string)dt.Rows[0]["Address1"] + " " + (string)dt.Rows[0]["Address2"] + " " + (string)dt.Rows[0]["Address3"],
                    Phone = (string)dt.Rows[0]["Phone1"],
                    Email = (string)dt.Rows[0]["Email"]
                };
                //try
                //{
                //    imgEmployee.ImageUrl = "~/User/EmpImage.ashx?imgid=" + Session["_UserID"].ToString();
                //    //ConfigurationManager.AppSettings["Picture_Path"] + "/" + ((string)dt.Rows[0]["EmployeeID"]).Trim() + ConfigurationManager.AppSettings["Picture_Extension"];
                //   // imgEmployee.ToolTip = (string)dt.Rows[0]["UserFullName"];
                //}
                //catch (Exception ex)
                //{
                //    imgEmployee.ImageUrl = ConfigurationManager.AppSettings["Picture_Path"] + "/noImageAvailable.jpg";
                //    imgEmployee.ToolTip = "Image not available.";
                //}

            }
            return emplouee;

        }

        public List<DropDownBindViewModel> GetLeaveTypes(string UserID)
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetLeaveTypes(UserID);
            while (dt.Read())
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = dt["LeaveType"].ToString(),
                    Value = dt["ID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }

        public List<DropDownBindViewModel> GetCountries()
        {
            List<DropDownBindViewModel> Countries = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetCountries();
            foreach (DataRow item in dt.Rows)

            {
                DropDownBindViewModel Country = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["Name"].ToString(),
                };
                Countries.Add(Country);
            }
            return Countries;
        }
        public List<DropDownBindViewModel> GetCities(string CountryId)
        {
            List<DropDownBindViewModel> Cities = new List<DropDownBindViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@reginId", CountryId);
            var dt = _requestRepo.GetCities(ht);
            foreach (DataRow item in dt.Rows)

            {
                DropDownBindViewModel City = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["Name"].ToString(),
                };
                Cities.Add(City);
            }
            return Cities;
        }
        public List<DropDownBindViewModel> GetReplacementEmployee(string CompanyId)
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@CompanyId", CompanyId);
            var dt = _requestRepo.GetReplacementEmployee(ht);
            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["FullName"].ToString(),
                    Value = item["EmployeeID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public LeavesModel FillCalculatedFields(string EmployeeId, string LeaveTypeId, string StartDate,
            string EndDate, string LeaveType)
        {
            bool SuccessStatus = false;
            bool postCheck = false;
            string ErrorMsg = "";
            LeavesModel leaves = new LeavesModel();

            if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(LeaveTypeId) && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && LeaveType == "Annual Leave")
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", EmployeeId);
                ht.Add("@type", "Annual Leave");

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeId);


                Int64 workerID = _requestRepo.GetWorkerIDByUserID(ht1);
                if (LeaveType == "Annual Leave")
                {
                    leaves.TotalLeaves = _requestRepo.GetUserApprovedLeaves(ht);
                }
                leaves.NoOfDays = (DateTime.Parse(EndDate).Date.Subtract(DateTime.Parse(StartDate).Date).Days + 1).ToString();
                if (string.IsNullOrEmpty(leaves.TotalLeaves))
                {
                    leaves.TotalLeaves = "0";
                }
                // Resources d365 = ODATAConnection();
                //if (d365 != null)
                //{
                string company = ConfigurationManager.AppSettings["Company"];
                // var empBalances = d365.LeaveBalances.Where(x => x.DataAreaId == company && x.LeaveType == rCmbLeaveType1.SelectedItem.Text && x.HcmWorker == workerID && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                // IQueryable<HcmLeaveType> empLeaetype = GetLeaveBalance(d365, rCmbLeaveType1.SelectedItem.Text);


                decimal leavbealance = Convert.ToDecimal(DBContext.ExecuteScalar("sp_User_GetUserLeaveBalance", ht));


                leaves.Balancebefore = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves));
                leaves.Balanceafter = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves) - Convert.ToDecimal(leaves.NoOfDays));

                // recordChk = true;

            }
            else if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(LeaveTypeId) && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && LeaveType == "Half Day Leave")
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", EmployeeId);
                ht.Add("@type", "Half Day Leave");

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeId);


                Int64 workerID = _requestRepo.GetWorkerIDByUserID(ht1);
                if (LeaveType == "Half Day Leave")
                {
                    leaves.TotalLeaves = _requestRepo.GetUserApprovedLeaves(ht);
                }
                leaves.NoOfDays = (DateTime.Parse(EndDate).Date.Subtract(DateTime.Parse(StartDate).Date).Days + 1).ToString();
                //Resources d365 = ODATAConnection();
                //if (d365 != null)
                //{
                //    string company = ConfigurationManager.AppSettings["Company"];
                //    // var empBalances = d365.LeaveBalances.Where(x => x.DataAreaId == company && x.LeaveType == rCmbLeaveType1.SelectedItem.Text && x.HcmWorker == workerID && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
                //    IQueryable<HcmLeaveType> empLeaetype = GetLeaveBalance(d365, "Annual Leave");
                //    if (empLeaetype != null)
                //    {


                //        if (lblTotalLeaesTaken.Text == "")
                //        {
                //            lblTotalLeaesTaken.Text = "0";
                //        }
                if (string.IsNullOrEmpty(leaves.TotalLeaves))
                {
                    leaves.TotalLeaves = "0";
                }
                decimal leavbealance = Convert.ToDecimal(DBContext.ExecuteScalar("sp_User_GetUserLeaveBalance", ht));


                leaves.Balancebefore = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves));
                leaves.Balanceafter = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves) - Convert.ToDecimal(leaves.NoOfDays));

                //        if (Convert.ToDecimal(lblTotalLeaesTaken.Text) >= 30)
                //        {
                //            rBtnSave.Enabled = false;
                //            rBtnSaveAndSubmit.Enabled = false;
                //            SuccessStatus = false;
                //        }
                //        else
                //        {

                //            rBtnSave.Enabled = true;
                //            rBtnSaveAndSubmit.Enabled = true;

                //            SuccessStatus = true;
                //        }
                //        // recordChk = true;


                //    }
                //    else
                //    {
                //        rBtnSave.Enabled = false;
                //        rBtnSaveAndSubmit.Enabled = false;

                //        // lblErrorMsg.Text = "Error: " + "Integration Error ";

                //        lblNoOfDays.Text = "0";
                //        //lblCalendarDays.Text = "0";
                //        lblRejoiningDate.Text = "--";

                //        SuccessStatus = false;
                //    }


                //}

            }
            else if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(LeaveTypeId) && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && LeaveType == "Lieu Days")
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", EmployeeId);
                ht.Add("@type", "Lieu Days");

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeId);

                Int64 workerID = _requestRepo.GetWorkerIDByUserID(ht1);

                leaves.TotalLeaves = _requestRepo.GetUserApprovedLeaves(ht);

                leaves.NoOfDays = (DateTime.Parse(EndDate).Date.Subtract(DateTime.Parse(StartDate).Date).Days + 1).ToString();

                string company = ConfigurationManager.AppSettings["Company"];


                decimal leavbealance = _requestRepo.GetUserLeaveBalance(ht);

                if (string.IsNullOrEmpty(leaves.TotalLeaves))
                {
                    leaves.TotalLeaves = "0";
                }

                string lblLeaveBalance = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves));
                leaves.Balanceafter = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves) - Convert.ToDecimal(leaves.NoOfDays));

                //if (Convert.ToDecimal(leaves.Balanceafter) < 0)
                //{
                //    rBtnSave.Enabled = false;
                //    rBtnSaveAndSubmit.Enabled = false;
                //    SuccessStatus = false;
                //}
                //else
                //{

                //    rBtnSave.Enabled = true;
                //    rBtnSaveAndSubmit.Enabled = true;

                //    SuccessStatus = true;
                //}





            }
            else if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(LeaveTypeId) && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && LeaveType == "Marriage Leave")
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", EmployeeId);
                ht.Add("@type", "Marriage Leave");

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeId);

                Int64 workerID = _requestRepo.GetWorkerIDByUserID(ht1);

                leaves.TotalLeaves = _requestRepo.GetUserApprovedLeaves(ht);

                leaves.NoOfDays = (DateTime.Parse(EndDate).Date.Subtract(DateTime.Parse(StartDate).Date).Days + 1).ToString();

                string company = ConfigurationManager.AppSettings["Company"];


                decimal leavbealance = _requestRepo.GetUserLeaveBalance(ht);

                if (string.IsNullOrEmpty(leaves.TotalLeaves))
                {
                    leaves.TotalLeaves = "0";
                }

                if (Convert.ToInt32(leaves.NoOfDays) <= 5 && Convert.ToInt32(leaves.TotalLeaves) <= 5)
                {
                    string lblLeaveBalance = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves));
                    leaves.Balanceafter = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves) - Convert.ToDecimal(leaves.NoOfDays));

                    //rBtnSave.Enabled = true;
                    //rBtnSaveAndSubmit.Enabled = true;

                    SuccessStatus = true;
                }
                else
                {
                    //lblErrorMsg.Text = "Error: You cannot exceed the maximum marriage leaves of 5 days.";

                    //rBtnSave.Enabled = false;
                    //rBtnSaveAndSubmit.Enabled = false;
                    SuccessStatus = false;
                }




                // }
            }
            else if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(LeaveTypeId) && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && LeaveType == "Haj Leave")
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", EmployeeId);
                ht.Add("@type", "Haj Leave");

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeId);

                Int64 workerID = _requestRepo.GetWorkerIDByUserID(ht1);

                leaves.TotalLeaves = _requestRepo.GetUserApprovedLeaves(ht);

                leaves.NoOfDays = (DateTime.Parse(EndDate).Date.Subtract(DateTime.Parse(StartDate).Date).Days + 1).ToString();

                string company = ConfigurationManager.AppSettings["Company"];


                decimal leavbealance = _requestRepo.GetUserLeaveBalance(ht);

                if (string.IsNullOrEmpty(leaves.TotalLeaves))
                {
                    leaves.TotalLeaves = "0";
                }

                if (Convert.ToInt32(leaves.NoOfDays) <= 10 && Convert.ToInt32(leaves.TotalLeaves) <= 10)
                {
                    string lblLeaveBalance = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves));
                    leaves.Balanceafter = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves) - Convert.ToDecimal(leaves.NoOfDays));

                    //rBtnSave.Enabled = true;
                    //rBtnSaveAndSubmit.Enabled = true;

                    SuccessStatus = true;
                }
                else
                {
                    //lblErrorMsg.Text = "Error: You cannot exceed the maximum Haj leaves of 10 days.";

                    //rBtnSave.Enabled = false;
                    //rBtnSaveAndSubmit.Enabled = false;
                    SuccessStatus = false;
                }




                // }
            }
            else if (!string.IsNullOrEmpty(EmployeeId) && !string.IsNullOrEmpty(LeaveTypeId) && !string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && LeaveType == "Maternity Leave")
            {
                Hashtable ht = new Hashtable();
                ht.Add("@UserID", EmployeeId);
                ht.Add("@type", "Maternity Leave");

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeId);

                Int64 workerID = _requestRepo.GetWorkerIDByUserID(ht1);

                leaves.TotalLeaves = _requestRepo.GetUserApprovedLeaves(ht);

                leaves.NoOfDays = (DateTime.Parse(EndDate).Date.Subtract(DateTime.Parse(StartDate).Date).Days + 1).ToString();

                string company = ConfigurationManager.AppSettings["Company"];


                decimal leavbealance = _requestRepo.GetUserLeaveBalance(ht);

                if (string.IsNullOrEmpty(leaves.TotalLeaves))
                {
                    leaves.TotalLeaves = "0";

                    if (Convert.ToInt32(leaves.NoOfDays) <= 3)
                    {
                        string lblLeaveBalance = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves));
                        leaves.Balanceafter = Convert.ToString(leavbealance - Convert.ToDecimal(leaves.TotalLeaves) - Convert.ToDecimal(leaves.NoOfDays));

                        //rBtnSave.Enabled = true;
                        //rBtnSaveAndSubmit.Enabled = true;

                        SuccessStatus = true;
                    }
                    else
                    {
                        //lblErrorMsg.Text = "Error: You cannot exceed the maximum Maternity Leaves of 3 days.";

                        //rBtnSave.Enabled = false;
                        //rBtnSaveAndSubmit.Enabled = false;
                        SuccessStatus = false;
                    }
                }
                else
                {
                    //lblErrorMsg.Text = "Error: You can only apply for Maternity Leaves once in a year.";

                    //rBtnSave.Enabled = false;
                    //rBtnSaveAndSubmit.Enabled = false;
                    SuccessStatus = false;
                }
                //}
            }

            return leaves;
        }

        #region Employee Detail Update Request Form
        public EmployeeDetailRequestListViewModel GetEmployeeDetailData(string UserID,string Depid, string CompanyId)
        {
            EmployeeDetailRequestListViewModel list = new EmployeeDetailRequestListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("EmployeeDetailForm");
            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);
            DataTable dt = _requestRepo.Get_All_EmployeeDetail_Requests(ht);
            List<EmployeeDetailRequestViewModel> History = new List<EmployeeDetailRequestViewModel>();
            foreach (DataRow item in dt.Rows)
            {
                EmployeeDetailRequestViewModel single = new EmployeeDetailRequestViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    ReqStatus = item["RequestStatus"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    DEPRTMNT = item["DEPRTMNT"].ToString(),
                    DESIGNATION = item["DESIGNATION"].ToString(),
                    ContactDetail = item["ContactDetail"].ToString(),
                    LastName = item["LastName"].ToString(),
                    EmployeeAddress = item["EmployeeAddress"].ToString(),
                    MatrialStatus = item["MatrialStatus"].ToString(),
                    RequestId = item["Id"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    AttachmentGuid = item["AtchGuid"].ToString(),

                };
                History.Add(single);
                list.History = History;
            }
            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            List<EmployeeDetailRequestViewModel> Approval = new List<EmployeeDetailRequestViewModel>();
            DataTable dt2 = _requestRepo.Get_EmployeeDetail_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                EmployeeDetailRequestViewModel single2 = new EmployeeDetailRequestViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    ContactDetail = item2["ContactDetail"].ToString(),
                    LastName = item2["LastName"].ToString(),
                    EmployeeAddress = item2["EmployeeAddress"].ToString(),
                    MatrialStatus = item2["MatrialStatus"].ToString(),
                    ReqStatus = item2["RequestStatusID"].ToString(),
                    RequestId = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString(),
                    AttachmentGuid = item2["AtchGuid"].ToString()

                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public string SaveEmployeeDetail(string AtchGuid, string CompanyId, string UserId, string EmployeeID, string ContactDetail, string LastName, string MatrialStatus, string EmployeeAddress)
        {
            string lblStatus = "";
            int saveStatus = 2;
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("EmployeeDetailForm");
                Guid guid = Guid.NewGuid();
                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", UserId);
                ht.Add("@CompanyId", CompanyId);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Employee_Request"));
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@AtchGuid", AtchGuid);
                ht.Add("@ContactDetail", ContactDetail.Trim());
                ht.Add("@LastName", LastName.Trim());
                ht.Add("@MatrialStatus", MatrialStatus);
                ht.Add("@EmployeeAddress", EmployeeAddress.Trim());
                ht.Add("@EmployeeDetailRequestID", 0);
                ht.Add("@RequestKey", guid);
                int ReqID = _requestRepo.Insert_EmployeeDetail_Request(ht);

                if (ReqID > 0)
                {
                    lblStatus = "Your request has been saved successfully.";
                    if (saveStatus == 2)
                    {
                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@EmployeeDetailRequestID", ReqID);

                        //#region Send Email to Approver
                        DataTable dt = DBContext.GetDataSet("[sp_User_Get_EmployeeDetail_Request_Info_4_Email]", ht).Tables[0];

                        if ((null != dt) && dt.Rows.Count > 0 && dt.Rows[0]["Email"] != null)
                        {
                            string sBody = "";
                            string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                            htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                            sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                            sBody = sBody.Replace("<%Type%>", "BankLetterForm");
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                            sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                            sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                            string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                            Uri uri = HttpContext.Current.Request.Url;
                            var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
                             "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                            "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                            "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=EmployeeDetailForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%ApproveLink%>", url);
                            var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
                            "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                             "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                             "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=EmployeeDetailForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%RejectLink%>", url2);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                        }
                        //#endregion
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public EmployeeDetailRequestViewModel GetEmployeeDetailEditData(string Reqid, string UserId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@EmployeeDetailRequestID", Reqid);

            var dt = _requestRepo.Get_EmployeeDetail_Request_ByID(ht);

            EmployeeDetailRequestViewModel single = new EmployeeDetailRequestViewModel();
            foreach (DataRow item in dt.Rows)
            {
                single = new EmployeeDetailRequestViewModel()
                {
                    ContactDetail = item["ContactDetail"].ToString(),
                    LastName = item["LastName"].ToString(),
                    MatrialStatus = item["MatrialStatus"].ToString(),
                    EmployeeAddress = item["EmployeeAddress"].ToString(),
                };
            }
            return single;
        }
        public string SaveEditEmployeeDetail(int saveStatus, string Id, string EmployeeID, string ContactDetail, string LastName, string MatrialStatus, string EmployeeAddress, string UserID)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@EmployeeDetailRequestID", Id);
                ht.Add("@ContactDetail", ContactDetail.Trim());
                ht.Add("@LastName", LastName.Trim());
                ht.Add("@MatrialStatus", MatrialStatus);
                ht.Add("@EmployeeAddress", EmployeeAddress.Trim());
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@DBMessage", "");
                Guid guid = Guid.NewGuid();
                ht.Add("@RequestKey", "");

                string DBMessage = _requestRepo.Update_EmployeeDetail_Request(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@EmployeeDetailRequestID", Id);
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public EmployeeDetailRequestViewModel GetEmployeeDetailApproval(string Id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@EmployeeDetailRequestID", Id);
            DataTable dt = _requestRepo.Get_EmployeeDetail_Request_ByID(ht);
            EmployeeDetailRequestViewModel single = new EmployeeDetailRequestViewModel();
            if (dt != null)
            {
                single.Employee = (string)dt.Rows[0]["UserFullName"];
                single.ContactDetail = (string)dt.Rows[0]["ContactDetail"];
                single.EmployeeAddress = (string)dt.Rows[0]["EmployeeAddress"];
                single.LastName = (string)dt.Rows[0]["LastName"];
                single.MatrialStatus = (string)dt.Rows[0]["MatrialStatus"];
                single.CompanyName = (string)dt.Rows[0]["Name"];

                dt = null;
                dt = _requestRepo.Get_EmployeeDetail_Request_Remarks_List(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        single.History += "<div><strong><span>"
                                                    + dr["UserFullName"].ToString().Trim() + "</span>"
                                                    + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                    + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return single;
        }
        public string SaveEmployeeDetailApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    //Hashtable htcheck = new Hashtable();
                    //htcheck.Add("@RequestID", Id);
                    //htcheck.Add("@StatusID", StatusId);
                    //int finalApprovercheck = _requestRepo.Get_BankLetter_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@EmployeeDetailRequestID", Id);
                    ht.Add("@EmployeeDetailStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);

                    string DBMessage = _requestRepo.Approve_EmployeeDetail_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", Convert.ToInt32(UserId));
                    DataTable dt = _requestRepo.Get_EmployeeDetail_RequestSubmitter_Info_4_Email(ht);
                    var dt1 = DBContext.ExecuteReaderWithCommand("select MainApproverUserID, WorkflowID from dbo.Get_Next_Approver_Of_EmployeeDetail_Request(" + Id + ")");
                    int MainApproverUserID = 0;
                    while (dt1.Read())
                    {
                        MainApproverUserID = int.Parse(dt1["MainApproverUserID"].ToString());
                        //MainApproverUserID = int.Parse(dt1[0].ToString());
                    }
                    if ((null != dt) && dt.Rows.Count > 0 && MainApproverUserID != 0)
                    {

                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                        sBody = sBody.Replace("<%Type%>", "BankLetterForm");
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=EmployeeDetailForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=EmployeeDetailForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();
                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        ErrorMsg = "";
                        ErrorMsg = sBody;
                        // clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    else if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", "BankLetterForm");
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=EmployeeDetailForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=EmployeeDetailForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);

                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@EmployeeDetailRequestID", Id);
                    ht.Add("@EmployeeDetailStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@@RequestKey", "");

                    string DBMessage = _requestRepo.Reject_EmployeeDetail_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_EmployeeDetail_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;

                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Employee Detail Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }
        public List<EmployeeDetailRequestViewModel> GetEmployeeDetailDetails(string Id)
        {
            List<EmployeeDetailRequestViewModel> lvmm = new List<EmployeeDetailRequestViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@EmployeeDetailRequestID", Id);
            DataTable dt = _requestRepo.Get_EmployeeDetail_Request_Statuses(ht);

            foreach (DataRow item in dt.Rows)
            {
                EmployeeDetailRequestViewModel lvm = new EmployeeDetailRequestViewModel();
                lvm.RequestDate = item["ProcessesDate"].ToString();
                lvm.ReqStatus = item["RequestStatus"].ToString();
                lvm.Approver = item["MainApproverFullName"].ToString();
                lvm.ApprovedBy = item["ApprovedByFullName"].ToString();
                lvm.Description = item["RequestStatusDesc"].ToString();
                lvmm.Add(lvm);
            }
            return lvmm;
        }
        #endregion
        //public Resources ODATAConnection()

        //{
        //    try
        //    {
        //        string ODataEntityPath = ClientConfiguration.Default.UriString + "data";
        //        Uri oDataUri = new Uri(ODataEntityPath, UriKind.Absolute);
        //        var context = new Resources(oDataUri);

        //        context.SendingRequest2 += new EventHandler<SendingRequest2EventArgs>(delegate (object sender, SendingRequest2EventArgs e)
        //        {
        //            var authenticationHeader = OAuthHelper.GetAuthenticationHeader(useWebAppAuthentication: true);
        //            e.RequestMessage.SetHeader(OAuthHelper.OAuthHeader, authenticationHeader);
        //        });

        //        context.BuildingRequest += (sender, e) =>
        //        {
        //            var uriBuilder = new UriBuilder(e.RequestUri);
        //            // Requires a reference to System.Web
        //            var paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);
        //            paramValues.Add("cross-company", "true");
        //            uriBuilder.Query = paramValues.ToString();
        //            e.RequestUri = uriBuilder.Uri;
        //        };

        //        return context;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }


        //}
        public string SaveLeaveRequest(string AtchGuid, string CompanyId, string EmployeeID, string LeaveType, string Remarks, string Airticket, string ReplacementId, string StartDate, string EndDate, string ResumetoWork, string LeaveTypeName, string UserId, string Noofdays, string Leavebalance, string TravelTo, string TravelFrom, string DateofTravel, string DateofReturn, string Accomodation)
        {
            string msg = "";
            string val = ValidateBeforeSave(EmployeeID, LeaveType);
            if (val == "")
            {
                msg = save(AtchGuid, CompanyId, 2, EmployeeID, LeaveType, Remarks, Airticket, ReplacementId, StartDate, EndDate, ResumetoWork, LeaveTypeName, UserId, Noofdays, Leavebalance, TravelTo, TravelFrom, DateofTravel, DateofReturn, Accomodation);
            }
            return msg;
        }
        protected string ValidateBeforeSave(string EmployeeID, string LeaveType)
        {
            //Check for marriage it should be count only 1
            string lblStatus = "";
            if (LeaveType == "8")
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeID);
                ht1.Add("@chk", "8");
                if (_requestRepo.CheckLeaveApplicationValidations(ht1) > 0)
                {
                    lblStatus = "Sorry! Marriage leave - Is allowed once during the employment.";
                    //lblStatus.ForeColor = Color.Red;
                    return lblStatus;
                }
                else
                {

                    return lblStatus;
                }

            }
            else if (LeaveType == "10")
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeID);
                ht1.Add("@chk", "10");
                if (_requestRepo.CheckLeaveApplicationValidations(ht1) > 0)
                {
                    lblStatus = "Sorry! Paternity leave - Is allowed once during the year.";
                    return lblStatus;
                }
                else
                {
                    return lblStatus;
                }

            }
            else if (LeaveType == "5")
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeID);
                ht1.Add("@chk", "5");
                if (_requestRepo.CheckLeaveApplicationValidations(ht1) > 0)
                {
                    lblStatus = "Sorry! Hajj leave - Is allowed once during the employment";
                    return lblStatus;
                }
                else
                {
                    return lblStatus;
                }

            }
            else
            {
                return lblStatus;
            }
        }
        private string save(string AtchGuid, string CompanyId, int saveStatus, string EmployeeID, string LeaveType, string Remarks, string Airticket, string ReplacementId, string StartDate, string EndDate, string ResumetoWork, string LeaveTypeName, string UserId, string Noofdays, string Leavebalance, string TravelTo, string TravelFrom, string DateofTravel, string DateofReturn, string Accomodation)
        {
            string lblStatus = "";

            try
            {

                Hashtable ht1 = new Hashtable();
                ht1.Add("@UserID", EmployeeID);
                ht1.Add("@start", DateTime.Parse(StartDate).Date);
                ht1.Add("@end", DateTime.Parse(EndDate).Date);
                ht1.Add("@type", LeaveType);

                int duplicateCHk = 0; int attachmentCHk = 0;
                duplicateCHk = _requestRepo.CheckforDuplicateLeaves(ht1);

                Hashtable ht2 = new Hashtable();


                //Discuss it with sir and anas bhai ************

                //ht2.Add("@DocId", hdnAtchGuid.Value);

                //attachmentCHk = Convert.ToInt32(clsDAL.ExecuteScalar("sp_User_CheckForAttachment", ht2));



                if (DateTime.Parse(EndDate).Date < DateTime.Parse(StartDate).Date)
                {
                    duplicateCHk = 1;
                }

                if (duplicateCHk == 0)
                {
                    if ((DateTime.Parse(EndDate).Date == DateTime.Parse(StartDate).Date) || LeaveTypeName != "Half Day Leave")
                    {
                        if ((/*attachmentCHk > 0 && */LeaveTypeName == "Sick Leave") || LeaveTypeName != "Sick Leave")
                        {

                            //if (((Convert.ToDecimal(lblLeaveBalance.Text) - Convert.ToDecimal(lblNoOfDays.Text)) >= 0 || rCmbLeaveType1.SelectedItem.Text != "Annual Leave"))
                            //{

                            string EmpID = "";
                            //if (string.IsNullOrEmpty(rCmbReplacementEmployee.SelectedItem.Value))
                            //    EmpID = rCmbReplacementEmployee.SelectedItem.Value;

                            if (ReplacementId != null && ReplacementId != "Select Replacement Name")
                                EmpID = ReplacementId;

                            Hashtable ht = new Hashtable();
                            int FormId = GetFormTypeID("LeaveAppForm");

                            ht.Add("@FormTypeID", FormId);
                            ht.Add("@EmployeeUserID", int.Parse(EmployeeID));
                            ht.Add("@LeaveTypeID", int.Parse(LeaveType));
                            ht.Add("@LeaveStepID", int.Parse(LeaveType));
                            ht.Add("@LeaveStartDate", string.Format("{0:dd-MM-yyyy}", DateTime.Parse(StartDate)));
                            ht.Add("@LeaveEndDate", string.Format("{0:dd-MM-yyyy}", DateTime.Parse(EndDate)));
                            ht.Add("@Remarks", Remarks);
                            ht.Add("@PayInAdvance", false);
                            ht.Add("@DuringLeaveAddress1", "");
                            ht.Add("@DuringLeaveAddress2", "");
                            ht.Add("@DuringLeaveMobile", "");
                            ht.Add("@DuringLeavePhone", "");
                            ht.Add("@ReplacementEmployeeID", EmpID);
                            ht.Add("@SubmittedByUserID", int.Parse(UserId));
                            ht.Add("@SaveStatus", saveStatus);
                            ht.Add("@cDays", 0);
                            ht.Add("@NoDays", Noofdays);
                            ht.Add("@AtchGuid", AtchGuid);
                            ht.Add("@LeaveRequestID", 0);
                            ht.Add("@LeaveBalance", Convert.ToDecimal(string.IsNullOrEmpty(Leavebalance) ? "0" : Leavebalance));
                            ht.Add("@AirTicketRequired", Airticket == "on" ? true : false);
                            ht.Add("@CompanyId", CompanyId);
                            ht.Add("@Accomodation", Accomodation == "on" ? true : false);
                            Guid guid = Guid.NewGuid();
                            ht.Add("@RequestKey", guid);
                            string d = string.Format("{0:dd-MM-yyyy}", DateTime.Parse(ResumetoWork));
                            ht.Add("@ResumeDuty", d);

                            if (Airticket == "on" ? true : false)
                            {
                                ht.Add("@travelFrom", string.IsNullOrEmpty(TravelFrom) ? "" : TravelFrom);
                                ht.Add("@travelTo", string.IsNullOrEmpty(TravelTo) ? "" : TravelTo);
                            }
                            else
                            {
                                ht.Add("@travelFrom", "");
                                ht.Add("@travelTo", "");
                            }

                            if (string.IsNullOrEmpty(DateofTravel))
                            {
                                DateofTravel = Convert.ToDateTime("01-01-1999").ToString();
                            }
                            if (string.IsNullOrEmpty(DateofReturn))
                            {
                                DateofReturn = Convert.ToDateTime("01-01-1999").ToString();
                            }
                            ht.Add("@Dateoftravel", string.Format("{0:MM/dd/yyyy}", DateTime.Parse(DateofTravel)));
                            ht.Add("@DatetofReturn", string.Format("{0:MM/dd/yyyy}", DateTime.Parse(DateofReturn)));
                            ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Leave_Request"));

                            int ReqID = _requestRepo.spUserInsertLeaveRequest(ht);

                            if (ReqID > 0)
                            {
                                lblStatus = "Your request has been saved successfully.";
                                if (saveStatus == 2)
                                {
                                    ht = null;
                                    ht = new Hashtable();
                                    ht.Add("@LeaveRequestID", ReqID);

                                    //#region Send Email to Approver
                                    DataTable dt = DBContext.GetDataSet("sp_User_Get_Leave_Request_Info_4_Email", ht).Tables[0];

                                    if ((null != dt) && dt.Rows.Count > 0 && dt.Rows[0]["Email"] != null)
                                    {
                                        string sBody = "";
                                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                                        sBody = File.ReadAllText(htmlEmailFormat);
                                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                                        sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

                                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
                              "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                                        sBody = sBody.Replace("<%ApproveLink%>", url);
                                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
                              "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                                        sBody = sBody.Replace("<%RejectLink%>", url2);
                                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                                    }
                                    //#endregion

                                    //#region send email to Replsement Employee
                                    DataTable dtLvRep = DBContext.GetDataSet("sp_User_Get_Leave_RequestReplacement_Info_4_Email", ht).Tables[0];
                                    foreach (DataRow RW in dtLvRep.Rows)
                                    {
                                        string sBody = "";
                                        string htmlEmailFormat = System.IO.File.ReadAllText(@"C:\Users\Administrator\Desktop\EmailTemplates\EmailTemplates/NotifyEmpReplaceEmail.htm"); //("~/EmailTemplates/NotifyEmpReplaceEmail.htm");

                                        sBody = File.ReadAllText(htmlEmailFormat);
                                        sBody = sBody.Replace("<%UserFullName%>", RW["MainApproverFullName"].ToString());
                                        sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", RW["RequestDate"]));

                                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", RW["LeaveStartDate"]));
                                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", RW["LeaveEndDate"]));
                                        sBody = sBody.Replace("<%cDays%>", RW["NoDays"].ToString());

                                        sBody = sBody.Replace("<%Type%>", RW["RequestType"].ToString());
                                        sBody = sBody.Replace("<%Remarks%>", RW["Remarks"].ToString());

                                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                                        sBody = sBody.Replace("<%RequesterFullName%>", RW["RequesterFullName"].ToString());

                                        //User/Forms/LeaveApplication.aspx?FormType=1&SelTab=2
                                        Uri uri = HttpContext.Current.Request.Url;
                                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}{3}", uri.Scheme,
                                                    Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath) + "?FormType=1&SelTab=2");
                                        //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                                        clsCommon.SendMail(sBody, RW["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "Leave replacement.");
                                    }
                                    //#endregion

                                }

                            }
                            else
                            {
                                lblStatus = "Sorry! some error has occurred. Please try again later.";
                            }
                        }
                        else
                        {
                            lblStatus = "Attachment is required.";
                        }


                    }
                    else
                    {
                        lblStatus = "Invalid Start & End Date ,it should be same in case of Half Day Leave Request.";
                    }
                }
                else
                {
                    lblStatus = "Record already exists in same dates or ToDate must be greater then FromDate.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public LeaveApplicationListViewModel GetLeaveApplicationData(string EmpId, string UserID, string CompanyId)
        {
            LeaveApplicationListViewModel list = new LeaveApplicationListViewModel();
            Hashtable ht = new Hashtable();
            DataTable dt;
            int FormId = GetFormTypeID("LeaveAppForm");

            ht.Add("@FilterOnUser", "0");
            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            if (!string.IsNullOrEmpty(EmpId) && EmpId != "0")
            {
                Hashtable ht2 = new Hashtable();
                ht2.Add("@FilterOnUser", "1");
                ht2.Add("@UserID", EmpId);
                ht2.Add("@FormTypeID", FormId);
                ht2.Add("@CompanyId", CompanyId);
                dt = _requestRepo.GetAllLeaveRequests(ht2);
            }
            else
            {
                dt = _requestRepo.GetAllLeaveRequests(ht);
            }
            List<LeaveApplicationViewModel> History = new List<LeaveApplicationViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                LeaveApplicationViewModel single = new LeaveApplicationViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    LeaveType = item["LeaveType"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    Approver = "",//item[""].ToString(),
                    ReqStatus = item["RequestStatus"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    From = item["LeaveStartDate"].ToString(),
                    To = item["LeaveEndDate"].ToString(),
                    NoofDays = item["NoDays"].ToString(),
                    Leavebalance = item["LeaveBalance"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    AttachmentGuid = item["AtchGuid"].ToString(),
                    RequestID = item["Id"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    //isResumeDuty=string.IsNullOrEmpty(item["ResumeDutyCustom"].ToString())?false:true

                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            DataTable dt2;
            List<LeaveApplicationViewModel> Approval = new List<LeaveApplicationViewModel>();
            ht.Add("@FilterOnUser", "0");
            ht.Add("@UserID", "");
            ht.Add("@ApproverUserID", int.Parse(UserID));
            ht.Add("@CompanyId", CompanyId);
            if (!string.IsNullOrEmpty(EmpId) && EmpId != "0")
            {
                Hashtable ht2 = new Hashtable();
                ht2.Add("@FilterOnUser", "1");
                ht2.Add("@UserID", EmpId);
                ht2.Add("@ApproverUserID", UserID);
                ht2.Add("@CompanyId", CompanyId);

                dt2 = _requestRepo.GetLeaveApplicationApprovalData(ht2);

            }
            else
            {
                dt2 = _requestRepo.GetLeaveApplicationApprovalData(ht);
            }
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                LeaveApplicationViewModel single2 = new LeaveApplicationViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    LeaveType = item2["LeaveType"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    Approver = "",//item[""].ToString(),
                    ReqStatus = item2["LeaveStatusID"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    From = item2["LeaveStartDate"].ToString(),
                    To = item2["LeaveEndDate"].ToString(),
                    NoofDays = item2["NoDays"].ToString(),
                    Leavebalance = item2["LeaveBalance"].ToString(),
                    AttachmentGuid = item2["AtchGuid"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    RequestID = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString()
                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        protected bool isEditVisible(string Last_Status_ID, int EmployeeUserID)
        {
            if (Last_Status_ID != "" && Last_Status_ID != null)
            {
                clsCommon.RequestStatus statusID = (clsCommon.RequestStatus)int.Parse(Last_Status_ID);

                switch (statusID)
                {
                    case clsCommon.RequestStatus.Initiated:
                        return true;
                    case clsCommon.RequestStatus.Edited:
                        return true;
                    case clsCommon.RequestStatus.Recalled:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
        protected bool isRecallVisible(string Last_Status_ID, int EmployeeUserID)
        {
            if (Last_Status_ID != "" && Last_Status_ID != null)
            {
                clsCommon.RequestStatus statusID = (clsCommon.RequestStatus)int.Parse(Last_Status_ID);

                switch (statusID)
                {
                    case clsCommon.RequestStatus.Pending:
                        return true;
                    case clsCommon.RequestStatus.InProcess:
                        return true;
                    case clsCommon.RequestStatus.Approved:
                        return true;
                    case clsCommon.RequestStatus.Canceled:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
        protected bool isSubmitVisible(string Last_Status_ID, int EmployeeUserID)
        {
            if (Last_Status_ID != "" && Last_Status_ID != null)
            {
                clsCommon.RequestStatus statusID = (clsCommon.RequestStatus)int.Parse(Last_Status_ID);

                switch (statusID)
                {
                    case clsCommon.RequestStatus.Initiated:
                        return true;
                    case clsCommon.RequestStatus.Edited:
                        return true;
                    case clsCommon.RequestStatus.Recalled:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;

        }
        protected bool isInProcessVisible(string Last_Status_ID)
        {
            if (Last_Status_ID != "" && Last_Status_ID != null)
            {
                clsCommon.RequestStatus statusID = (clsCommon.RequestStatus)int.Parse(Last_Status_ID);

                switch (statusID)
                {
                    case clsCommon.RequestStatus.Pending:
                        return true;
                    case clsCommon.RequestStatus.InProcess:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
        protected bool isCompletedVisible(string Last_Status_ID)
        {
            if (Last_Status_ID != "" && Last_Status_ID != null)
            {
                clsCommon.RequestStatus statusID = (clsCommon.RequestStatus)int.Parse(Last_Status_ID);

                switch (statusID)
                {
                    case clsCommon.RequestStatus.Completed:
                        return true;
                    default:
                        return false;
                }
            }
            else
                return false;
        }
        public LeaveApplicationViewModel GetEmployeeLeavesEditData(string Reqid)
        {
            LeaveApplicationViewModel UserDD = new LeaveApplicationViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);
            DataTable dt = _requestRepo.GetLeaveRequestByID(ht);
            UserDD.EmployeeId = dt.Rows[0]["EmployeeUserID"].ToString();
            UserDD.LeaveType = dt.Rows[0]["LeaveTypeID"].ToString();
            UserDD.From = dt.Rows[0]["LeaveStartDate"].ToString();
            UserDD.To = dt.Rows[0]["LeaveEndDate"].ToString();
            UserDD.Remarks = dt.Rows[0]["Remarks"].ToString();
            UserDD.ReplacementEmployee = dt.Rows[0]["ReplacementEmployeeID"].ToString();
            UserDD.NoofDays = dt.Rows[0]["NoOfDays"].ToString();
            return UserDD;
        }
        public string SaveEditLeaveRequest(string Reqid, string EmployeeID, string LeaveType, string Remarks, string ReplacementId, string StartDate, string EndDate, string UserId, string Companyid, string Noofdays, string Leavebalance)
        {
            string msg = "";
            msg = saveEdit(Reqid, 2, EmployeeID, LeaveType, Remarks, ReplacementId, StartDate, EndDate, UserId, Companyid, Noofdays, Leavebalance);
            return msg;
        }
        private string saveEdit(string Reqid, int saveStatus, string EmployeeID, string LeaveType, string Remarks, string ReplacementId, string StartDate, string EndDate, string UserId, string Companyid, string Noofdays, string Leavebalance)
        {
            string lblStatus = "";

            try
            {
                string ReqID = Reqid;

                //string EmpID = "";
                //if (!string.IsNullOrEmpty(ReplacementId))
                //    EmpID = ReplacementId;

                Hashtable ht = new Hashtable();

                ht.Add("@LeaveRequestID", ReqID);
                ht.Add("@EmployeeUserID", EmployeeID);
                ht.Add("@LeaveTypeID", LeaveType);
                ht.Add("@LeaveStepID", 0);
                ht.Add("@LeaveStartDate", string.Format("{0:dd/MM/yyyy}", StartDate));
                ht.Add("@LeaveEndDate", string.Format("{0:dd/MM/yyyy}", EndDate));
                ht.Add("@Remarks", Remarks);
                ht.Add("@PayInAdvance", false);
                ht.Add("@DuringLeaveAddress1", "");
                ht.Add("@DuringLeaveAddress2", "");
                ht.Add("@DuringLeaveMobile", "");
                ht.Add("@DuringLeavePhone", "");
                ht.Add("@ReplacementEmployeeID", Convert.ToDecimal(string.IsNullOrEmpty(ReplacementId) ? "0" : ReplacementId));
                ht.Add("@SubmittedByUserID", UserId);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@cDays", 0);
                ht.Add("@NoDays", Noofdays);
                ht.Add("@AtchGuid", "");
                ht.Add("@DBMessage", "");
                ht.Add("@LeaveBalance", Convert.ToDecimal(string.IsNullOrEmpty(Leavebalance) ? "0" : Leavebalance));

                string DBMessage = _requestRepo.UpdateLeaveRequest(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@LeaveRequestID", ReqID);

                    //#region Send Email to Approver
                    DataTable dt = _requestRepo.GetLeaveRequestInfo4Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0 && dt.Rows[0]["Email"] != null)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + Companyid.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + Companyid.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                    }
                    //#endregion

                    //#region send email to Replsement Employee
                    //DataTable dtLvRep = _requestRepo.GetLeaveRequestReplacementInfo4Email(ht);
                    //foreach (DataRow RW in dtLvRep.Rows)
                    //{
                    //    string sBody = "";
                    //    string htmlEmailFormat = "";//Server.MapPath("~/EmailTemplates/NotifyEmpReplaceEmail.htm");

                    //    sBody = File.ReadAllText(htmlEmailFormat);
                    //    sBody = sBody.Replace("<%UserFullName%>", RW["MainApproverFullName"].ToString());
                    //    sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                    //    sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", RW["RequestDate"]));

                    //    sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", RW["LeaveStartDate"]));
                    //    sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", RW["LeaveEndDate"]));
                    //    sBody = sBody.Replace("<%cDays%>", RW["NoDays"].ToString());

                    //    sBody = sBody.Replace("<%Type%>", RW["RequestType"].ToString());
                    //    sBody = sBody.Replace("<%Remarks%>", RW["Remarks"].ToString());

                    //    sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                    //    sBody = sBody.Replace("<%RequesterFullName%>", RW["RequesterFullName"].ToString());

                    //    //User/Forms/LeaveApplication.aspx?FormType=1&SelTab=2
                    //    Uri uri = HttpContext.Current.Request.Url;
                    //    sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}{3}", uri.Scheme,
                    //                Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath) + "?FormType=1&SelTab=2");
                    //    //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                    //    clsCommon.SendMail(sBody, RW["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "Leave replacement.");
                    //}
                    //#endregion
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public LeaveApplicationViewModel GetLeaveApproval(string Id)
        {
            LeaveApplicationViewModel lvm = new LeaveApplicationViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.GetLeaveRequestByID(ht);

            if (dt != null)
            {
                lvm.Employee = (string)dt.Rows[0]["UserFullName"];
                lvm.LeaveType = (string)dt.Rows[0]["LeaveType"];
                lvm.From = string.Format("{0:d/M/yyyy}", dt.Rows[0]["LeaveStartDate"]);
                lvm.To = string.Format("{0:d/M/yyyy}", dt.Rows[0]["LeaveEndDate"]);
                lvm.CalendarDays = string.Format("{0:###} day(s).", dt.Rows[0]["cDays"]);
                lvm.NoofDays = string.Format("{0:###} day(s).", dt.Rows[0]["NoDays"]);
                lvm.CompanyName = (string)dt.Rows[0]["Name"];

                dt = null;
                dt = _requestRepo.GetLeaveRequestRemarksList(ht);

                lvm.Remarks = "";
                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lvm.Remarks += "<div><strong><span>"
                                                        + dr["UserFullName"].ToString().Trim() + "</span>"
                                                        + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                        + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }

            return lvm;
        }
        public string SaveLeaveApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    //int finalApprovercheck = Convert.ToInt32(clsDAL.ExecuteScalar("sp_User_Get_Leave_RequestsApprovers_Count"));
                    Hashtable htcheck = new Hashtable();

                    htcheck.Add("@leaveID", Id);
                    htcheck.Add("@Status", StatusId);

                    //int finalApprovercheck = int.Parse(clsDAL.ExecuteNonQuery("sp_User_Get_Leave_RequestsApprovers_Count", htcheck, "@Status", System.Data.SqlDbType.Int, 0).ToString());


                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck==1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (postCheck || finalApprovercheck==0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@LeaveRequestID", Id);
                    ht.Add("@LeaveStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);

                    string DBMessage = _requestRepo.ApproveLeaveRequest(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@LeaveRequestID", Id);
                    ht.Add("@Approver", Convert.ToInt32(UserId));
                    DataTable dt = _requestRepo.GetLeaveRequestSubmitterInfo4Email(ht);
                    var dt1 = DBContext.ExecuteReaderWithCommand("select MainApproverUserID, WorkflowID from dbo.Get_Next_Approver_Of_Leave_Request(" + Id + ")");
                    int MainApproverUserID = 0;
                    while (dt1.Read())
                    {
                        MainApproverUserID = int.Parse(dt1["MainApproverUserID"].ToString());
                        //MainApproverUserID = int.Parse(dt1[0].ToString());
                    }
                    if ((null != dt) && dt.Rows.Count > 0 && MainApproverUserID != 0)
                    {

                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        ErrorMsg = "";
                        ErrorMsg = sBody;
                        // clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    else if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyRequestSubmitter.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LeaveAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        //sBody = sBody.Replace("<%RedirectURL%>", "http://" + HttpContext.Current.Request.Url.Host + "/User/Forms/LeaveApplication.aspx?FormType=1&SelTab=1");
                        ErrorMsg = "";
                        ErrorMsg = sBody;
                        //clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@LeaveRequestID", Id);
                    ht.Add("@LeaveStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);
                    string DBMessage = _requestRepo.RejectLeaveRequest(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@LeaveRequestID", Id);
                    ht.Add("@Approver", Convert.ToInt32(UserId));
                    DataTable dt = _requestRepo.GetLeaveRequestSubmitterInfo4Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyRequestSubmitter.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);

                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Rejected");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }



                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }
        public List<LeaveApplicationViewModel> GetLeaveDetails(string Id)
        {
            List<LeaveApplicationViewModel> lvmm = new List<LeaveApplicationViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@LeaveRequestID", Id);
            DataTable dt = _requestRepo.GetLeaveDetails(ht);

            foreach (DataRow item in dt.Rows)
            {
                LeaveApplicationViewModel lvm = new LeaveApplicationViewModel();
                lvm.RequestDate = item["ProcessedDate"].ToString();
                lvm.ReqStatus = item["RequestStatus"].ToString();
                lvm.Description = item["RequestStatusDesc"].ToString();
                lvm.Approver = item["MainApproverFullName"].ToString();
                lvm.Employee = item["ApprovedByFullName"].ToString();
                lvm.Remarks = item["Remarks"].ToString();
                lvmm.Add(lvm);
            }
            return lvmm;
        }
        public string SaveResumptions(string Id, string Date, string Remarks)
        {
            string ErrorMsg = "";

            try
            {
                Hashtable ht = new Hashtable();

                ht.Add("@LeaveRequestID", Id);
                ht.Add("@DBMessage", "");
                ht.Add("@ResumptionDate", string.Format("{0:yyyy-MM-dd}", Date));
                ht.Add("@Remarks", Remarks);

                string DBMessage = _requestRepo.User_Resume_Leave_Request(ht);

                ht.Remove("@ResumptionDate");
                ht.Remove("@DBMessage");
                ht.Remove("@Remarks");
                DataTable dt = _requestRepo.User_Get_ResumeLeave_Trx_ByID(ht);

                if ((null != dt) && dt.Rows.Count > 0 && !string.IsNullOrEmpty(DBMessage) && !DBMessage.Contains("ERROR:"))
                {
                    string sBody = "";
                    //string htmlEmailFormat = Server.MapPath("~/EmailTemplates/NotifyResumeDutyEmail.htm");
                    string htmlEmailFormat = System.IO.File.ReadAllText(@"C:\Users\Administrator\Desktop\EmailTemplates\EmailTemplates/NotifyResumeDutyEmail.htm");
                    sBody = File.ReadAllText(htmlEmailFormat);
                    sBody = sBody.Replace("<%TRX_ID%>", dt.Rows[0]["TRX_ID"].ToString());
                    sBody = sBody.Replace("<%YEAR%>", dt.Rows[0]["YEAR1"].ToString());
                    sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["TRXDATE"]));
                    sBody = sBody.Replace("<%Type%>", dt.Rows[0]["LeaveType"].ToString());
                    sBody = sBody.Replace("<%ResumeDate%>", string.Format("{0:dd/MM/yyyy}", Date));
                    sBody = sBody.Replace("<%Period%>", string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", dt.Rows[0]["From_Date"], dt.Rows[0]["TODATE"]));
                    sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["UserFullName"].ToString());
                    clsCommon.SendMail(sBody, ConfigurationManager.AppSettings["To_Email_4_Duty_Resumption"], ConfigurationManager.AppSettings["EMAIL_ACC"], "Duty Resumption Notification.");
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    ErrorMsg = "Sorry! some error has occurred. Please try again later. <br />" + DBMessage;
                }
                else
                {
                    ErrorMsg = "Processed successfully.";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Sorry! some error has occurred. Please try again later. <br />" + ex.Message;
            }
            return ErrorMsg;
        }

        #endregion

        #region Reimbursement

        public List<DropDownBindViewModel> GetReimbursementType()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetReimbursementType();
            while (dt.Read())
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = dt["Name"].ToString(),
                    Value = dt["ID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public List<DropDownBindViewModel> GetCurrencyCode()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetCurrencyCode();
            while (dt.Read())
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = dt["CurrencyCode"].ToString(),
                    Value = dt["CurrencyCode"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public List<DropDownBindViewModel> GetActivityTypes()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetActivityTypes();
            while (dt.Read())
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = dt["Name"].ToString(),
                    Value = dt["ID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public string Reimbursement(string AtchGuid, string Type, List<ReimbursementSaveViewModel> RModle, string UserId, string CompanyId)
        {
            string ErrorMsg = "";
            int FormId = GetFormTypeID("ReimburseForm");

            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", UserId);
                ht.Add("@SaveStatus", int.Parse(Type));
                ht.Add("@ReimbursementRequestID", 0);
                ht.Add("@AtchGuid", AtchGuid);
                ht.Add("@Remarks", "");
                ht.Add("@CompanyId", CompanyId);
                Guid guid = Guid.NewGuid();
                ht.Add("@RequestKey", guid);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Reimbursement_Request"));
                int ReqID = _requestRepo.InsertReimbursementRequest(ht);

                if (ReqID > 0)
                {

                    foreach (var dr in RModle)
                    {
                        ht = null;
                        ht = new Hashtable();

                        ht.Add("@ReimbursementRequestID", ReqID);
                        ht.Add("@FromDate", string.Format("{0:dd/MM/yyyy}", dr.From));
                        ht.Add("@ToDate", string.Format("{0:dd/MM/yyyy}", dr.To));
                        ht.Add("@ReceiptDate", string.Format("{0:dd/MM/yyyy}", dr.Reciept));
                        ht.Add("@Location", dr.City);
                        ht.Add("@ActivityTypes", dr.ActivityType);
                        ht.Add("@ActivityId", dr.ActivityId);
                        ht.Add("@VisitedCurr", dr.Currency);
                        ht.Add("@VisitedAmount", dr.Amount);
                        ht.Add("@Remarks", dr.Remarks);
                        ht.Add("@ReimbursmentType", dr.ReimbursementType);

                        _requestRepo.InsertReimbursmentRequestDetail(ht);
                    }

                    ErrorMsg = "Your request has been saved successfully.";
                    if (Type == "2")
                    {

                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@ReimbursementRequestID", ReqID);

                        DataTable dt = _requestRepo.GetReimbursementRequestInfo4Email(ht);

                        if ((null != dt) && dt.Rows.Count > 0)
                        {
                            string sBody = "";
                            string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                            htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                            sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                            sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                            string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

                            var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=ReimbursementForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%ApproveLink%>", url);
                            var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=ReimbursementForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%RejectLink%>", url2);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");

                        }
                    }
                    //#regin Send Email Notification to HR
                    string qry = @"select HrEmployeeId from tbl_WorkFlowMaster where ID=(select uwfm.WorkFlowMasterID from dbo.tbl_User_WorkFlow_Mapping uwfm
inner join dbo.tbl_WorkFlowMaster wfm on wfm.ID = uwfm.WorkFlowMasterID
     where uwfm.UserID =" + UserId +
      "and wfm.FormTypeID = " + FormId + ")";
                    string EmployeeId = "";
                    var a = DBContext.ExecuteReaderWithCommand(qry);
                    while (a.Read())
                    {
                        EmployeeId = a[0].ToString();
                    }
                    if (!string.IsNullOrEmpty(EmployeeId))
                    {
                        var HREmployee = GetEmployeeDeatils(EmployeeId);
                        var Employee = GetEmployeeDeatils(UserId);
                        string sBody = "";
                        string htmlEmailFormat = HttpContext.Current.Server.MapPath("~/EmailTemplates/NotifyHREmail.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", Employee.Name);
                        sBody = sBody.Replace("<%ID%>", "");
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", DateTime.Now));
                        sBody = sBody.Replace("<%Type%>", "Reimbursement");
                        sBody = sBody.Replace("<%Remarks%>", "");
                        //sBody = sBody.Replace("<%RedirectURL%>", "");
                        clsCommon.SendMail(sBody, HREmployee.Email, ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is Generated for Reimbursement.");

                    }
                    //#endregion
                }
                else
                {
                    ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return ErrorMsg;
        }
        public ReimbursementListViewModel GetReimbursementList(string UserId, string CompanyId)
        {
            ReimbursementListViewModel UserDD = new ReimbursementListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("ReimburseForm");

            ht.Add("@UserID", UserId);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            var dt = _requestRepo.GetReimbursementList(ht);

            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserId);
            ht.Add("@CompanyId", CompanyId);

            var dt2 = _requestRepo.GetReimbursementListForApproval(ht);

            if ((null != dt2) && dt2.Rows.Count > 0)
                UserDD.IsApproval = true;
            else
            {
                UserDD.IsApproval = false;

            }
            foreach (DataRow item in dt2.Rows)
            {

                ReimbursementDataListViewModel User = new ReimbursementDataListViewModel()
                {
                    EmployeeId = item["EMPLOYID"].ToString(),
                    Ref = item["RequestID"].ToString(),
                    ReimbursementType = item["ReimbursementType"].ToString(),
                    RequestedDate = item["RequestDate"].ToString(),
                    ReimbursementStatusID = item["ReimbursementStatusID"].ToString(),
                    SubmittedBy = item["UserName"].ToString(),
                    TrxId = "",
                    RequestID = item["Id"].ToString(),
                    AttachmentGuid = item["AtchGuid"].ToString(),
                    CompanyName = item["Name"].ToString()

                    //item[""].ToString(),
                    //IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EMPLOYID"].ToString())),
                    //isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EMPLOYID"].ToString())),
                    //isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EMPLOYID"].ToString())),
                    //isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    //isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                };
                UserDD.Approval.Add(User);
            }

            foreach (DataRow item in dt.Rows)
            {

                ReimbursementDataListViewModel User = new ReimbursementDataListViewModel()
                {
                    EmployeeId = item["EMPLOYID"].ToString(),
                    Ref = item["RequestID"].ToString(),
                    ReimbursementType = item["ReimbursementType"].ToString(),
                    RequestedDate = item["RequestDate"].ToString(),
                    SubmittedBy = item["UserName"].ToString(),
                    TrxId = "",//item[""].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    AttachmentGuid = item["AtchGuid"].ToString(),
                    RequestID = item["Id"].ToString(),

                };
                UserDD.History.Add(User);
            }
            return UserDD;
        }
        public List<ReimbursementEditViewModel> GetEditDatabind(string Reqid, string UserId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@ReimbustRequestID", Reqid);

            var dt = _requestRepo.GetReimbursementRequestDetail(ht);

            List<ReimbursementEditViewModel> list = new List<ReimbursementEditViewModel>();
            foreach (DataRow item in dt.Rows)
            {
                ReimbursementDropdownBindingViewModel rvm = new ReimbursementDropdownBindingViewModel()
                {
                    Countries = GetCountries(),
                    //Cities = _requestService.GetCities(""),
                    ReimbursementType = GetReimbursementType(),
                    CurrencyCode = GetCurrencyCode(),
                    ActivityTypes = GetActivityTypes()
                };
                ReimbursementEditViewModel single = new ReimbursementEditViewModel()
                {
                    ActivityId = item["ActivityIdd"].ToString(),
                    Amount = item["VisitedAmount"].ToString(),
                    ActivityType = item["ActivityID"].ToString(),
                    City = item["Location"].ToString(),
                    Country = item["Location"].ToString(),
                    Currency = item["VisitedCurr"].ToString(),
                    From = item["FromDate"].ToString(),
                    ImbursementId = Reqid,
                    Remarks = item["Remarks"].ToString(),
                    Reciept = item["ReceiptDate"].ToString(),
                    RMTYPE = item["ReimbursementType"].ToString(),
                    To = item["ToDate"].ToString()
                };
                single.dropdowns = new ReimbursementDropdownBindingViewModel();
                single.dropdowns = rvm;
                list.Add(single);
            }
            return list;
        }
        public string EditSaveReimbursement(string Type, List<ReimbursementSaveViewModel> RModle, string UserId, string Id, bool Recall)
        {
            string ErrorMsg = "";
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@SaveStatus", int.Parse(Type));
                ht.Add("@ReimbursementRequestID", int.Parse(Id));
                ht.Add("@Remarks", "");
                ht.Add("@DBMessage", "");
                ht.Add("@isRecalled", Recall);
                string DBMessage = _requestRepo.EditSaveReimbursementRequest(ht);

                if (!string.IsNullOrEmpty(DBMessage) && !DBMessage.Contains("ERROR:"))
                {

                    foreach (var dr in RModle)
                    {
                        ht = null;
                        ht = new Hashtable();

                        ht.Add("@ReimbursementRequestID", Id);
                        ht.Add("@FromDate", string.Format("{0:dd/MM/yyyy}", dr.From));
                        ht.Add("@ToDate", string.Format("{0:dd/MM/yyyy}", dr.To));
                        ht.Add("@ReceiptDate", string.Format("{0:dd/MM/yyyy}", dr.Reciept));
                        ht.Add("@Location", dr.City);
                        ht.Add("@ActivityTypes", dr.ActivityType);
                        ht.Add("@ActivityId", dr.ActivityId);
                        ht.Add("@VisitedCurr", dr.Currency);
                        ht.Add("@VisitedAmount", dr.Amount);
                        ht.Add("@Remarks", dr.Remarks);
                        ht.Add("@ReimbursmentType", dr.ReimbursementType);

                        _requestRepo.InsertReimbursmentRequestDetail(ht);
                    }

                    ErrorMsg = "Your request has been saved successfully.";
                    if (Type == "2")
                    {

                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@ReimbursementRequestID", Id);
                        ht.Add("@Approver", Convert.ToInt32(UserId));

                        DataTable dt = _requestRepo.GetReimbursementRequestInfo4Email(ht);

                        if ((null != dt) && dt.Rows.Count > 0)
                        {
                            string sBody = "";
                            string htmlEmailFormat = "";//Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", Id);
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                            sBody = sBody.Replace("<%RedirectURL%>", "");
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                            //hdnAtchGuid.Value = Guid.NewGuid().ToString();
                        }

                    }
                    //                    //#regin Send Email Notification to HR
                    //                    string qry = @"select HrEmployeeId from tbl_WorkFlowMaster where ID=(select uwfm.WorkFlowMasterID from dbo.tbl_User_WorkFlow_Mapping uwfm
                    //inner join dbo.tbl_WorkFlowMaster wfm on wfm.ID = uwfm.WorkFlowMasterID
                    //     where uwfm.UserID =" + UserId +
                    //      "and wfm.FormTypeID = " + FormId + ")";
                    //                    string EmployeeId = "";
                    //                    var a = DBContext.ExecuteReaderWithCommand(qry);
                    //                    while (a.Read())
                    //                    {
                    //                        EmployeeId = a[0].ToString();
                    //                    }
                    //                    if (!string.IsNullOrEmpty(EmployeeId))
                    //                    {
                    //                        var HREmployee = GetEmployeeDeatils(EmployeeId);
                    //                        var Employee = GetEmployeeDeatils(UserId);
                    //                        string sBody = "";
                    //                        string htmlEmailFormat = HttpContext.Current.Server.MapPath("~/EmailTemplates/NotifyHREmail.htm");

                    //                        sBody = File.ReadAllText(htmlEmailFormat);
                    //                        sBody = sBody.Replace("<%UserFullName%>", Employee.Name);
                    //                        sBody = sBody.Replace("<%ID%>", "");
                    //                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", DateTime.Now));
                    //                        sBody = sBody.Replace("<%Type%>", "Reimbursement");
                    //                        sBody = sBody.Replace("<%Remarks%>", "");
                    //                        //sBody = sBody.Replace("<%RedirectURL%>", "");
                    //                        clsCommon.SendMail(sBody, HREmployee.Email, ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is Generated for Reimbursement.");

                    //                    }
                    //                    //#endregion
                }
                else
                {
                    ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return ErrorMsg;
        }
        public List<ReimbursementEditViewModel> GetDetails(string Reqid, string UserId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@ReimbustRequestID", Reqid);

            var dt = _requestRepo.GetReimbursementRequestDetail(ht);

            List<ReimbursementEditViewModel> list = new List<ReimbursementEditViewModel>();
            foreach (DataRow item in dt.Rows)
            {

                ReimbursementEditViewModel single = new ReimbursementEditViewModel()
                {
                    ActivityId = item["ActivityIdd"].ToString(),
                    Amount = item["VisitedAmount"].ToString(),
                    ActivityType = item["ActivityName"].ToString(),
                    City = item["Location"].ToString(),
                    Country = item["Location"].ToString(),
                    Currency = item["VisitedCurr"].ToString(),
                    From = item["FromDate"].ToString(),
                    ImbursementId = Reqid,
                    Remarks = item["Remarks"].ToString(),
                    Reciept = item["ReceiptDate"].ToString(),
                    RMTYPE = item["ReimbursementName"].ToString(),
                    To = item["ToDate"].ToString()
                };
                list.Add(single);
            }
            return list;
        }
        public List<ReimbursementEditViewModel> GetDetailReport(string Empid,string Depid, string UserId)
        {
            Hashtable ht = new Hashtable();
            //ht.Add("@ReimbustRequestID", Reqid);

            var dt = _requestRepo.GetReimbursementRequestDetailReport(ht);

            List<ReimbursementEditViewModel> list = new List<ReimbursementEditViewModel>();
            foreach (DataRow item in dt.Rows)
            {

                ReimbursementEditViewModel single = new ReimbursementEditViewModel()
                {
                    RMTYPE = item["ReimbursementName"].ToString(),
                    From = item["FromDate"].ToString(),
                    To = item["ToDate"].ToString(),
                    Country = item["Location"].ToString(),
                    ActivityType = item["ActivityName"].ToString(),
                    Reciept = item["ReceiptDate"].ToString(),
                    Currency = item["VisitedCurr"].ToString(),
                    Amount = item["VisitedAmount"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    SubmittedBy = item["Username"].ToString(),
                    DEPRTMNT = item["DEPRTMNT"].ToString(),
                    DESIGNATION = item["DESIGNATION"].ToString(),
                };
                list.Add(single);
            }
            return list;
        }
        public List<LeaveViewModel> GetLeaveDetailReport(string Empid, string UserId)
        {
            Hashtable ht = new Hashtable();
            //ht.Add("@ReimbustRequestID", Reqid);

            var dt = _requestRepo.GetLeaveRequestDetailReport(ht);

            List<LeaveViewModel> list = new List<LeaveViewModel>();
            //foreach (DataRow item in dt.Rows)
            //{

            //    LeaveViewModel single = new LeaveViewModel()
            //    {
            //        RMTYPE = item["ReimbursementName"].ToString(),
            //        From = item["FromDate"].ToString(),
            //        To = item["ToDate"].ToString(),
            //        Country = item["Location"].ToString(),
            //        ActivityType = item["ActivityName"].ToString(),
            //        Reciept = item["ReceiptDate"].ToString(),
            //        Currency = item["VisitedCurr"].ToString(),
            //        Amount = item["VisitedAmount"].ToString(),
            //        Remarks = item["Remarks"].ToString(),
            //        SubmittedBy = item["Username"].ToString(),
            //        DEPRTMNT = item["DEPRTMNT"].ToString(),
            //        DESIGNATION = item["DESIGNATION"].ToString(),
            //    };
            //    list.Add(single);
            //}
            return list;
        }
        public string GetApproval(string Id)
        {
            string History = "";
            string Employee = "";
            string CompanyName = "";
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.GetReimbursementRequestByID(ht);

            if (dt != null)
            {
                Employee = (string)dt.Rows[0]["UserFullName"];
                CompanyName = (string)dt.Rows[0]["Name"];

                // lblReimbursementType.Text = (string)dt["ReimbursementType"];

                dt = null;
                dt = _requestRepo.GetReimbursementRequestRemarksList(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        History += "<div><strong><span>"
                                                        + dr["UserFullName"].ToString().Trim() + "</span>"
                                                        + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                        + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return Employee + "_" + History + "_" + CompanyName;
        }
        public string SaveApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@RequestID", Id);
                    htcheck.Add("@StatusID", StatusId);
                    int finalApprovercheck = _requestRepo.ApproveReimbursement(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@ReimbursementRequestID", Id);
                    ht.Add("@ReimbursementStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);
                    string DBMessage = _requestRepo.ApproveReimbursementRequest(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@ReimbursementRequestID", Id);
                    ht.Add("@Approver", UserId);

                    DataTable dt = _requestRepo.GetReimbursementRequestSubmitterInfo4Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";//Server.MapPath("~/EmailTemplates/NotifyRequestSubmitter.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id);
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/Reimbursement.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=2&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@ReimbursementRequestID", Id);
                    ht.Add("@ReimbursementStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);
                    string DBMessage = _requestRepo.RejectReimbursementRequest(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@ReimbursementRequestID", Id);
                    ht.Add("@Approver", UserId);

                    DataTable dt = _requestRepo.GetReimbursementRequestSubmitterInfo4Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyRequestSubmitter.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);

                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Rejected");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }
        #endregion

        #region LasteandAbsenceJustification
        public LateandAbsenceListViewModel GetLateandAbsenceJustificationData(string EmpId, string UserID, string CompanyId)
        {
            LateandAbsenceListViewModel list = new LateandAbsenceListViewModel();
            int FormId = GetFormTypeID("Late And Absence Justifications Form");

            Hashtable ht = new Hashtable();
            DataTable dt;
            ht.Add("@FilterOnUser", "0");
            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            if (!string.IsNullOrEmpty(EmpId) && EmpId != "0")
            {
                Hashtable ht2 = new Hashtable();
                ht2.Add("@FilterOnUser", "1");
                ht2.Add("@UserID", EmpId);
                ht2.Add("@FormTypeID", FormId);
                ht2.Add("@CompanyId", CompanyId);
                dt = _requestRepo.Get_All_LateandAbsence_Requests(ht2);
            }
            else
            {
                dt = _requestRepo.Get_All_LateandAbsence_Requests(ht);
            }
            List<LateandAbsenceViewModel> History = new List<LateandAbsenceViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                LateandAbsenceViewModel single = new LateandAbsenceViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    Approver = item["MainApproverFullName"].ToString(),
                    ReqStatus = item["RequestStatus"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    Remarks = item["Notes"].ToString(),
                    Category = item["Category"].ToString(),
                    Date = item["RequestDate"].ToString(),
                    SubCategory = item["SubCategory"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    AttachmentGuid = item["AtchGuid"].ToString(),
                    RequestID = item["Id"].ToString()
                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            DataTable dt2;
            List<LateandAbsenceViewModel> Approval = new List<LateandAbsenceViewModel>();
            ht.Add("@FilterOnUser", "0");
            ht.Add("@UserID", "");
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            if (!string.IsNullOrEmpty(EmpId) && EmpId != "0")
            {
                Hashtable ht2 = new Hashtable();
                ht2.Add("@FilterOnUser", "1");
                ht2.Add("@UserID", EmpId);
                ht2.Add("@ApproverUserID", UserID);
                ht.Add("@CompanyId", CompanyId);

                dt2 = _requestRepo.Get_LateandAbsence_Requests_For_Approval(ht2);

            }
            else
            {
                dt2 = _requestRepo.Get_LateandAbsence_Requests_For_Approval(ht);
            }
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                LateandAbsenceViewModel single2 = new LateandAbsenceViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    Approver = "",//item[""].ToString(),
                    ReqStatus = item2["StatusID"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    Category = item2["Category"].ToString(),
                    SubCategory = item2["SubCategory"].ToString(),
                    Date = item2["RequestDate"].ToString(),
                    RequestID = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString(),
                    AttachmentGuid = item2["AtchGuid"].ToString()
                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public string SaveLateandLeaveRequest(string AtchGuid, int saveStatus, string Company, string EmployeeID, string Category, string Date, string PunchIn, string PunchOut, string SubCategory, string Remarks, string UserId)
        {
            string ErroMsg = "";
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("Late And Absence Justifications Form");

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", EmployeeID);
                ht.Add("@Remarks", Remarks);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@rDate", Date);
                ht.Add("@Category", Category);
                ht.Add("@AtchGuid", AtchGuid);
                ht.Add("@CompanyId", Company);
                Guid guid = Guid.NewGuid();
                ht.Add("@RequestKey", guid);
                if (Category == "Punch - in/out")
                {
                    ht.Add("@SubCategory", "");
                    ht.Add("@PunchIn", PunchIn);
                    ht.Add("@PunchOut", PunchOut);

                }
                else
                {
                    ht.Add("@PunchIn", "");
                    ht.Add("@PunchOut", "");
                    ht.Add("@SubCategory", SubCategory);
                }
                ht.Add("@RequestID", 0);
                ht.Add("@RequestLocalID", GenerateID(Company, "tbl_Late_AbsenceJustification"));
                int ReqID = _requestRepo.Insert_LateandAbsence_Request(ht);

                if (ReqID > 0)
                {
                    ErroMsg = "Your request has been saved successfully.";
                    if (saveStatus == 2)
                    {
                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@RequestID", ReqID);

                        DataTable dt = _requestRepo.Get_LateandAbsence_Request_Info_4_Email(ht);

                        if ((null != dt) && dt.Rows.Count > 0)
                        {
                            string sBody = "";
                            string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                            htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                            sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                            sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                            sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                            string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

                            var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + Company.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LateandAbsence" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%ApproveLink%>", url);
                            var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + Company.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LateandAbsence" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%RejectLink%>", url2);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                        }
                    }
                }
                else
                {
                    ErroMsg = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
            }
            return ErroMsg;
        }
        public List<LateandAbsenceViewModel> GetLateandLeaveDetails(string Id)
        {
            List<LateandAbsenceViewModel> lvmm = new List<LateandAbsenceViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_LateandAbsence_Request_Statuses(ht);

            foreach (DataRow item in dt.Rows)
            {
                LateandAbsenceViewModel lvm = new LateandAbsenceViewModel();
                lvm.Date = item["ProcessedDate"].ToString();
                lvm.ReqStatus = item["RequestStatus"].ToString();
                lvm.Description = item["RequestStatusDesc"].ToString();
                lvm.Approver = item["MainApproverFullName"].ToString();
                lvm.ApprovedBy = item["ApprovedByFullName"].ToString();
                lvm.Remarks = item["Remarks"].ToString();
                lvmm.Add(lvm);
            }
            return lvmm;
        }
        public LateandAbsenceViewModel GetLateandAbsenceEditData(string Reqid)
        {
            LateandAbsenceViewModel UserDD = new LateandAbsenceViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);
            DataTable dt = _requestRepo.Get_LateandAbsence_Request_ByID(ht);
            UserDD.EmployeeId = dt.Rows[0]["EmployeeUserID"].ToString();
            UserDD.Date = dt.Rows[0]["RequestDate"].ToString();
            UserDD.SubCategory = dt.Rows[0]["SubCategory"].ToString();
            UserDD.Category = dt.Rows[0]["Category"].ToString();
            UserDD.Remarks = dt.Rows[0]["Notes"].ToString();
            UserDD.PunchIn = dt.Rows[0]["PunchIn"].ToString();
            UserDD.PunchOut = dt.Rows[0]["PunchOut"].ToString();
            return UserDD;
        }
        public string saveLateandAnsenceEdit(string Reqid, int saveStatus, string Category,
            string Remarks, string PunchIn, string PunchOut, string SubCategory, string Date)
        {
            string lblStatus = "";
            try
            {
                Hashtable ht = new Hashtable();

                ht.Add("@RequestID", Reqid);
                ht.Add("@Category", Category);
                if (Category == "Punch - in/out")
                {
                    ht.Add("@SubCategory", "");
                    ht.Add("@PunchIn", PunchIn);
                    ht.Add("@PunchOut", PunchOut);
                }
                else
                {
                    ht.Add("@PunchIn", "");
                    ht.Add("@PunchOut", "");
                    ht.Add("@SubCategory", SubCategory);
                }
                ht.Add("@rDate", Date);
                ht.Add("@Remarks", Remarks);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@DBMessage", "");

                string DBMessage = _requestRepo.EditInsert_LateandAbsence_Request(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Reqid);

                    DataTable dt = _requestRepo.Get_LateandAbsence_Request_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Reqid);
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["rDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        sBody = sBody.Replace("<%RedirectURL%>", "");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    }

                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public string GetLateandAbsenceApproval(string Id)
        {
            string History = "";
            string Employee = "";
            string CompanyName = "";
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_LateandAbsence_Request_ByID(ht);

            if (dt != null)
            {
                Employee = (string)dt.Rows[0]["UserFullName"];
                CompanyName = (string)dt.Rows[0]["Name"];

                // lblReimbursementType.Text = (string)dt["ReimbursementType"];

                dt = null;
                dt = _requestRepo.Get_LateandAbsence_Request_Remarks_List(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        History += "<div><strong><span>"
                                                    + dr["UserFullName"].ToString().Trim() + "</span><br/>"

                                                    + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return Employee + "_" + History + "_" + CompanyName;
        }
        public string SaveLeaveandAbsenceApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@leaveID", Id);
                    htcheck.Add("@Status", StatusId);
                    //int finalApprovercheck = _requestRepo.Get_LateAndAbsence_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@RequestID", Id);
                    ht.Add("@StatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);
                    string DBMessage = _requestRepo.Approve_LateandAbsence_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);

                    DataTable dt = _requestRepo.Get_LateandAbsence_RequestSubmitter_Info_4_Email(ht);

                    var dt1 = DBContext.ExecuteReaderWithCommand("select MainApproverUserID, WorkflowID from dbo.Get_Next_Approver_Of_LateandAbsence_Request(" + Id + ")");
                    int MainApproverUserID = 0;
                    while (dt1.Read())
                    {
                        MainApproverUserID = int.Parse(dt1["MainApproverUserID"].ToString());
                    }
                    if ((null != dt) && dt.Rows.Count > 0 && MainApproverUserID != 0)
                    {

                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LateandAbsenceForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LateandAbsenceForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Late and Absence Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    else if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LateandAbsenceForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=LateandAbsenceForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Late and Absence Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@RequestID", Id);
                    ht.Add("@StatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);

                    string DBMessage = _requestRepo.Reject_LateandAbsence_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_LateandAbsence_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;

                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Allowance Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }
            }
            return ErrorMsg;
        }

        #endregion

        #region AllownceRequest
        public List<DropDownBindViewModel> GetAllownceTypes()
        {
            List<DropDownBindViewModel> Countries = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetAllownceTypes();
            foreach (DataRow item in dt.Rows)

            {
                DropDownBindViewModel Country = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["ID"].ToString(),
                };
                Countries.Add(Country);
            }
            return Countries;
        }
        public string SaveAllowanceRequest(string AtchGuid, int saveStatus, string Date, string Amount, string Type, string Remarks, string CompanyId, string UserId)
        {
            string ErroMsg = "";
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("AllowanceRequest");

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", UserId);
                ht.Add("@AllowanceTypeID", Type);
                ht.Add("@AllowanceAmount", Amount);
                ht.Add("@RequestDate", Convert.ToDateTime(Date));
                ht.Add("@Remarks", Remarks.Trim());
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@AtchGuid", AtchGuid);
                ht.Add("@AllowanceRequestID", 0);
                ht.Add("@CompanyId", CompanyId);
                Guid guid = Guid.NewGuid();
                ht.Add("@RequestKey", guid);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Allowance_Request"));
                int ReqID = _requestRepo.SaveAllowanceRequest(ht);

                if (ReqID > 0)
                {
                    ErroMsg = "Your request has been saved successfully";

                    if (saveStatus == 2)
                    {
                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@AllowanceRequestID", ReqID);

                        DataTable dt = _requestRepo.Get_Allowance_Request_Info_4_Email(ht);

                        if ((null != dt) && dt.Rows.Count > 0)
                        {
                            string sBody = "";
                            string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                            htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                            sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                            sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                            sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                            string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

                            var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=AllowanceAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%ApproveLink%>", url);
                            var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
                            "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                    "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=AllowanceAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%RejectLink%>", url2);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                        }
                    }
                }
                else
                {
                    ErroMsg = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
            }
            return ErroMsg;
        }
        public AllowanceRequestListViewModel GetAllowanceRequestData(string UserID, string CompanyId)
        {
            AllowanceRequestListViewModel list = new AllowanceRequestListViewModel();

            Hashtable ht = new Hashtable();
            DataTable dt;
            int FormId = GetFormTypeID("AllowanceRequest");

            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            dt = _requestRepo.Get_All_Allowance_Requests(ht);
            List<AllowanceRequestViewModel> History = new List<AllowanceRequestViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                AllowanceRequestViewModel single = new AllowanceRequestViewModel()
                {
                    Date = item["RequestDate"].ToString(),
                    Amount = item["Amount"].ToString(),
                    Type = item["AllowanceTypeID"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    RequestId = item["RequestID"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    AttachmentGuid = item["AtchGuid"].ToString(),
                    RequestLoaclID = item["RequestLoaclID"].ToString()
                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            DataTable dt2;
            List<AllowanceRequestViewModel> Approval = new List<AllowanceRequestViewModel>();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            dt2 = _requestRepo.Get_Allowance_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                AllowanceRequestViewModel single2 = new AllowanceRequestViewModel()
                {
                    Date = item2["RequestDate"].ToString(),
                    Amount = item2["Amount"].ToString(),
                    Type = item2["AllowanceTypeID"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    RequestId = item2["RequestID"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    ReqStatus = item2["AllowanceStatusID"].ToString(),
                    RequestLoaclID = item2["RequestLoaclID"].ToString(),
                    AttachmentGuid = item2["AtchGuid"].ToString(),
                    CompanyName = item2["Name"].ToString()

                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public AllowanceRequestViewModel GetAllowanceRequestEditData(string Reqid)
        {
            AllowanceRequestViewModel UserDD = new AllowanceRequestViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);
            DataTable dt = _requestRepo.GetAllowanceRequestEditData(ht);
            UserDD.Amount = dt.Rows[0]["Amount"].ToString();
            UserDD.Type = dt.Rows[0]["AllowanceTypeID"].ToString();
            UserDD.Date = dt.Rows[0]["RequestDate"].ToString();
            UserDD.Remarks = dt.Rows[0]["Remarks"].ToString();
            return UserDD;
        }
        public string saveAllowanceRequestEdit(string Reqid, int saveStatus, string Amount,
           string Remarks, string Type, string Date)
        {
            string lblStatus = "";
            try
            {
                Hashtable ht = new Hashtable();

                ht.Add("@AllowanceRequestID", Reqid);
                ht.Add("@AllowanceTypeID", Type);
                ht.Add("@AllowanceAmount", Amount.Trim());
                ht.Add("@RequestDate", Date);
                ht.Add("@Remarks", Remarks);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@AtchGuid", "");
                ht.Add("@DBMessage", "");

                string DBMessage = _requestRepo.Update_Allowance_Request(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@AllowanceRequestID", Reqid);

                    DataTable dt = _requestRepo.Get_Allowance_Request_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = ""; //Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Reqid);
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        sBody = sBody.Replace("<%RedirectURL%>", ""/*Request.Url.AbsoluteUri*/);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    }
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public AllowanceRequestViewModel GetAllowanceRequestApproval(string Id)
        {
            AllowanceRequestViewModel model = new AllowanceRequestViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_Allowance_Request_ByID(ht);

            if (dt != null)
            {
                model.Employee = (string)dt.Rows[0]["UserFullName"];
                model.Type = (string)dt.Rows[0]["AllowanceType"];
                model.Amount = dt.Rows[0]["Amount"].ToString();
                model.CompanyName = (string)dt.Rows[0]["Name"];
                // lblReimbursementType.Text = (string)dt["ReimbursementType"];

                dt = null;
                dt = _requestRepo.Get_Allowance_Request_Remarks_List(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.History += "<div><strong><span>"
                                            + dr["UserFullName"].ToString().Trim() + "</span>"
                                            + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                            + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return model;
        }
        public string SaveAllowanceRequestApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@RequestID", Id);
                    htcheck.Add("@StatusID", StatusId);
                    //int finalApprovercheck = _requestRepo.Get_AllowanceRequest_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@AllowanceRequestID", Id);
                    ht.Add("@AllowanceStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);
                    string DBMessage = _requestRepo.Approve_Allowance_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@AllowanceRequestID", Id);
                    ht.Add("@Approver", UserId);

                    DataTable dt = _requestRepo.Get_Allowance_RequestSubmitter_Info_4_Email(ht);

                    var dt1 = DBContext.ExecuteReaderWithCommand("select MainApproverUserID, WorkflowID from dbo.Get_Next_Approver_Of_Allowance_Request(" + Id + ")");
                    int MainApproverUserID = 0;
                    while (dt1.Read())
                    {
                        MainApproverUserID = int.Parse(dt1["MainApproverUserID"].ToString());
                        //MainApproverUserID = int.Parse(dt1[0].ToString());
                    }
                    if ((null != dt) && dt.Rows.Count > 0 && MainApproverUserID != 0)
                    {

                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=AllowanceAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=AllowanceAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);

                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Allowance Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    else if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=AllowanceAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=AllowanceAppForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Allowance Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@AllowanceRequestID", Id);
                    ht.Add("@AllowanceStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");
                    Guid guid = Guid.NewGuid();
                    ht.Add("@RequestKey", guid);
                    string DBMessage = _requestRepo.Reject_Allowance_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@AllowanceRequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_Allowance_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;

                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Allowance Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }
            }
            return ErrorMsg;
        }

        #endregion

        #region AdvanceSalary
        public AdvanceSalaryListViewModel GetAdvanceSalaryData(string UserID, string CompanyId)
        {
            AdvanceSalaryListViewModel list = new AdvanceSalaryListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("AdvSalaryForm");

            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            DataTable dt = _requestRepo.Get_All_AdvanceSalary_Requests(ht);
            List<AdvanceSalaryViewModel> History = new List<AdvanceSalaryViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                AdvanceSalaryViewModel single = new AdvanceSalaryViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    PayPeriod = item["PayPeriod"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    LoanType = item["Loan_Code"].ToString(),
                    Amount = item["Amount"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    RequqestId = item["Id"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),

                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            List<AdvanceSalaryViewModel> Approval = new List<AdvanceSalaryViewModel>();
            DataTable dt2 = _requestRepo.Get_AdvanceSalary_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                AdvanceSalaryViewModel single2 = new AdvanceSalaryViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    PayPeriod = item2["PayPeriod"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    LoanType = item2["Loan_Code"].ToString(),
                    Amount = item2["Amount"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    ReqStatus = item2["AdvanceSalaryStatusID"].ToString(),
                    RequqestId = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString()
                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public string SaveAdvanceSalary(string CompanyId, string EmployeeID, string Remarks, string UserId, string Payperiod, string Amount)
        {
            string msg = "";
            bool val = validateForm(Payperiod);
            if (val)
            {
                msg = save(CompanyId, 2, EmployeeID, Remarks, UserId, Payperiod, Amount);
            }
            else
            {
                msg = "Sorry! some error has occurred. Please select the 1st Day of the month .";
            }
            return msg;
        }
        public Boolean validateForm(string Period)
        {

            if (Convert.ToDateTime(Period).Day == 1)
            {

                return true;
            }
            else
            {
                return false;
            }

        }
        private string save(string CompanyId, int saveStatus, string EmployeeID, string Remarks, string UserId, string Payperiod, string Amount)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("AdvSalaryForm");

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", EmployeeID);
                ht.Add("@Loan_Code", "AdvanceSalary");
                ht.Add("@Amount", Amount);
                ht.Add("@Remarks", Remarks);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@PayPeriod", Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", Payperiod)));
                ht.Add("@AdvanceSalaryRequestID", 0);
                ht.Add("@CompanyId", CompanyId);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_AdvanceSalary_Request"));
                int ReqID = _requestRepo.Insert_AdvanceSalary_Request(ht);

                if (ReqID > 0)
                {
                    lblStatus = "Your request has been saved successfully.";

                    //if (saveStatus == 2)
                    //{
                    //    ht = null;
                    //    ht = new Hashtable();
                    //    ht.Add("@AdvanceSalaryRequestID", ReqID);

                    //    DataTable dt = _requestRepo.Get_AdvanceSalary_Request_Info_4_Email(ht);

                    //    if ((null != dt) && dt.Rows.Count > 0)
                    //    {
                    //        string sBody = "";
                    //        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                    //        sBody = File.ReadAllText(htmlEmailFormat);
                    //        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                    //        sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                    //        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                    //        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                    //        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                    //        Uri uri = HttpContext.Current.Request.Url;
                    //        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}{3}", uri.Scheme,
                    //                    Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath) + "?FormType=1&SelTab=2");



                    //        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    //    }
                    //}
                }
                else
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public AdvanceSalaryViewModel GetAdvanceSalaryEditData(string Reqid)
        {
            AdvanceSalaryViewModel UserDD = new AdvanceSalaryViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);
            DataTable dt = _requestRepo.GetAdvanceSalaryEditData(ht);
            UserDD.EmployeeId = dt.Rows[0]["EmployeeUserID"].ToString();
            UserDD.Remarks = dt.Rows[0]["Remarks"].ToString();
            UserDD.Amount = dt.Rows[0]["Amount"].ToString();
            return UserDD;
        }
        public string SaveEditAdvanceSalary(string Reqid, string EmployeeID, string Remarks, string Amount, string UserId, int saveStatus)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();

                ht.Add("@AdvanceSalaryRequestID", Reqid);
                ht.Add("@EmployeeUserID", EmployeeID);
                ht.Add("@Loan_Code", "AdvanceSalary");
                ht.Add("@Amount", Amount);
                ht.Add("@Remarks", Remarks);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@DBMessage", "");

                string DBMessage = _requestRepo.Update_AdvanceSalary_Request(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@AdvanceSalaryRequestID", Reqid);

                    DataTable dt = _requestRepo.Get_AdvanceSalary_Request_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Reqid);
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        sBody = sBody.Replace("<%RedirectURL%>", ""/* Request.Url.AbsoluteUri*/);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    }
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public AdvanceSalaryViewModel GetAdvanceSalaryApproval(string Id)
        {
            AdvanceSalaryViewModel lvm = new AdvanceSalaryViewModel();
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_AdvanceSalary_Request_ByID(ht);

            if (dt != null)
            {
                lvm.Employee = (string)dt.Rows[0]["UserFullName"];
                lvm.Amount = string.Format("{0:#,##0.00}", dt.Rows[0]["Amount"]);
                lvm.CompanyName = (string)dt.Rows[0]["Name"];
                dt = null;
                dt = _requestRepo.Get_AdvanceSalary_Request_Remarks_List(ht);

                lvm.Remarks = "";
                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lvm.Remarks += "<div><strong><span>"
                                                    + dr["UserFullName"].ToString().Trim() + "</span>"
                                                    + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                    + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }

            return lvm;
        }
        public string SaveAdvanceSalaryApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();

                    htcheck.Add("@AdvanceSalaryRequestID", Id);
                    htcheck.Add("@AdvanceSalaryStatusID", StatusId);

                    int finalApprovercheck = _requestRepo.Get_Advances_RequestsApprovers_Count(htcheck);

                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}
                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{


                    Hashtable ht = new Hashtable();

                    ht.Add("@AdvanceSalaryRequestID", Id);
                    ht.Add("@AdvanceSalaryStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Approve_AdvanceSalary_Request(ht);

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                        //lblStatus.ForeColor = Color.Green;
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@AdvanceSalaryRequestID", Id);
                    ht.Add("@AdvanceSalaryStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Reject_AdvanceSalary_Request(ht);

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }

        #endregion

        #region LoanApplication
        public List<DropDownBindViewModel> GetLoanTypes()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetLoanTypes();
            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["LoanTypeDesc"].ToString(),
                    Value = item["Loan_Code"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public List<DropDownBindViewModel> GetInstallments(string LoanType)
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@LoanCode", LoanType);
            var dt = _requestRepo.GetInstallments(ht);
            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["Number"].ToString(),
                    Value = item["Number"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public LoanRequestListViewModel GetLoanData(string UserID, string CompanyId)
        {
            LoanRequestListViewModel list = new LoanRequestListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("LoanAppForm");

            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            DataTable dt = _requestRepo.Get_All_Loan_Requests(ht);
            List<LoanRequestViewModel> History = new List<LoanRequestViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                LoanRequestViewModel single = new LoanRequestViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    ExpectedDate = item["LoanExpectedDate"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    LoanType = item["LoanType"].ToString(),
                    Amount = item["Amount"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    RequestID = item["Id"].ToString(),
                    Installments = item["NoOfInstallments"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),

                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            List<LoanRequestViewModel> Approval = new List<LoanRequestViewModel>();
            DataTable dt2 = _requestRepo.Get_Loan_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                LoanRequestViewModel single2 = new LoanRequestViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    ExpectedDate = item2["LoanExpectedDate"].ToString(),
                    Installments = item2["NoOfInstallments"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    LoanType = item2["LoanType"].ToString(),
                    Amount = item2["Amount"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    ReqStatus = item2["LoanStatusID"].ToString(),
                    RequestID = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString()

                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public string SaveLoanApplication(string Emp, string Type, string Amount, string Installments, List<LoanSaveViewModel> RModle, string UserId, string CompanyId)
        {
            string ErrorMsg = "";
            int saveStatus = 2;
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("LoanAppForm");

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", Emp);
                ht.Add("@Loan_Code", Type);
                ht.Add("@Amount", Amount);
                ht.Add("@LoanExpectedDate", string.Format("{0:dd/MM/yyyy}", DateTime.Now));
                ht.Add("@NoOfInstallments", Installments);
                //  ht.Add("@Remarks", rTxtRemarks.Text);
                ht.Add("@Remarks", "");
                ht.Add("@SaveStatus", 2);
                ht.Add("@LoanRequestID", 0);
                ht.Add("@CompanyId", CompanyId);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Loan_Request"));

                int ReqID = _requestRepo.Insert_Loan_Request(ht);

                if (ReqID > 0)
                {
                    foreach (var item in RModle)
                    {
                        ht = null;
                        ht = new Hashtable();

                        ht.Add("@RequestID", ReqID);
                        ht.Add("@LoanDate", Convert.ToDateTime(item.Date));
                        ht.Add("@Amount", item.Amount);
                        ht.Add("@Remarks", item.Remarks);

                        _requestRepo.Insert_Loan_Request_Detail(ht);
                    }

                    ErrorMsg = "Your request has been saved successfully.";


                    if (saveStatus == 2)
                    {
                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@LoanRequestID", ReqID);

                        DataTable dt = _requestRepo.Get_Loan_Request_Info_4_Email(ht);

                        if ((null != dt) && dt.Rows.Count > 0)
                        {
                            string sBody = "";
                            string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                            sBody = sBody.Replace("<%RedirectURL%>", ""/* Request.Url.AbsoluteUri*/);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                        }
                    }
                }
                else
                {
                    ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return ErrorMsg;
        }
        public LoanSaveViewModel GetLoanEditDatabind(string Reqid, string UserId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);

            var dt = _requestRepo.Get_Loan_Request_ByID(ht);

            LoanSaveViewModel single = new LoanSaveViewModel();
            foreach (DataRow item in dt.Rows)
            {
                single = new LoanSaveViewModel()
                {
                    Date = item["RequestDate"].ToString(),
                    Amount = item["Amount"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    Employee = item["EmployeeUserID"].ToString(),
                    Type = item["LoanType"].ToString(),
                    Installment = item["NoOfInstallments"].ToString(),
                    Id = item["RequestID"].ToString(),
                };
            }
            return single;
        }
        public string EditSaveLoan(int saveStatus, string Id, string User, string Type, string Amount, string Installment, string Date, string Remarks, string UserId)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();
                string date = string.Format("{0:dd/MM/yyyy}", DateTime.Parse(Date).Date.ToString());
                ht.Add("@LoanRequestID", Id);
                ht.Add("@EmployeeUserID", User);
                ht.Add("@Loan_Code", Type);
                ht.Add("@Amount", Amount);
                ht.Add("@LoanExpectedDate", "" + DateTime.Parse(date).Day + "/" + DateTime.Parse(date).Month + "/" + DateTime.Parse(date).Year + "");
                ht.Add("@NoOfInstallments", Installment);
                ht.Add("@Remarks", Remarks);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@DBMessage", "");

                string DBMessage = _requestRepo.Update_Loan_Request(ht);

                if (saveStatus == 2)
                {
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public LoanRequestViewModel GetLoanApproval(string Id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_ApprovalLoan_Request_ByID(ht);
            LoanRequestViewModel single = new LoanRequestViewModel();
            if (dt != null)
            {
                single.Employee = (string)dt.Rows[0]["UserFullName"];
                single.LoanType = (string)dt.Rows[0]["LoanType"];
                single.ExpectedDate = string.Format("{0:d/M/yyyy}", dt.Rows[0]["LoanExpectedDate"]);
                single.Amount = string.Format("{0:#,##0.00}", dt.Rows[0]["Amount"]);
                single.Installments = string.Format("{0:###} installment(s).", dt.Rows[0]["NoOfInstallments"]);
                single.CompanyName = (string)dt.Rows[0]["Name"];

                dt = null;
                dt = _requestRepo.Get_Loan_Request_Remarks_List(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        single.History += "<div><strong><span>"
                                                    + dr["UserFullName"].ToString().Trim() + "</span>"
                                                    + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                    + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return single;
        }
        public string SaveLoanApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@RequestID", Id);
                    htcheck.Add("@StatusID", StatusId);
                    int finalApprovercheck = _requestRepo.Get_LoanRequest_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@LoanRequestID", Id);
                    ht.Add("@LoanStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Approve_Loan_Request(ht);

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@LoanRequestID", Id);
                    ht.Add("@LoanStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Reject_Loan_Request(ht);

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }

        #endregion

        #region TicketRequest
        public List<DropDownBindViewModel> GetTicketTypes()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetTicketType();

            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["ID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public string SaveTicket(string AtchGuid, string TicketBalance, string Emp, string Type, string Remarks, string FromCountry, string FromCity, string ToCountry, string ToCity, string TravelDate, string ReturnDate, string UserId, string CompanyId)
        {
            string lblStatus = "";
            int saveStatus = 2;
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("TicketReqForm");

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", Emp);
                ht.Add("@TicketTypeID", Type);
                ht.Add("@Remarks", Remarks);
                ht.Add("@SaveStatus", 2);
                ht.Add("@TicketRequestID", 0);
                ht.Add("@TicketBalance", Convert.ToDecimal("0"/*lblTicketBalance.Text*/));
                ht.Add("@TravelFrom", FromCity);
                ht.Add("@TravelTo", ToCity);
                ht.Add("@Dateoftravel", TravelDate);
                ht.Add("@Datetotravel", ReturnDate);
                ht.Add("@AtchGuid", AtchGuid);
                ht.Add("@CompanyId", CompanyId);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Ticket_Request"));

                int ReqID = _requestRepo.Insert_Ticket_Request(ht);

                if (ReqID > 0)
                {
                    lblStatus = "Your request has been saved successfully.";

                    //if (saveStatus == 2)
                    //{
                    //    ht = null;
                    //    ht = new Hashtable();
                    //    ht.Add("@TicketRequestID", ReqID);

                    //    DataTable dt = _requestRepo.Get_Ticket_Request_Info_4_Email(ht);

                    //    if ((null != dt) && dt.Rows.Count > 0)
                    //    {
                    //        string sBody = "";
                    //        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                    //        sBody = File.ReadAllText(htmlEmailFormat);
                    //        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                    //        sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                    //        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                    //        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                    //        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                    //        sBody = sBody.Replace("<%RedirectURL%>", ""/*Request.Url.AbsoluteUri*/);
                    //        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public TicketRequestListViewModel GetTicketData(string UserID, string CompanyId)
        {
            TicketRequestListViewModel list = new TicketRequestListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("TicketReqForm");

            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            DataTable dt = _requestRepo.Get_All_Ticket_Requests(ht);
            List<TicketRequestViewModel> History = new List<TicketRequestViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                TicketRequestViewModel single = new TicketRequestViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    TicketBalance = item["TicketBalance"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    TicketType = item["TicketType"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    AttachmentGuid = item["AtchGuid"].ToString(),
                    RequestID = item["Id"].ToString()

                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            List<TicketRequestViewModel> Approval = new List<TicketRequestViewModel>();
            DataTable dt2 = _requestRepo.Get_Ticket_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                TicketRequestViewModel single2 = new TicketRequestViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    TicketType = item2["TicketType"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    TicketBalance = item2["TicketBalance"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    ReqStatus = item2["TicketStatusID"].ToString(),
                    RequestID = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString()

                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public TicketRequestViewModel GetTicketEditDatabind(string Reqid, string UserId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);

            var dt = _requestRepo.Get_Ticket_Request_ByID(ht);

            TicketRequestViewModel single = new TicketRequestViewModel();
            foreach (DataRow item in dt.Rows)
            {
                string qry1 = string.Format("Select ID from [tbl_Locations] where Name ='{0}'", item["TravelFrom"].ToString());
                var fromcointry = DBContext.ExecuteReaderWithCommand(qry1);
                string qry2 = string.Format("Select ID from [tbl_Locations] where Name ='{0}'", item["TravelTo"].ToString());
                var tocountry = DBContext.ExecuteReaderWithCommand(qry2);
                DataTable dr = new DataTable();
                dr.Load(fromcointry);
                DataTable dr2 = new DataTable();
                dr2.Load(tocountry);
                single = new TicketRequestViewModel()
                {
                    EmployeeId = item["EmployeeUserID"].ToString(),
                    TicketType = item["TicketTypeID"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    TravelFrom = item["TravelFrom"].ToString(),
                    TravelTo = item["TravelTo"].ToString(),
                    DateTravel = item["Dateoftravel"].ToString(),
                    DateReturn = item["Datetotravel"].ToString(),
                    TravelFromCountry = "",//dr.Rows[0]["ID"].ToString(),
                    TravelToCountry = "",//dr2.Rows[0]["ID"].ToString(),
                };
            }
            return single;
        }
        public string EditSaveTicket(int saveStatus, string Id, string Emp, string Type, string FromCity, string ToCity, string TravelDate, string ReturnDate, string TicketBalance, string Remarks, string UserId)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();

                ht.Add("@TicketRequestID", Id);
                ht.Add("@EmployeeUserID", Emp);
                ht.Add("@TicketTypeID", Type);
                ht.Add("@Remarks", Remarks);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@DBMessage", "");
                ht.Add("@TravelFrom", FromCity);
                ht.Add("@TravelTo", ToCity);
                ht.Add("@Dateoftravel", TravelDate);
                ht.Add("@Datetotravel", ReturnDate);

                string DBMessage = _requestRepo.Update_Ticket_Request(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@TicketRequestID", Id);

                    DataTable dt = _requestRepo.Get_Ticket_Request_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id);
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        sBody = sBody.Replace("<%RedirectURL%>", ""/*Request.Url.AbsoluteUri*/);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    }
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public TicketRequestViewModel GetTicketApproval(string Id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_Ticket_Request_ByID(ht);
            TicketRequestViewModel single = new TicketRequestViewModel();
            if (dt != null)
            {
                single.Employee = (string)dt.Rows[0]["UserFullName"];
                single.TicketType = (string)dt.Rows[0]["TicketType"];
                single.CompanyName = (string)dt.Rows[0]["Name"];


                dt = null;
                dt = _requestRepo.Get_Ticket_Request_Remarks_List(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        single.History += "<div><strong><span>"
                                                    + dr["UserFullName"].ToString().Trim() + "</span>"
                                                    + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                    + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return single;
        }
        public string SaveTicketApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@RequestID", Id);
                    htcheck.Add("@StatusID", StatusId);
                    int finalApprovercheck = _requestRepo.Get_TicketRequest_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@TicketRequestID", Id);
                    ht.Add("@TicketStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Approve_Ticket_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_TicketRequest_RequestSubmitter_Info_4_Email(ht);

                    //if ((null != dt) && dt.Rows.Count > 0)
                    //{
                    //    string sBody = "";
                    //    string htmlEmailFormat = "";//Server.MapPath("~/EmailTemplates/NotifyRequestSubmitter.htm");

                    //    sBody = File.ReadAllText(htmlEmailFormat);
                    //    sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                    //    sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                    //    sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                    //    sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                    //    sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                    //    sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                    //    sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                    //    //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                    //    Uri uri = HttpContext.Current.Request.Url;
                    //    sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                    //                    Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1");

                    //    //sBody = sBody.Replace("<%RedirectURL%>", "http://" + HttpContext.Current.Request.Url.Host + "/User/Forms/LeaveApplication.aspx?FormType=1&SelTab=1");

                    //    clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    //}

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@TicketRequestID", Id);
                    ht.Add("@TicketStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Reject_Ticket_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_TicketRequest_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyRequestSubmitter.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Rejected");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1");

                        //sBody = sBody.Replace("<%RedirectURL%>", "http://" + HttpContext.Current.Request.Url.Host + "/User/Forms/LeaveApplication.aspx?FormType=1&SelTab=1");

                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }
        public List<TicketRequestViewModel> GetTicketDetails(string Id)
        {
            List<TicketRequestViewModel> lvmm = new List<TicketRequestViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@TicketRequestID", Id);
            DataTable dt = _requestRepo.Get_Ticket_Request_Statuses(ht);

            foreach (DataRow item in dt.Rows)
            {
                TicketRequestViewModel lvm = new TicketRequestViewModel();
                lvm.RequestDate = item["ProcessedDate"].ToString();
                lvm.ReqStatus = item["RequestStatus"].ToString();
                lvm.Description = item["RequestStatusDesc"].ToString();
                lvm.Approver = item["MainApproverFullName"].ToString();
                lvm.ApprovedBy = item["ApprovedByFullName"].ToString();
                lvm.Remarks = item["Remarks"].ToString();
                lvmm.Add(lvm);
            }
            return lvmm;
        }


        #endregion

        #region MiscellaneousApplication
        public List<DropDownBindViewModel> GetMiscellaneousTypes()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetMiscellaneousTypes();

            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["ID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public List<DropDownBindViewModel> GetPreferredLanguage()
        {
            List<DropDownBindViewModel> UserDD = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetPreferredLanguage();

            foreach (DataRow item in dt.Rows)
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["ID"].ToString(),
                };
                UserDD.Add(User);
            }
            return UserDD;
        }
        public MiscellaneousRequestListViewModel GetMiscellaneousData(string UserID, string CompanyId)
        {
            MiscellaneousRequestListViewModel list = new MiscellaneousRequestListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("MiscellaneousForm");

            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            DataTable dt = _requestRepo.Get_All_Miscellaneous_Requests(ht);
            List<MiscellaneousRequestViewModel> History = new List<MiscellaneousRequestViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                MiscellaneousRequestViewModel single = new MiscellaneousRequestViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    Employee = item["UserFullName"].ToString(),
                    Address = item["AddressedTo"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    MType = item["MiscellaneousType"].ToString(),
                    Language = item["PreferredLanguage"].ToString(),
                    RequestId = item["Id"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),
                    AttachmentGuid = item["AtchGuid"].ToString(),

                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            List<MiscellaneousRequestViewModel> Approval = new List<MiscellaneousRequestViewModel>();
            DataTable dt2 = _requestRepo.Get_Miscellaneous_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                MiscellaneousRequestViewModel single2 = new MiscellaneousRequestViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    Employee = item2["UserFullName"].ToString(),
                    Address = item2["AddressedTo"].ToString(),
                    EmployeeId = item2["EMPLOYID"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    MType = item2["MiscellaneousType"].ToString(),
                    Language = item2["PreferredLanguage"].ToString(),
                    ReqStatus = item2["MiscellaneousStatusID"].ToString(),
                    RequestId = item2["Id"].ToString(),
                    CompanyName = item2["Name"].ToString(),
                    AttachmentGuid = item2["AtchGuid"].ToString()

                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public string SaveMiscellaneous(string AtchGuid, string MiscellaneousType, string PreferredType, string Opening, string Salary, string Department, string Report, string Address, string Remarks, string UserId, string CompanyId)
        {
            string lblStatus = "";
            int saveStatus = 2;
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("MiscellaneousForm");

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", UserId);
                ht.Add("@MiscellaneousTypeID", MiscellaneousType);
                ht.Add("@PreferredLanguageID", PreferredType);
                ht.Add("@AddressedTo", Address.Trim());
                ht.Add("@Remarks", Remarks.Trim());
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@NumberOfOpenings", (string.IsNullOrEmpty(Opening.Trim()) ? "0" : Opening));
                ht.Add("@TargetedSalary", Convert.ToDecimal(string.IsNullOrEmpty(Salary.Trim()) ? "0.00" : Salary));
                ht.Add("@Department", Department.Trim());
                ht.Add("@ReportToPosition", Report.Trim());
                ht.Add("@AtchGuid", AtchGuid);
                ht.Add("@MiscellaneousRequestID", 0);
                ht.Add("@CompanyId", CompanyId);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Miscellaneous_Request"));
                int ReqID = _requestRepo.Insert_Miscellaneous_Request(ht);

                if (ReqID > 0)
                {
                    lblStatus = "Your request has been saved successfully.";

                    if (saveStatus == 2)
                    {
                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@MiscellaneousRequestID", ReqID);

                        DataTable dt = _requestRepo.Get_Miscellaneous_Request_Info_4_Email(ht);
                        if ((null != dt) && dt.Rows.Count > 0 && dt.Rows[0]["Email"] != null)
                        {
                            string sBody = "";
                            string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                            htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                            sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                            sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                            string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];

                            var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=MiscellaneousForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%ApproveLink%>", url);
                            var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
                  "&ReqID=" + ReqID + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
                  "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
                  "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=MiscellaneousForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                            sBody = sBody.Replace("<%RejectLink%>", url2);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public MiscellaneousRequestViewModel GetMiscellaneousEditData(string Reqid, string UserId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);

            var dt = _requestRepo.Get_Miscellaneous_Request_ByID(ht);

            MiscellaneousRequestViewModel single = new MiscellaneousRequestViewModel();
            foreach (DataRow item in dt.Rows)
            {
                single = new MiscellaneousRequestViewModel()
                {
                    MType = item["MiscellaneousTypeID"].ToString(),
                    PType = item["PreferredLanguageID"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    Address = item["AddressedTo"].ToString(),
                };
            }
            return single;
        }
        public string SaveEditMiscellaneous(int saveStatus, string Id, string MiscellaneousType, string PreferredType, string Address, string Remarks, string UserId)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();

                ht.Add("@MiscellaneousRequestID", Id);
                ht.Add("@MiscellaneousTypeID", MiscellaneousType);
                ht.Add("@PreferredLanguageID", PreferredType);
                ht.Add("@AddressedTo", Address.Trim());
                ht.Add("@Remarks", Remarks);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@AtchGuid", "");
                ht.Add("@DBMessage", "");

                string DBMessage = _requestRepo.Update_Miscellaneous_Request(ht);

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@MiscellaneousRequestID", Id);

                    //DataTable dt = _requestRepo.Get_Ticket_Request_Info_4_Email(ht);

                    //if ((null != dt) && dt.Rows.Count > 0)
                    //{
                    //    string sBody = "";
                    //    string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                    //    sBody = File.ReadAllText(htmlEmailFormat);
                    //    sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                    //    sBody = sBody.Replace("<%ID%>", Id);
                    //    sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                    //    sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                    //    sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                    //    sBody = sBody.Replace("<%RedirectURL%>", ""/*Request.Url.AbsoluteUri*/);
                    //    clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "A request is pending for your approval.");
                    //}
                }

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                    lblStatus = "Your request has been saved successfully.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public MiscellaneousRequestViewModel GetMiscellaneousApproval(string Id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_Miscellaneous_Request_ByID(ht);
            MiscellaneousRequestViewModel single = new MiscellaneousRequestViewModel();
            if (dt != null)
            {
                single.Employee = (string)dt.Rows[0]["UserFullName"];
                single.MType = (string)dt.Rows[0]["MiscellaneousType"];
                single.PType = (string)dt.Rows[0]["PreferredLanguage"];
                single.Address = (string)dt.Rows[0]["AddressedTo"];
                single.CompanyName = (string)dt.Rows[0]["Name"];

                dt = null;
                dt = _requestRepo.Get_Miscellaneous_Request_Remarks_List(ht);

                if ((null != dt) && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        single.History += "<div><strong><span>"
                                                    + dr["UserFullName"].ToString().Trim() + "</span>"
                                                    + " said (" + clsCommon.GetPostedFieldText(DateTime.Parse(dr["UpdateDate"].ToString())) + "):</strong><br/><em>"
                                                    + dr["Remarks"].ToString().Trim() + "</em></div>";
                    }
                }

            }
            return single;
        }
        public string SaveMiscellaneousApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@RequestID", Id);
                    htcheck.Add("@StatusID", StatusId);
                    int finalApprovercheck = _requestRepo.Get_MiscRequest_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@MiscellaneousRequestID", Id);
                    ht.Add("@MiscellaneousStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Approve_Miscellaneous_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_MiscellaneousRequest_RequestSubmitter_Info_4_Email(ht);
                    var dt1 = DBContext.ExecuteReaderWithCommand("select MainApproverUserID, WorkflowID from dbo.Get_Next_Approver_Of_Miscellaneous_Request(" + Id + ")");
                    int MainApproverUserID = 0;
                    while (dt1.Read())
                    {
                        MainApproverUserID = int.Parse(dt1["MainApproverUserID"].ToString());
                    }
                    if ((null != dt) && dt.Rows.Count > 0 && MainApproverUserID != 0)
                    {

                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyApproverEmail.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id.ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%StartDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["StartDate"]));
                        sBody = sBody.Replace("<%EndDate%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["EndDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        Uri uri = HttpContext.Current.Request.Url;
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=MiscellaneousForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=MiscellaneousForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    else if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyRequestSubmitter.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        string WebUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
                        var url = WebUrl + "/Account/EmailWFAction?btnApprove=1&type=Approve" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=MiscellaneousForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%ApproveLink%>", url);
                        var url2 = WebUrl + "/Account/EmailWFAction?btnApprove=0&type=Reject" +
              "&ReqID=" + Id + "&StatusId=" + dt.Rows[0]["RequestStatusId"].ToString() + "&UserId=" + UserId.ToString() +
              "&CompanyId=" + CompanyId.ToString() + "&username=" + dt.Rows[0]["MainApproverFullName"].ToString() +
              "&hdnGuid=" + dt.Rows[0]["RequestKey"].ToString() + "&formtype=MiscellaneousForm" + "&leavetype=" + dt.Rows[0]["LeaveType"].ToString();


                        sBody = sBody.Replace("<%RejectLink%>", url2);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@MiscellaneousRequestID", Id);
                    ht.Add("@MiscellaneousStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Reject_Miscellaneous_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_MiscellaneousRequest_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = @"~\EmailTemplates\NotifyRequestSubmitter.htm";
                        htmlEmailFormat = HttpContext.Current.Server.MapPath(htmlEmailFormat);
                        sBody = File.ReadAllText(htmlEmailFormat);

                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Rejected");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1&SelTab=1");
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Miscellaneous Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }
        public List<MiscellaneousRequestViewModel> GetMiscellaneousDetails(string Id)
        {
            List<MiscellaneousRequestViewModel> lvmm = new List<MiscellaneousRequestViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@MiscellaneousRequestID", Id);
            DataTable dt = _requestRepo.Get_Miscellaneous_Request_Statuses(ht);

            foreach (DataRow item in dt.Rows)
            {
                MiscellaneousRequestViewModel lvm = new MiscellaneousRequestViewModel();
                lvm.RequestDate = item["ProcessedDate"].ToString();
                lvm.ReqStatus = item["RequestStatus"].ToString();
                lvm.Description = item["RequestStatusDesc"].ToString();
                lvm.Approver = item["MainApproverFullName"].ToString();
                lvm.ApprovedBy = item["ApprovedByFullName"].ToString();
                lvm.Remarks = item["Remarks"].ToString();
                lvmm.Add(lvm);
            }
            return lvmm;
        }

        #endregion

        #region AssetManagementRequest

        public List<DropDownBindViewModel> GetAssetType()
        {
            List<DropDownBindViewModel> Types = new List<DropDownBindViewModel>();
            var dt = _requestRepo.GetAssetType();
            while (dt.Read())
            {
                DropDownBindViewModel User = new DropDownBindViewModel()
                {
                    Text = dt["Name"].ToString(),
                    Value = dt["ID"].ToString(),
                };
                Types.Add(User);
            }
            return Types;
        }
        public AssetListViewModel GetAssetData(string UserID, string CompanyId)
        {
            AssetListViewModel list = new AssetListViewModel();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("AssetManagement");

            ht.Add("@UserID", UserID);
            ht.Add("@FormTypeID", FormId);
            ht.Add("@CompanyId", CompanyId);

            DataTable dt = _requestRepo.Get_All_Asset_Requests(ht);
            List<AssetViewModel> History = new List<AssetViewModel>();

            foreach (DataRow item in dt.Rows)
            {
                AssetViewModel single = new AssetViewModel()
                {
                    RefNo = item["RequestID"].ToString(),
                    RequestDate = item["RequestDate"].ToString(),
                    SubmittedBy = item["UserFullName"].ToString(),
                    Remarks = item["Remarks"].ToString(),
                    RequestId = item["Id"].ToString(),
                    IsEditViisible = isEditVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isRecallVisible = isRecallVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isSubmitVisible = isSubmitVisible(item["Last_Status_ID"].ToString(), int.Parse(item["EmployeeUserID"].ToString())),
                    isInProcessVisible = isInProcessVisible(item["Last_Status_ID"].ToString()),
                    isCompletedVisible = isCompletedVisible(item["Last_Status_ID"].ToString()),

                };
                History.Add(single);
                list.History = History;
            }

            ht = new Hashtable();
            ht.Add("@ApproverUserID", UserID);
            ht.Add("@CompanyId", CompanyId);

            List<AssetViewModel> Approval = new List<AssetViewModel>();
            DataTable dt2 = _requestRepo.Get_Asset_Requests_For_Approval(ht);
            if ((null != dt2) && dt2.Rows.Count > 0)
                list.IsApproval = true;
            else
            {
                list.IsApproval = false;

            }
            foreach (DataRow item2 in dt2.Rows)
            {
                AssetViewModel single2 = new AssetViewModel()
                {
                    RefNo = item2["RequestID"].ToString(),
                    RequestDate = item2["RequestDate"].ToString(),
                    SubmittedBy = item2["UserFullName"].ToString(),
                    Remarks = item2["Remarks"].ToString(),
                    ReqStatus = item2["AssetStatusID"].ToString(),
                    CompanyName = item2["Name"].ToString(),
                    RequestId = item2["Id"].ToString(),
                };
                Approval.Add(single2);
            }
            list.Approval = Approval;
            return list;
        }
        public List<AssetViewModel> GetAssetDetails(string Id)
        {
            List<AssetViewModel> lvmm = new List<AssetViewModel>();
            Hashtable ht = new Hashtable();
            ht.Add("@AssetRequestID", Id);
            DataTable dt = _requestRepo.Get_Asset_Request_Statuses(ht);

            foreach (DataRow item in dt.Rows)
            {
                AssetViewModel lvm = new AssetViewModel();
                lvm.RequestDate = item["ProcessedDate"].ToString();
                lvm.ReqStatus = item["RequestStatus"].ToString();
                lvm.Description = item["RequestStatusDesc"].ToString();
                lvm.Approver = item["MainApproverFullName"].ToString();
                lvm.ApprovedBy = item["ApprovedByFullName"].ToString();
                lvm.Remarks = item["Remarks"].ToString();
                lvmm.Add(lvm);
            }
            return lvmm;
        }
        public string SaveAssetRequest(string EmpFor, string RequestDate, List<AssetViewModel> List, string Remarks, string UserId, string CompanyId)
        {
            string lblStatus = "";
            int saveStatus = 2;
            try
            {
                Hashtable ht = new Hashtable();
                int FormId = GetFormTypeID("AssetManagement");

                string userstr = UserId;

                ht.Add("@FormTypeID", FormId);
                ht.Add("@EmployeeUserID", UserId);
                ht.Add("@ForEmployeeUserID", EmpFor);
                ht.Add("@RequestDate", RequestDate);
                ht.Add("@Remarks", Remarks);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@AtchGuid", "");
                ht.Add("@AssetRequestID", 0);
                ht.Add("@CompanyId", CompanyId);
                ht.Add("@RequestID", GenerateID(CompanyId, "tbl_Asset_Request"));
                int ReqID = _requestRepo.Insert_Asset_Request(ht);

                if (ReqID > 0)
                {
                    foreach (var dr in List)
                    {
                        ht = null;
                        ht = new Hashtable();

                        ht.Add("@AssetRequestID", ReqID);
                        ht.Add("@AssetTypeID", dr.AssetType);
                        ht.Add("@Description", dr.Remarks);
                        ht.Add("@ID", 0);

                        _requestRepo.Insert_AssetRequest_Detail(ht);
                    }
                    lblStatus = "Your request has been saved successfully.";

                    if (saveStatus == 2)
                    {
                        ht = null;
                        ht = new Hashtable();
                        ht.Add("@AssetRequestID", ReqID); //sp_User_Submit_Asset_Request
                        _requestRepo.Submit_Asset_Request(ht);

                        //#region Send Email to Approver
                        DataTable dt = _requestRepo.Get_Asset_Request_Info_4_Email(ht);

                        if ((null != dt) && dt.Rows.Count > 0 && dt.Rows[0]["Email"] != null)
                        {
                            string sBody = "";
                            string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                            sBody = File.ReadAllText(htmlEmailFormat);
                            sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                            sBody = sBody.Replace("<%ID%>", ReqID.ToString());
                            sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                            sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                            sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                            sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                            sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                            //User/Forms/LeaveApplication.aspx?FormType=1&SelTab=2
                            Uri uri = HttpContext.Current.Request.Url;
                            sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}{3}", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath) + "?FormType=1&SelTab=2");
                            //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                            clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                        }
                        //#endregion

                    }
                }
                else
                {
                    lblStatus = "Sorry! some error has occurred. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public AssetEditViewModel GetAssetEditDatabind(string Reqid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Reqid);

            var dt = _requestRepo.Get_Asset_Request_ByID(ht);

            AssetEditViewModel list = new AssetEditViewModel()
            {
                EmpFor = dt.Rows[0]["EmployeeUserID"].ToString(),
                EmpBy = dt.Rows[0]["UserFullName"].ToString(),
                RequestDate = dt.Rows[0]["RequestDate"].ToString(),
                Remarks = dt.Rows[0]["Remarks"].ToString(),
            };
            list.dropdowns = new List<DropDownBindViewModel>();
            list.dropdowns = GetAssetType();
            list.Selected = new List<AssetEditList>();
            ht = new Hashtable();
            ht.Add("@RequestID", Reqid);

            DataTable dt2 = _requestRepo.Get_Asset_Detail_By_AssetID(ht);
            foreach (DataRow item in dt2.Rows)
            {
                AssetEditList single = new AssetEditList()
                {
                    Description = item["Description"].ToString(),
                    Type = item["AssetTypeID"].ToString()
                };
                list.Selected.Add(single);
            }
            return list;
        }
        public string SaveEditAsset(int saveStatus, string Id, string EmpFor, string RequestDate, List<AssetViewModel> List, string Remarks, string UserId)
        {
            string lblStatus = "";

            try
            {
                Hashtable ht = new Hashtable();

                string formstr = "1025";
                string userstr = UserId;

                ht.Add("@AssetRequestID", Id);
                ht.Add("@RequestDate", RequestDate);
                ht.Add("@Remarks", Remarks);
                ht.Add("@ForEmployeeUserID", EmpFor);
                ht.Add("@isRecalled", true);
                ht.Add("@SaveStatus", saveStatus);
                ht.Add("@AtchGuid", "");
                ht.Add("@DBMessage", "");

                string DBMessage = _requestRepo.Update_Asset_Request(ht);
                _requestRepo.DeleteAssetDetails(Id);

                foreach (var dr in List)
                {
                    ht = null;
                    ht = new Hashtable();

                    ht.Add("@AssetRequestID", Id);
                    ht.Add("@AssetTypeID", dr.AssetType);
                    ht.Add("@Description", dr.Remarks);
                    ht.Add("@ID", 0);

                    _requestRepo.Insert_AssetRequest_Detail(ht);
                }
                lblStatus = "Your request has been saved successfully.";

                if (saveStatus == 2)
                {
                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@AssetRequestID", Id); //sp_User_Submit_Asset_Request
                    _requestRepo.Submit_Asset_Request(ht);

                    //#region Send Email to Approver
                    DataTable dt = _requestRepo.Get_Asset_Request_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0 && dt.Rows[0]["Email"] != null)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyApproverEmail.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%ID%>", Id);
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());

                        sBody = sBody.Replace("<%EmployeeID%>", dt.Rows[0]["RequesterEmpID"].ToString());
                        sBody = sBody.Replace("<%EmployeeName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        //User/Forms/LeaveApplication.aspx?FormType=1&SelTab=2
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}{3}", uri.Scheme,
                                    Uri.SchemeDelimiter, uri.Authority, uri.AbsolutePath) + "?FormType=1&SelTab=2");
                        //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"].ToString(), "A request is pending for your approval.");
                    }
                    //#endregion

                }
            }
            catch (Exception ex)
            {
                lblStatus = ex.Message;
            }
            return lblStatus;
        }
        public AssetViewModel GetAssetApproval(string Id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@RequestID", Id);
            DataTable dt = _requestRepo.Get_Asset_Request_ByID(ht);
            AssetViewModel single = new AssetViewModel();
            if (dt != null)
            {
                single.SubmittedBy = (string)dt.Rows[0]["UserFullName"];
                single.EmpFor = (string)dt.Rows[0]["ForUserFullName"];
                single.RequestDate = dt.Rows[0]["RequestDate"].ToString();
                single.CompanyName = (string)dt.Rows[0]["Name"];
                single.Remarks = (string)dt.Rows[0]["Remarks"];

            }
            return single;
        }
        public string SaveAssetApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId)
        {
            string ErrorMsg = "";
            if (Type == "Approve")
            {
                try
                {
                    Hashtable htcheck = new Hashtable();
                    htcheck.Add("@leaveID", Id);
                    htcheck.Add("@StatusID", StatusId);
                    //int finalApprovercheck = _requestRepo.Get_Assets_RequestsApprovers_Count(htcheck);
                    //Resources d365 = ODATAConnection();
                    //if (d365 != null && finalApprovercheck == 1)
                    //{
                    //    postCheck = InsertRecord(d365, Convert.ToInt32(Request.QueryString["RequestID"]));
                    //}

                    //if (/*postCheck ||*/ finalApprovercheck == 0)
                    //{
                    Hashtable ht = new Hashtable();

                    ht.Add("@AssetRequestID", Id);
                    ht.Add("@AssetStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@CompanyId", CompanyId);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Approve_Asset_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_Asset_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";//Server.MapPath("~/EmailTemplates/NotifyRequestSubmitter.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Approved");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1");

                        //sBody = sBody.Replace("<%RedirectURL%>", "http://" + HttpContext.Current.Request.Url.Host + "/User/Forms/LeaveApplication.aspx?FormType=1&SelTab=1");

                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }

                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;

                }

            }
            else if (Type == "Reject")
            {
                try
                {
                    Hashtable ht = new Hashtable();

                    ht.Add("@AssetRequestID", Id);
                    ht.Add("@AssetStatusID", StatusId);
                    ht.Add("@ApproverUserID", UserId);
                    ht.Add("@Remarks", Remarks);
                    ht.Add("@DBMessage", "");

                    string DBMessage = _requestRepo.Reject_Asset_Request(ht);

                    ht = null;
                    ht = new Hashtable();
                    ht.Add("@RequestID", Id);
                    ht.Add("@Approver", UserId);
                    DataTable dt = _requestRepo.Get_Asset_RequestSubmitter_Info_4_Email(ht);

                    if ((null != dt) && dt.Rows.Count > 0)
                    {
                        string sBody = "";
                        string htmlEmailFormat = "";// Server.MapPath("~/EmailTemplates/NotifyRequestSubmitter.htm");

                        sBody = File.ReadAllText(htmlEmailFormat);
                        sBody = sBody.Replace("<%UserFullName%>", dt.Rows[0]["MainApproverFullName"].ToString());
                        sBody = sBody.Replace("<%RequesterFullName%>", dt.Rows[0]["RequesterFullName"].ToString());
                        sBody = sBody.Replace("<%ApprovalStatus%>", "Rejected");
                        sBody = sBody.Replace("<%ID%>", dt.Rows[0]["ID"].ToString());
                        sBody = sBody.Replace("<%Date%>", string.Format("{0:dd/MM/yyyy}", dt.Rows[0]["RequestDate"]));
                        sBody = sBody.Replace("<%Type%>", dt.Rows[0]["RequestType"].ToString());
                        sBody = sBody.Replace("<%Remarks%>", dt.Rows[0]["Remarks"].ToString());
                        //sBody = sBody.Replace("<%RedirectURL%>", Request.Url.AbsoluteUri);
                        Uri uri = HttpContext.Current.Request.Url;
                        sBody = sBody.Replace("<%RedirectURL%>", String.Format("{0}{1}{2}/User/Forms/LeaveApplication.aspx", uri.Scheme,
                                        Uri.SchemeDelimiter, uri.Authority) + "?FormType=1&SelTab=1");

                        //sBody = sBody.Replace("<%RedirectURL%>", "http://" + HttpContext.Current.Request.Url.Host + "/User/Forms/LeaveApplication.aspx?FormType=1&SelTab=1");

                        clsCommon.SendMail(sBody, dt.Rows[0]["Email"].ToString(), ConfigurationManager.AppSettings["EMAIL_ACC"], "Leave Request " + dt.Rows[0]["ApprovalStatus"].ToString() + " By " + dt.Rows[0]["MainApproverFullName"]);
                    }
                    if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                    {
                        ErrorMsg = "Sorry! some error has occurred. Please try again later.";
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "mykey", "CloseAndRebind();", true);
                        ErrorMsg = "Processed successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg = ex.Message;
                }

            }
            return ErrorMsg;
        }


        #endregion

        #region EmployeeAttendanceDeatils
        public List<EmployeeAttendanceViewModel> GetEmployeeAttendance(string StartDate, string EndDate,string Depid, string Emp, string CompanyId, string UserId)
        {
            bool isHR = false;
            List<EmployeeAttendanceViewModel> lvmm = new List<EmployeeAttendanceViewModel>();
            string qry = string.Format("select Name from tbl_User_UserRole_Mapping inner join tbl_UserRole on tbl_UserRole.UserRoleID=tbl_User_UserRole_Mapping.UserRoleID where  ([Name]='System Administrators' or [Name]='HR Appraisals') and UserID={0}", UserId);
            var DS = _requestRepo.GetISHR(qry);

            if (DS.HasRows)
            {
                isHR = true;
            }

            DataTable dt = null;
            string selectedEmployeeValue = string.Empty;

            Hashtable ht = new Hashtable();
            ht.Add("@isIHR", isHR);
            ht.Add("@start", StartDate);
            ht.Add("@end", EndDate);
            //ht.Add("@UserId", Convert.ToInt32(Convert.ToInt32(Session["_UserID"])));
            selectedEmployeeValue = Emp;
            ht.Add("@UserId", string.IsNullOrEmpty(Emp) ? "" : Emp);
            ht.Add("@CompanyId", CompanyId);


            dt = _requestRepo.EmployeeAttendDetails(ht);

            foreach (DataRow item in dt.Rows)
            {
                EmployeeAttendanceViewModel emp = new EmployeeAttendanceViewModel()
                {
                    UserId = item["UserId"].ToString(),
                    CheckIn = item["CheckIn"].ToString(),
                    CheckOut = item["CheckOut"].ToString(),
                    Date = item["Date"].ToString(),
                    Department = item["Department"].ToString(),
                    DESIGNATION=item["DESIGNATION"].ToString(),
                    EmployeeId = item["EMPLOYID"].ToString(),
                    Name = item["Name"].ToString(),
                    WorkingHours = item["WorkingTime"].ToString(),
                };
            }



            return lvmm;
        }

        #endregion

        #region StaffExpenseForm
        public List<StaffExpenseViewModel> GetStaffExpenseList(string UserId, string CompanyId)
        {
            List<StaffExpenseViewModel> UserDD = new List<StaffExpenseViewModel>();
            Hashtable ht = new Hashtable();
            int FormId = GetFormTypeID("Staff Expense Form");

            ht.Add("@UserID", UserId);
            ht.Add("@FormTypeID", FormId);
            var dt = _requestRepo.GetStaffExpenseList(ht);

            return UserDD;
        }

        #endregion
    }
    public class LeavesModel
    {
        public string NoOfDays { get; set; }
        public string TotalLeaves { get; set; }
        public string Balancebefore { get; set; }
        public string Balanceafter { get; set; }
    }
}