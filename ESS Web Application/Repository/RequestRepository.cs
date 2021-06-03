using ESS_Web_Application.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public class RequestRepository : IRequestRepository
    {
        public int GetFromTypeID(string qry)
        {
            int id = 0;
            var a = DBContext.ExecuteReaderWithCommand(qry);
            while (a.Read())
            {
                id = int.Parse(a[0].ToString());
            }
            return id;
        }
        public int InsertUpdateEmployeeImage(Hashtable ht)
        {
            return Convert.ToInt32(DBContext.ExecuteScalar("sp_InsertUpdateEmployeeImage", ht));
        }
        #region LeaveApplication
        public DataTable GetLeaveAppUsers(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Employees_By_UserID_4_DDL", ht).Tables[0];
        }
        public DataTable GetEmployeeDeatils(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Profile_Info_4_Requests", ht).Tables[0];
        }
        public SqlDataReader GetLeaveTypes()
        {
            return DBContext.ExecuteReaderWithCommand("Select * from tbl_LeaveType");
        }
        public DataTable GetReplacementEmployee(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_Admin_Get_Replacment_Employees_4_DDL", ht).Tables[0];
        }
        public long GetWorkerIDByUserID(Hashtable ht)
        {
            return Convert.ToInt64(DBContext.ExecuteScalar("sp_User_Get_WorkerID_By_UserID", ht));
        }
        public string GetUserApprovedLeaves(Hashtable ht)
        {
            return Convert.ToString(DBContext.ExecuteScalar("sp_User_GetUserApprovedLeaves", ht));
        }
        public decimal GetUserLeaveBalance(Hashtable ht)
        {
            return Convert.ToDecimal(DBContext.ExecuteScalar("sp_User_GetUserLeaveBalance", ht));
        }
        public int CheckLeaveApplicationValidations(Hashtable ht)
        {
            return Convert.ToInt32(DBContext.ExecuteScalar("sp_User_CheckLeaveApplicationValidations", ht));
        }
        public int CheckforDuplicateLeaves(Hashtable ht)
        {
            return Convert.ToInt32(DBContext.ExecuteScalar("sp_User_CheckforDuplicateLeaves", ht));
        }
        public int spUserInsertLeaveRequest(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Leave_Request", ht, "@LeaveRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public DataTable GetCountries()
        {
            return DBContext.GetDataSet("SP_Locations", null).Tables[0];
        }
        public DataTable GetCities(Hashtable ht)
        {
            return DBContext.GetDataSet("SP_Locations", ht).Tables[0];
        }
        public DataTable GetLeaveApplicationApprovalData(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_Requests_For_Approval", ht).Tables[0];
        }
        public DataTable GetLeaveDetails(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_Request_Statuses", ht).Tables[0];
        }
        public DataTable GetAllLeaveRequests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Leave_Requests", ht).Tables[0];
        }
        public DataTable GetLeaveRequestByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_Request_ByID", ht).Tables[0];
        }
        public string UpdateLeaveRequest(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Leave_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable GetLeaveRequestInfo4Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable GetLeaveRequestReplacementInfo4Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_RequestReplacement_Info_4_Email", ht).Tables[0];
        }
        public DataTable GetLeaveRequestRemarksList(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_Request_Remarks_List", ht).Tables[0];
        }
        public string ApproveLeaveRequest(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Leave_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable GetLeaveRequestSubmitterInfo4Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Leave_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string RejectLeaveRequest(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Leave_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public string User_Resume_Leave_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Resume_Leave_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;

        }
        public DataTable User_Get_ResumeLeave_Trx_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_ResumeLeave_Trx_ByID", ht).Tables[0];

        }
        #endregion

        #region Reimbursement

        public SqlDataReader GetReimbursementType()
        {
            return DBContext.ExecuteReaderWithCommand("select ID,Name from tbl_ReimbursementType where IsActive=1");
        }
        public SqlDataReader GetCurrencyCode()
        {
            return DBContext.ExecuteReaderWithCommand("Select CurrencyCode from tbl_Currency");
        }
        public SqlDataReader GetActivityTypes()
        {
            return DBContext.ExecuteReaderWithCommand("Select ID,Name from tbl_ActivityTypes");
        }
        public int InsertReimbursementRequest(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Reimbursement_Request", ht, "@ReimbursementRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public void InsertReimbursmentRequestDetail(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_User_Insert_Reimbursment_Request_Detail", ht);
        }
        public DataTable GetReimbursementRequestInfo4Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Reimbursement_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable GetReimbursementList(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Reimbursement_Requests", ht).Tables[0];
        }
        public DataTable GetReimbursementListForApproval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Reimbursement_Requests_For_Approval", ht).Tables[0];
        }
        public DataTable GetReimbursementRequestDetail(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Reimbursement_Request_Detail", ht).Tables[0];
        }
        public string EditSaveReimbursementRequest(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Reimbursement_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable GetReimbursementRequestByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Reimbursement_Request_ByID", ht).Tables[0];
        }
        public DataTable GetReimbursementRequestRemarksList(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Reimbursement_Request_Remarks_List", ht).Tables[0];
        }
        public int ApproveReimbursement(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_ReimbursementRequest_RequestsApprovers_Count", ht, "@StatusID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string ApproveReimbursementRequest(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Reimbursement_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable GetReimbursementRequestSubmitterInfo4Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Reimbursement_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string RejectReimbursementRequest(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Reimbursement_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }

        #endregion

        #region LasteandAbsenceJustification
        public DataTable Get_All_LateandAbsence_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_LateandAbsence_Requests", ht).Tables[0];
        }
        public DataTable Get_LateandAbsence_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_LateandAbsence_Requests_For_Approval", ht).Tables[0];
        }
        public int Insert_LateandAbsence_Request(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_LateandAbsence_Request", ht, "@RequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public DataTable Get_LateandAbsence_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_LateandAbsence_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable Get_LateandAbsence_Request_Statuses(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_LateandAbsence_Request_Statuses", ht).Tables[0];
        }
        public DataTable Get_LateandAbsence_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_LateandAbsence_Request_ByID", ht).Tables[0];
        }
        public string EditInsert_LateandAbsence_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_LateandAbsence_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_LateandAbsence_Request_Remarks_List(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_LateandAbsence_Request_Remarks_List", ht).Tables[0];
        }
        public int Get_LateAndAbsence_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_LateAndAbsence_RequestsApprovers_Count", ht, "@Status", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_LateandAbsence_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_LateandAbsence_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string; ;
        }
        public DataTable Get_LateandAbsence_RequestSubmitter_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_LateandAbsence_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string Reject_LateandAbsence_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_LateandAbsence_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }

        #endregion

        #region AllownceRequest
        public DataTable GetAllownceTypes()
        {
            return DBContext.GetDataSet("sp_User_Get_AllowanceTypes", null).Tables[0];
        }
        public int SaveAllowanceRequest(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Allowance_Request", ht, "@AllowanceRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public DataTable Get_Allowance_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Allowance_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable Get_All_Allowance_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Allowance_Requests", ht).Tables[0];
        }
        public DataTable Get_Allowance_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Allowance_Requests_For_Approval", ht).Tables[0];
        }
        public DataTable GetAllowanceRequestEditData(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Allowance_Request_ByID", ht).Tables[0];
        }
        public string Update_Allowance_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Allowance_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_Allowance_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Allowance_Request_ByID", ht).Tables[0];
        }
        public DataTable Get_Allowance_Request_Remarks_List(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Allowance_Request_Remarks_List", ht).Tables[0];
        }
        public int Get_AllowanceRequest_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_AllowanceRequest_RequestsApprovers_Count", ht, "@Status", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_Allowance_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Allowance_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string; ;
        }
        public DataTable Get_Allowance_RequestSubmitter_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Allowance_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string Reject_Allowance_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Allowance_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        #endregion

        #region AdvanceSalary
        public DataTable Get_AdvanceSalary_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_AdvanceSalary_Requests_For_Approval", ht).Tables[0];
        }
        public DataTable Get_All_AdvanceSalary_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_AdvanceSalary_Requests", ht).Tables[0];
        }
        public int Insert_AdvanceSalary_Request(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_AdvanceSalary_Request", ht, "@AdvanceSalaryRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public DataTable Get_AdvanceSalary_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_AdvanceSalary_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable GetAdvanceSalaryEditData(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_AdvanceSalary_Request_ByID", ht).Tables[0];
        }
        public string Update_AdvanceSalary_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_AdvanceSalary_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_AdvanceSalary_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_AdvanceSalary_Request_ByID", ht).Tables[0];
        }
        public DataTable Get_AdvanceSalary_Request_Remarks_List(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_AdvanceSalary_Request_Remarks_List", ht).Tables[0];
        }
        public int Get_Advances_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_Advances_RequestsApprovers_Count", ht, "@AdvanceSalaryStatusID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_AdvanceSalary_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_AdvanceSalary_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public string Reject_AdvanceSalary_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_AdvanceSalary_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        #endregion

        #region LoanApplication
        public DataTable GetLoanTypes()
        {
            return DBContext.GetDataSet("sp_User_Get_LoanTypes_4_DDL", null).Tables[0];
        }
        public DataTable GetInstallments(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_NoOfLoanInstallmentList_4_DDL", ht).Tables[0];
        }
        public DataTable Get_All_Loan_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Loan_Requests", ht).Tables[0];
        }
        public DataTable Get_Loan_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Loan_Requests_For_Approval", ht).Tables[0];
        }
        public int Insert_Loan_Request(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Loan_Request", ht, "@LoanRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public void Insert_Loan_Request_Detail(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_User_Insert_Loan_Request_Detail", ht);
        }
        public DataTable Get_Loan_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Loan_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable Get_Loan_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Loan_Request_ByID", ht).Tables[0];
        }
        public string Update_Loan_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Loan_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_ApprovalLoan_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Loan_Request_ByID", ht).Tables[0];
        }
        public DataTable Get_Loan_Request_Remarks_List(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Loan_Request_Remarks_List", ht).Tables[0];
        }
        public int Get_LoanRequest_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_LoanRequest_RequestsApprovers_Count", ht, "@StatusID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_Loan_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Loan_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public string Reject_Loan_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Loan_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }

        #endregion

        #region TicketRequest
        public DataTable GetTicketType()
        {
            return DBContext.GetDataSet("sp_User_Get_TicketTypes_4_DDL", null).Tables[0];
        }
        public int Insert_Ticket_Request(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Ticket_Request", ht, "@TicketRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public DataTable Get_Ticket_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Ticket_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable Get_All_Ticket_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Ticket_Requests", ht).Tables[0];
        }
        public DataTable Get_Ticket_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Ticket_Requests_For_Approval", ht).Tables[0];
        }
        public DataTable Get_Ticket_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Ticket_Request_ByID", ht).Tables[0];
        }
        public string Update_Ticket_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Ticket_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_Ticket_Request_Remarks_List(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Ticket_Request_Remarks_List", ht).Tables[0];
        }
        public int Get_TicketRequest_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_TicketRequest_RequestsApprovers_Count", ht, "@StatusID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_Ticket_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Ticket_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_TicketRequest_RequestSubmitter_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_TicketRequest_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string Reject_Ticket_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Ticket_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_Ticket_Request_Statuses(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Ticket_Request_Statuses", ht).Tables[0];
        }
        #endregion

        #region MiscellaneousApplication

        public DataTable GetMiscellaneousTypes()
        {
            return DBContext.GetDataSet("sp_User_Get_MiscellaneousTypes_4_DDL", null).Tables[0];
        }
        public DataTable GetPreferredLanguage()
        {
            return DBContext.GetDataSet("sp_User_Get_PreferredLanguages_4_DDL", null).Tables[0];
        }
        public DataTable Get_All_Miscellaneous_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Miscellaneous_Requests", ht).Tables[0];
        }
        public DataTable Get_Miscellaneous_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Miscellaneous_Requests_For_Approval", ht).Tables[0];
        }
        public int Insert_Miscellaneous_Request(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Miscellaneous_Request", ht, "@MiscellaneousRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public DataTable Get_Miscellaneous_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Miscellaneous_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable Get_Miscellaneous_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Miscellaneous_Request_ByID", ht).Tables[0];
        }
        public string Update_Miscellaneous_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Miscellaneous_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_Miscellaneous_Request_Remarks_List(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Miscellaneous_Request_Remarks_List", ht).Tables[0];
        }
        public int Get_MiscRequest_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_MiscRequest_RequestsApprovers_Count", ht, "@StatusID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_Miscellaneous_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Miscellaneous_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_MiscellaneousRequest_RequestSubmitter_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_MiscellaneousRequest_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string Reject_Miscellaneous_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Miscellaneous_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_Miscellaneous_Request_Statuses(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Miscellaneous_Request_Statuses", ht).Tables[0];
        }

        #endregion

        #region AssetManagementRequest
        public SqlDataReader GetAssetType()
        {
            return DBContext.ExecuteReaderWithCommand("select ID, Name from tbl_AssetType where IsActive = 1");
        }
        public DataTable Get_All_Asset_Requests(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_Asset_Requests", ht).Tables[0];
        }
        public DataTable Get_Asset_Requests_For_Approval(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Asset_Requests_For_Approval", ht).Tables[0];
        }
        public DataTable Get_Asset_Request_Statuses(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Asset_Request_Statuses", ht).Tables[0];
        }
        public int Insert_Asset_Request(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Insert_Asset_Request", ht, "@AssetRequestID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public void Insert_AssetRequest_Detail(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_User_Insert_AssetRequest_Detail", ht);
        }
        public void Submit_Asset_Request(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_User_Submit_Asset_Request", ht);
        }
        public DataTable Get_Asset_Request_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Asset_Request_Info_4_Email", ht).Tables[0];
        }
        public DataTable Get_Asset_Request_ByID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Asset_Request_ByID", ht).Tables[0];
        }
        public DataTable Get_Asset_Detail_By_AssetID(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Asset_Detail_By_AssetID", ht).Tables[0];
        }
        public string Update_Asset_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Update_Asset_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public void DeleteAssetDetails(string Id)
        {
            DBContext.ExecuteNonQuery("delete from dbo.tbl_AssetRequest_Detail where REQ_ID=" + Id + ";");
        }
        public int Get_Assets_RequestsApprovers_Count(Hashtable ht)
        {
            return int.Parse(DBContext.ExecuteNonQuery("sp_User_Get_Assets_RequestsApprovers_Count", ht, "@StatusID", System.Data.SqlDbType.Int, 0).ToString());
        }
        public string Approve_Asset_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Approve_Asset_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public DataTable Get_Asset_RequestSubmitter_Info_4_Email(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_Asset_RequestSubmitter_Info_4_Email", ht).Tables[0];
        }
        public string Reject_Asset_Request(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_User_Reject_Asset_Request", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        #endregion

        #region EmployeeAttendanceDeatils

        public SqlDataReader GetISHR(string qry)
        {
            return DBContext.ExecuteReaderWithCommand(qry);
        }
        public DataTable EmployeeAttendDetails(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_EmployeeAttendDetails", ht).Tables[0];
        }

        #endregion

        #region StaffExpense
        public DataTable GetStaffExpenseList(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_User_Get_All_StaffExpense_Requests", ht).Tables[0];
        }

        #endregion
    }
}