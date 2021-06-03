using ESS_Web_Application.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Services
{
    public interface IWorkflowsService
    {
        List<DropDownBindViewModel> GetWorkFlowsDropDown();
        List<WorkFlowViewModel> WorkFlow_GridBinding();
        List<DropDownBindViewModel> GeFormTypeDropDown();
        string InsertUpdateWorkflow(string Operation, string FormType, string Name, string ID,
             string Description, string IsActive);
        void DeleteWorkFlow(string ID);
        List<WorkFlowSubListViewModel> GetWorkFlowSubList(int ID);
        List<DropDownBindViewModel> GetAllWorkFlows();
        List<DropDownBindViewModel> GetAllUsers();
        string SubWorkFlowInsertUpdate(string subworkflow, string proxyapprover, string approver, string WorkFlow, string days, string level);
        void DeleteSubWorkFlow(string ID);
        List<WorkFlowMappingViewModel> GetWorkFlowMapping(Hashtable hs);
        List<DropDownBindViewModel> GetWorkFlowsMappingDropDown();
        List<DropDownBindViewModel> GetUserName_EmpId();
        void WorkFlowMappingInsert(string Name, string Workflow);
        void DeleteWorkFlowMapping(string UserID, string WorkflowMasterID);

        List<FormTypeViewModel> FormType_GridBinding();
        string InsertUpdateFormType(string Operation, string Name, string ID,
              string Description, string IsActive);
        void DeleteFormType(string ID);
        List<SearchFormType> GetFormName();
    }
}