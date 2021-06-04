using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public interface IWorkflowsRepository
    {
        DataTable GetWorkFlowsDropDown();
        DataTable WorkFlow_GridBinding();
        DataTable FormTypesDDropDown();
        DataTable HRDropDown();
        string InsertUpdateWorkflow(string operation, Hashtable ht);
        void DeleteWorkFlow(Hashtable ID);
        DataTable GetWorkFlowSubList(Hashtable ht);
        DataTable GetAllWorkFlows();
        DataTable GetAllUsers();
        string SubWorkFlowInsertUpdate(string operation, Hashtable ht);
        void DeleteSubWorkFlow(Hashtable ht);
        DataTable GetWorkFlowMapping(Hashtable hs);
        DataTable GetUserName_EmpId();
        DataTable GetWorkFlowsMappingDropDown();
        string WorkFlowMappingInsert(Hashtable ht);
        void DeleteWorkFlowMapping(Hashtable ht);

        DataTable FormType_GridBinding();
        string InsertUpdateFormType(string operation, Hashtable ht);
        void DeleteFormType(Hashtable ht);
    }
}