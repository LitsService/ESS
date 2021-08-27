using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.ViewModels
{
    public class AssetDropdownBindingViewModel
    {
        public List<DropDownBindViewModel> Type { get; set; }

    }
    public class AssetListViewModel
    {
        public AssetListViewModel()
        {
            History = new List<AssetViewModel>();
            Approval = new List<AssetViewModel>();
        }
        public bool IsApproval { get; set; }
        public List<AssetViewModel> History { get; set; }
        public List<AssetViewModel> Approval { get; set; }
    }
    public class AssetViewModel
    {
        public string ReqStatus { get; set; }
        public string RefNo { get; set; }
        public bool IsEditViisible { get; set; }
        public bool isCompletedVisible { get; set; }
        public bool isRecallVisible { get; set; }
        public bool isInProcessVisible { get; set; }
        public bool isSubmitVisible { get; set; }
        public string RequestDate { get; set; }
        public string SubmittedBy { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string Approver { get; set; }
        public string ApprovedBy { get; set; }
        public string AssetType { get; set; }
        public string EmpFor { get; set; }
        public string RequestId { get; set; }
        public string CompanyName { get;  set; }
    }
    public class AssetEditViewModel
    {
        public string EmpFor { get; set; }
        public string EmpBy { get; set; }
        public string RequestDate { get; set; }
        public string Remarks { get; set; }
        public List<DropDownBindViewModel> dropdowns { get; set; }
        public List<AssetEditList> Selected { get; set; }

    }
    public class AssetEditList
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }



    public class EmployeeAttendanceViewModel
    {
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string DESIGNATION { get; set; }
        public string Date { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string WorkingHours { get; set; }
    }
}