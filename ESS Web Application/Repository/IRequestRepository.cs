using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public interface IRequestRepository
    {
        int GetFromTypeID(string qry);

        DataTable GetLeaveAppUsers(Hashtable ht);
        DataTable GetEmployeeDeatils(Hashtable ht);
        SqlDataReader GetLeaveTypes();
        DataTable GetReplacementEmployee(Hashtable ht);
        long GetWorkerIDByUserID(Hashtable ht);
        string GetUserApprovedLeaves(Hashtable ht);
        decimal GetUserLeaveBalance(Hashtable ht);
        int CheckLeaveApplicationValidations(Hashtable ht);
        int CheckforDuplicateLeaves(Hashtable ht);
        int InsertUpdateEmployeeImage(Hashtable ht);
        int spUserInsertLeaveRequest(Hashtable ht);
        #region Employee Detail Update Request Form
        DataTable Get_All_EmployeeDetail_Requests(Hashtable ht);
        DataTable Get_EmployeeDetail_Requests_For_Approval(Hashtable ht);
        int Insert_EmployeeDetail_Request(Hashtable ht);
        DataTable Get_EmployeeDetail_Request_Info_4_Email(Hashtable ht);
        DataTable Get_EmployeeDetail_Request_ByID(Hashtable ht);
        string Update_EmployeeDetail_Request(Hashtable ht);
        DataTable Get_EmployeeDetail_Request_Remarks_List(Hashtable ht);
        int Get_EmployeeDetail_RequestsApprovers_Count(Hashtable ht);
        string Approve_EmployeeDetail_Request(Hashtable ht);
        DataTable Get_EmployeeDetail_RequestSubmitter_Info_4_Email(Hashtable ht);
        string Reject_EmployeeDetail_Request(Hashtable ht);
        DataTable Get_EmployeeDetail_Request_Statuses(Hashtable ht);
        #endregion
        DataTable GetCountries();
        DataTable GetCities(Hashtable ht);
        DataTable GetLeaveApplicationApprovalData(Hashtable ht);
        DataTable GetLeaveDetails(Hashtable ht);
        DataTable GetAllLeaveRequests(Hashtable ht);
        DataTable GetLeaveRequestByID(Hashtable ht);
        string UpdateLeaveRequest(Hashtable ht);
        DataTable GetLeaveRequestInfo4Email(Hashtable ht);
        DataTable GetLeaveRequestReplacementInfo4Email(Hashtable ht);
        DataTable GetLeaveRequestRemarksList(Hashtable ht);
        string ApproveLeaveRequest(Hashtable ht);
        DataTable GetLeaveRequestSubmitterInfo4Email(Hashtable ht);
        string RejectLeaveRequest(Hashtable ht);
        string User_Resume_Leave_Request(Hashtable ht);
        DataTable User_Get_ResumeLeave_Trx_ByID(Hashtable ht);

        SqlDataReader GetReimbursementType();
        SqlDataReader GetCurrencyCode();
        SqlDataReader GetActivityTypes();
        int InsertReimbursementRequest(Hashtable ht);
        void InsertReimbursmentRequestDetail(Hashtable ht);
        DataTable GetReimbursementRequestInfo4Email(Hashtable ht);
        DataTable GetReimbursementList(Hashtable ht);
        DataTable GetReimbursementListForApproval(Hashtable ht);
        DataTable GetReimbursementRequestDetail(Hashtable ht);
        string EditSaveReimbursementRequest(Hashtable ht);
        DataTable GetReimbursementRequestByID(Hashtable ht);
        DataTable GetReimbursementRequestRemarksList(Hashtable ht);
        int ApproveReimbursement(Hashtable ht);
        string ApproveReimbursementRequest(Hashtable ht);
        DataTable GetReimbursementRequestSubmitterInfo4Email(Hashtable ht);
        string RejectReimbursementRequest(Hashtable ht);


        DataTable Get_All_LateandAbsence_Requests(Hashtable ht);
        DataTable Get_LateandAbsence_Requests_For_Approval(Hashtable ht);
        int Insert_LateandAbsence_Request(Hashtable ht);
        DataTable Get_LateandAbsence_Request_Info_4_Email(Hashtable ht);
        DataTable Get_LateandAbsence_Request_Statuses(Hashtable ht);
        DataTable Get_LateandAbsence_Request_ByID(Hashtable ht);
        string EditInsert_LateandAbsence_Request(Hashtable ht);
        DataTable Get_LateandAbsence_Request_Remarks_List(Hashtable ht);
        int Get_LateAndAbsence_RequestsApprovers_Count(Hashtable ht);
        string Approve_LateandAbsence_Request(Hashtable ht);
        DataTable Get_LateandAbsence_RequestSubmitter_Info_4_Email(Hashtable ht);
        string Reject_LateandAbsence_Request(Hashtable ht);
        DataTable GetAllownceTypes();
        int SaveAllowanceRequest(Hashtable ht);
        DataTable Get_Allowance_Request_Info_4_Email(Hashtable ht);
        DataTable Get_All_Allowance_Requests(Hashtable ht);
        DataTable Get_Allowance_Requests_For_Approval(Hashtable ht);
        DataTable GetAllowanceRequestEditData(Hashtable ht);
        string Update_Allowance_Request(Hashtable ht);
        DataTable Get_Allowance_Request_Remarks_List(Hashtable ht);
        DataTable Get_Allowance_Request_ByID(Hashtable ht);
        int Get_AllowanceRequest_RequestsApprovers_Count(Hashtable ht);
        string Approve_Allowance_Request(Hashtable ht);
        DataTable Get_Allowance_RequestSubmitter_Info_4_Email(Hashtable ht);
        string Reject_Allowance_Request(Hashtable ht);

        DataTable Get_AdvanceSalary_Requests_For_Approval(Hashtable ht);
        DataTable Get_All_AdvanceSalary_Requests(Hashtable ht);
        int Insert_AdvanceSalary_Request(Hashtable ht);
        DataTable Get_AdvanceSalary_Request_Info_4_Email(Hashtable ht);
        DataTable GetAdvanceSalaryEditData(Hashtable ht);
        string Update_AdvanceSalary_Request(Hashtable ht);
        DataTable Get_AdvanceSalary_Request_ByID(Hashtable ht);
        DataTable Get_AdvanceSalary_Request_Remarks_List(Hashtable ht);
        int Get_Advances_RequestsApprovers_Count(Hashtable ht);
        string Approve_AdvanceSalary_Request(Hashtable ht);
        string Reject_AdvanceSalary_Request(Hashtable ht);

        DataTable GetLoanTypes();
        DataTable GetInstallments(Hashtable ht);
        DataTable Get_All_Loan_Requests(Hashtable ht);
        DataTable Get_Loan_Requests_For_Approval(Hashtable ht);
        int Insert_Loan_Request(Hashtable ht);
        void Insert_Loan_Request_Detail(Hashtable ht);
        DataTable Get_Loan_Request_Info_4_Email(Hashtable ht);
        DataTable Get_Loan_Request_ByID(Hashtable ht);
        string Update_Loan_Request(Hashtable ht);
        DataTable Get_ApprovalLoan_Request_ByID(Hashtable ht);
        DataTable Get_Loan_Request_Remarks_List(Hashtable ht);
        int Get_LoanRequest_RequestsApprovers_Count(Hashtable ht);
        string Approve_Loan_Request(Hashtable ht);
        string Reject_Loan_Request(Hashtable ht);


        DataTable GetTicketType();
        int Insert_Ticket_Request(Hashtable ht);
        DataTable Get_Ticket_Request_Info_4_Email(Hashtable ht);
        DataTable Get_All_Ticket_Requests(Hashtable ht);
        DataTable Get_Ticket_Requests_For_Approval(Hashtable ht);
        DataTable Get_Ticket_Request_ByID(Hashtable ht);
        string Update_Ticket_Request(Hashtable ht);
        DataTable Get_Ticket_Request_Remarks_List(Hashtable ht);
        int Get_TicketRequest_RequestsApprovers_Count(Hashtable ht);
        string Approve_Ticket_Request(Hashtable ht);
        DataTable Get_TicketRequest_RequestSubmitter_Info_4_Email(Hashtable ht);
        string Reject_Ticket_Request(Hashtable ht);
        DataTable Get_Ticket_Request_Statuses(Hashtable ht);


        DataTable GetMiscellaneousTypes();
        DataTable GetPreferredLanguage();
        DataTable Get_All_Miscellaneous_Requests(Hashtable ht);
        DataTable Get_Miscellaneous_Requests_For_Approval(Hashtable ht);
        int Insert_Miscellaneous_Request(Hashtable ht);
        DataTable Get_Miscellaneous_Request_Info_4_Email(Hashtable ht);
        DataTable Get_Miscellaneous_Request_ByID(Hashtable ht);
        string Update_Miscellaneous_Request(Hashtable ht);
        DataTable Get_Miscellaneous_Request_Remarks_List(Hashtable ht);
        int Get_MiscRequest_RequestsApprovers_Count(Hashtable ht);
        string Approve_Miscellaneous_Request(Hashtable ht);
        DataTable Get_MiscellaneousRequest_RequestSubmitter_Info_4_Email(Hashtable ht);
        string Reject_Miscellaneous_Request(Hashtable ht);
        DataTable Get_Miscellaneous_Request_Statuses(Hashtable ht);
        DataTable Get_Asset_Requests_For_Approval(Hashtable ht);

        SqlDataReader GetAssetType();
        DataTable Get_All_Asset_Requests(Hashtable ht);
        DataTable Get_Asset_Request_Statuses(Hashtable ht);
        int Insert_Asset_Request(Hashtable ht);
        void Insert_AssetRequest_Detail(Hashtable ht);
        void Submit_Asset_Request(Hashtable ht);
        DataTable Get_Asset_Request_Info_4_Email(Hashtable ht);
        DataTable Get_Asset_Request_ByID(Hashtable ht);
        DataTable Get_Asset_Detail_By_AssetID(Hashtable ht);
        string Update_Asset_Request(Hashtable ht);
        void DeleteAssetDetails(string Id);
        int Get_Assets_RequestsApprovers_Count(Hashtable ht);
        string Approve_Asset_Request(Hashtable ht);
        DataTable Get_Asset_RequestSubmitter_Info_4_Email(Hashtable ht);
        string Reject_Asset_Request(Hashtable ht);


        SqlDataReader GetISHR(string qry);
        DataTable EmployeeAttendDetails(Hashtable ht);


        DataTable GetStaffExpenseList(Hashtable ht);

    }
}