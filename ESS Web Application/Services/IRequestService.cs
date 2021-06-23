using ESS_Web_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public interface IRequestService
    {
        List<DropDownBindViewModel> GetLeaveAppUsers(string UserID, string FormTypeID, string CompanyId);
        EmployeeDeatilViewModel GetEmployeeDeatils(string UserID);
        List<DropDownBindViewModel> GetLeaveTypes(string UserID);
        List<DropDownBindViewModel> GetReplacementEmployee(string CompanyId);
        LeavesModel FillCalculatedFields(string EmployeeId, string LeaveTypeId, string StartDate, string EndDate, string LeaveType);
        string SaveLeaveRequest(string AtchGuid,string CompanyId, string EmployeeID, string LeaveType, string Remarks, string Airticket, string ReplacementId, string StartDate, string EndDate, string ResumetoWork, string LeaveTypeName, string UserId, string Noofdays, string Leavebalance, string TravelTo, string TravelFrom, string DateofTravel, string DateofReturn, string Accomodation);
        LeaveApplicationViewModel GetLeaveApproval(string Id);
        List<LeaveApplicationViewModel> GetLeaveDetails(string Id);
        int SaveImageToDB(byte[] ImageByteArray, string UserID);
        List<DropDownBindViewModel> GetCountries();
        List<DropDownBindViewModel> GetCities(string CountryId);
        LeaveApplicationListViewModel GetLeaveApplicationData(string EmpId, string UserID, string CompanyId);
        LeaveApplicationViewModel GetEmployeeLeavesEditData(string Reqid);
        string SaveEditLeaveRequest(string Reqid, string EmployeeID, string LeaveType, string Remarks, string ReplacementId, string StartDate, string EndDate, string UserId,string Companyid, string Noofdays, string Leavebalance);
        string SaveLeaveApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId);
        string SaveResumptions(string Id, string Date,string Remarks);


        List<DropDownBindViewModel> GetReimbursementType();
        List<DropDownBindViewModel> GetCurrencyCode();
        List<DropDownBindViewModel> GetActivityTypes();
        string Reimbursement(string AtchGuid, string Type, List<ReimbursementSaveViewModel> RModle, string UserId, string CompanyId);
        ReimbursementListViewModel GetReimbursementList(string UserId, string CompanyId);
        List<ReimbursementEditViewModel> GetEditDatabind(string Reqid, string UserId);
        string EditSaveReimbursement(string Type, List<ReimbursementSaveViewModel> RModle, string UserId, string Id, bool Recall);
        List<ReimbursementEditViewModel> GetDetails(string Reqid, string UserId);
        string GetApproval(string Id);
        string SaveApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks,string CompanyId);

        LateandAbsenceListViewModel GetLateandAbsenceJustificationData(string EmpId, string UserID, string CompanyId);
        string SaveLateandLeaveRequest(string AtchGuid, int saveStatus, string Company, string EmployeeID, string Category, string Date, string PunchIn, string PunchOut, string SubCategory, string Remarks, string UserId);
        List<LateandAbsenceViewModel> GetLateandLeaveDetails(string Id);
        LateandAbsenceViewModel GetLateandAbsenceEditData(string Reqid);
        string saveLateandAnsenceEdit(string Reqid, int saveStatus, string Category,
            string Remarks, string PunchIn, string PunchOut, string SubCategory, string Date);
        string GetLateandAbsenceApproval(string Id);
        string SaveLeaveandAbsenceApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks);

        List<DropDownBindViewModel> GetAllownceTypes();
        string SaveAllowanceRequest(string AtchGuid, int saveStatus, string Date, string Amount, string Type, string Remarks, string CopanyId, string UserId);
        AllowanceRequestListViewModel GetAllowanceRequestData(string UserID, string CompanyId);
        AllowanceRequestViewModel GetAllowanceRequestEditData(string Reqid);
        string saveAllowanceRequestEdit(string Reqid, int saveStatus, string Amount,
           string Remarks, string Type, string Date);
        AllowanceRequestViewModel GetAllowanceRequestApproval(string Id);
        string SaveAllowanceRequestApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks,string CompanyId);


        AdvanceSalaryListViewModel GetAdvanceSalaryData(string UserID, string CompanyId);
        string SaveAdvanceSalary(string CompanyId, string EmployeeID, string Remarks, string UserId, string Payperiod, string Amount);
        AdvanceSalaryViewModel GetAdvanceSalaryEditData(string Reqid);
        string SaveEditAdvanceSalary(string Reqid, string EmployeeID, string Remarks, string Amount, string UserId, int saveStatus);
        AdvanceSalaryViewModel GetAdvanceSalaryApproval(string Id);
        string SaveAdvanceSalaryApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks,string CompanyId);

        List<DropDownBindViewModel> GetLoanTypes();
        List<DropDownBindViewModel> GetInstallments(string LoanType);
        LoanRequestListViewModel GetLoanData(string UserID, string CompanyId);
        string SaveLoanApplication(string Emp, string Type, string Amount, string Installments, List<LoanSaveViewModel> RModle, string UserId, string CompanyId);
        LoanSaveViewModel GetLoanEditDatabind(string Reqid, string UserId);
        string EditSaveLoan(int saveStatus, string Id, string User, string Type, string Amount, string Installment, string Date, string Remarks, string UserId);
        LoanRequestViewModel GetLoanApproval(string Id);
        string SaveLoanApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks,string CompanyId);


        List<DropDownBindViewModel> GetTicketTypes();
        string SaveTicket(string AtchGuid, string TicketBalance, string Emp, string Type, string Remarks, string FromCountry, string FromCity, string ToCountry, string ToCity, string TravelDate, string ReturnDate, string UserId, string CompanyId);
        TicketRequestListViewModel GetTicketData(string UserID, string CompanyId);
        TicketRequestViewModel GetTicketEditDatabind(string Reqid, string UserId);
        string EditSaveTicket(int saveStatus, string Id, string Emp, string Type, string FromCity, string ToCity, string TravelDate, string ReturnDate, string TicketBalance, string Remarks, string UserId);
        TicketRequestViewModel GetTicketApproval(string Id);
        string SaveTicketApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks, string CompanyId);
        List<TicketRequestViewModel> GetTicketDetails(string Id);

        List<DropDownBindViewModel> GetMiscellaneousTypes();
        List<DropDownBindViewModel> GetPreferredLanguage();
        MiscellaneousRequestListViewModel GetMiscellaneousData(string UserID, string CompanyId);
        string SaveMiscellaneous(string AtchGuid, string MiscellaneousType, string PreferredType, string Opening, string Salary, string Department, string Report, string Address, string Remarks, string UserId, string CompanyId);
        MiscellaneousRequestViewModel GetMiscellaneousEditData(string Reqid, string UserId);
        string SaveEditMiscellaneous(int saveStatus, string Id, string MiscellaneousType, string PreferredType, string Address, string Remarks, string UserId);
        MiscellaneousRequestViewModel GetMiscellaneousApproval(string Id);
        string SaveMiscellaneousApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks,string CompanyId);
        List<MiscellaneousRequestViewModel> GetMiscellaneousDetails(string Id);

        List<DropDownBindViewModel> GetAssetType();
        AssetListViewModel GetAssetData(string UserID, string CompanyId);
        List<AssetViewModel> GetAssetDetails(string Id);
        string SaveAssetRequest(string EmpFor, string RequestDate, List<AssetViewModel> List, string Remarks, string UserId, string CompanyId);
        AssetEditViewModel GetAssetEditDatabind(string Reqid);
        string SaveEditAsset(int saveStatus, string Id, string EmpFor, string RequestDate, List<AssetViewModel> List, string Remarks, string UserId);
        AssetViewModel GetAssetApproval(string Id);
        string SaveAssetApprove_Reject(string Type, string Id, string StatusId, string UserId, string Remarks,string CompanyId);


        List<EmployeeAttendanceViewModel> GetEmployeeAttendance(string StartDate, string EndDate, string Emp, string CompanyId, string UserId);

        List<StaffExpenseViewModel> GetStaffExpenseList(string UserId, string CompanyId);
    }
}