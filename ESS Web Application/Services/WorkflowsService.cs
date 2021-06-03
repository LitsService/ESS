using ESS_Web_Application.Repository;
using ESS_Web_Application.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESS_Web_Application.Services
{
    public class WorkflowsService : IWorkflowsService
    {
        IWorkflowsRepository _workflowsrepo = new WorkflowsRepository();

        #region WorkFlows
        public List<DropDownBindViewModel> GetWorkFlowsDropDown()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Wrokflows = _workflowsrepo.GetWorkFlowsDropDown();
            DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
            {
                Text = "ALL",
                Value = "all",
            };
            DD.Add(roleviewmodel);
            foreach (DataRow item in Wrokflows.Rows)
            {
                roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["WorkflowMasterName"].ToString(),
                    Value = item["WorkflowMasterID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }

        public List<WorkFlowViewModel> WorkFlow_GridBinding()
        {
            var dt = _workflowsrepo.WorkFlow_GridBinding();
            List<WorkFlowViewModel> list = new List<WorkFlowViewModel>();
            foreach (DataRow item in dt.Rows)
            {
                WorkFlowViewModel wf = new WorkFlowViewModel()
                {
                    ID = item["ID"].ToString(),
                    Name = item["Name"].ToString(),
                    Description = item["Description"].ToString(),
                    FormTypeId = item["FormTypeId"].ToString(),
                    FormType = item["FormType"].ToString(),
                    IsActive = bool.Parse(item["IsActive"].ToString())
                };
                list.Add(wf);
            }
            return list;
        }
        public List<DropDownBindViewModel> GeFormTypeDropDown()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Wrokflows = _workflowsrepo.FormTypesDDropDown();
            DropDownBindViewModel roleviewmodel = new DropDownBindViewModel();
            foreach (DataRow item in Wrokflows.Rows)
            {
                roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["FormType"].ToString(),
                    Value = item["FormTypeID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }
        public string InsertUpdateWorkflow(string Operation, string FormType, string Name, string ID,
             string Description, string IsActive)
        {
            string result = "";
            Hashtable newValues = new Hashtable();

            if (!string.IsNullOrEmpty(ID))
            {
                newValues["@ID"] = int.Parse(ID);
            }

            newValues["@Name"] = Name;
            newValues["@Description"] = Description;
            newValues["@FormTypeID"] = FormType;
            newValues["@IsActive"] = IsActive == "on" ? true : false;
            newValues["@DBMessage"] = "";

            try
            {
                string DBMessage = "";
                DBMessage = _workflowsrepo.InsertUpdateWorkflow(Operation, newValues);
                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    result = DBMessage;
                }
            }
            catch (Exception ex)
            {
                result = "Unable to update/insert Workflow Master entry. Reason: " + ex.Message;
            }
            return result;
        }
        public void DeleteWorkFlow(string ID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@ID", int.Parse(ID));
            _workflowsrepo.DeleteWorkFlow(ht);
        }
        public List<WorkFlowSubListViewModel> GetWorkFlowSubList(int ID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@WorkflowMasterID", ID);
            DataTable dt = _workflowsrepo.GetWorkFlowSubList(ht);
            List<WorkFlowSubListViewModel> WFSL = new List<WorkFlowSubListViewModel>();
            foreach (DataRow item in dt.Rows)
            {
                WorkFlowSubListViewModel workflow = new WorkFlowSubListViewModel()
                {
                    ID = ID.ToString(),
                    ApproverName = item["UserFullName"].ToString(),
                    ApproverLevel = item["UserLevel"].ToString(),
                    ApproverGraceDays = item["ApprovalGraceDays"].ToString(),
                    ProxyApproverName = item["ProxyUserFullName"].ToString(),
                    ApproverId = item["USERID"].ToString(),
                    ProxyApproverId = item["ProxyUserID"].ToString(),
                    WorkFlowSubId = item["WorkflowID"].ToString()
                };
                WFSL.Add(workflow);
            }
            return WFSL;
        }
        public List<DropDownBindViewModel> GetAllWorkFlows()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Wrokflows = _workflowsrepo.GetAllWorkFlows();
            foreach (DataRow item in Wrokflows.Rows)
            {
                DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["WorkflowMasterName"].ToString(),
                    Value = item["WorkflowMasterID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }
        public List<DropDownBindViewModel> GetAllUsers()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Wrokflows = _workflowsrepo.GetAllUsers();
            foreach (DataRow item in Wrokflows.Rows)
            {
                DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["FullDesc"].ToString(),
                    Value = item["UserID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }
        public string SubWorkFlowInsertUpdate(string subworkflow, string proxyapprover, string approver, string WorkFlow, string days, string level)
        {
            string result = "";
            string operation = "Insert";
            Hashtable newValues = new Hashtable();
            if (!string.IsNullOrEmpty(subworkflow))
            {
                newValues["@ID"] = int.Parse(subworkflow);
                operation = "Update";
            }
            newValues["@WorkFlowMasterID"] = int.Parse(WorkFlow);
            newValues["@UserID"] = int.Parse(approver);
            newValues["@UserLevel"] = int.Parse(level);
            newValues["@ApprovalGraceDays"] = int.Parse(days);
            newValues["@ProxyUserID"] = int.Parse(proxyapprover);
            newValues["@DBMessage"] = "";
            try
            {
                string DBMessage = "";
                DBMessage = _workflowsrepo.SubWorkFlowInsertUpdate(operation, newValues);

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    result = DBMessage;
                }
            }
            catch (Exception ex)
            {
                result = "Unable to update/insert Workflow. Reason: " + ex.Message;
            }
            return result;
        }
        public void DeleteSubWorkFlow(string ID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@ID", int.Parse(ID));
            _workflowsrepo.DeleteSubWorkFlow(ht);
        }
        public List<WorkFlowMappingViewModel> GetWorkFlowMapping(Hashtable hs)
        {
            var AllUsers = _workflowsrepo.GetWorkFlowMapping(hs);
            List<WorkFlowMappingViewModel> UVM = new List<WorkFlowMappingViewModel>();
            foreach (DataRow item in AllUsers.Rows)
            {
                WorkFlowMappingViewModel userviewmodel = new WorkFlowMappingViewModel()
                {
                    ID = item["WorkflowMasterID"].ToString(),
                    Name = item["UserFullName"].ToString(),
                    WorkFlow = item["WorkflowMaster"].ToString(),
                    UserID = item["UserID"].ToString()
                };
                UVM.Add(userviewmodel);
            }
            return UVM;
        }
        public List<DropDownBindViewModel> GetUserName_EmpId()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Wrokflows = _workflowsrepo.GetUserName_EmpId();
            foreach (DataRow item in Wrokflows.Rows)
            {
                DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["FirstName"].ToString() + " " + item["LastName"].ToString(),
                    Value = item["UserID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }
        public List<DropDownBindViewModel> GetWorkFlowsMappingDropDown()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Wrokflows = _workflowsrepo.GetWorkFlowsMappingDropDown();
            foreach (DataRow item in Wrokflows.Rows)
            {
                DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["WorkflowMasterName"].ToString(),
                    Value = item["WorkflowMasterID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }
        public void WorkFlowMappingInsert(string Name, string Workflow)
        {
            Hashtable newValues = new Hashtable();

            newValues["@UserID"] = int.Parse(Name);
            newValues["@WorkflowMasterID"] = int.Parse(Workflow);
            newValues["@DBMessage"] = "";

            try
            {
                string DBMessage = "";

                DBMessage = _workflowsrepo.WorkFlowMappingInsert(newValues);

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    //lblError2.Text = DBMessage;
                }
            }
            catch (Exception ex)
            {
                //lblError2.Text = "Unable to insert user/workflow mapping. Reason: " + ex.Message;
            }

        }
        public void DeleteWorkFlowMapping(string UserID, string WorkflowMasterID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", int.Parse(UserID));
            ht.Add("@WorkflowMasterID", int.Parse(WorkflowMasterID));
            _workflowsrepo.DeleteWorkFlowMapping(ht);
        }
        #endregion

        #region FormTypes
        public List<FormTypeViewModel> FormType_GridBinding()
        {
            var dt = _workflowsrepo.FormType_GridBinding();
            List<FormTypeViewModel> list = new List<FormTypeViewModel>();
            foreach (DataRow item in dt.Rows)
            {
                FormTypeViewModel wf = new FormTypeViewModel()
                {
                    ID = item["ID"].ToString(),
                    Name = item["Name"].ToString(),
                    Description = item["Description"].ToString(),
                    IsActive = bool.Parse(item["IsActive"].ToString())
                };
                list.Add(wf);
            }
            return list;
        }
        public string InsertUpdateFormType(string Operation, string Name, string ID,
             string Description, string IsActive)
        {
            string result = "";
            Hashtable newValues = new Hashtable();

            if (!string.IsNullOrEmpty(ID))
            {
                newValues["@ID"] = int.Parse(ID);
            }

            newValues["@Name"] = Name;
            newValues["@Description"] = Description;
            newValues["@IsActive"] = IsActive == "on" ? true : false;
            newValues["@DBMessage"] = "";

            try
            {
                string DBMessage = "";
                DBMessage = _workflowsrepo.InsertUpdateFormType(Operation, newValues);
                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    result = DBMessage;
                }
            }
            catch (Exception ex)
            {
                result = "Unable to update/insert Workflow Master entry. Reason: " + ex.Message;
            }
            return result;
        }
        public void DeleteFormType(string ID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@ID", int.Parse(ID));
            _workflowsrepo.DeleteFormType(ht);
        }

        public List<SearchFormType> GetFormName()
        {

            var dt = _workflowsrepo.FormType_GridBinding();
            List<SearchFormType> list = new List<SearchFormType>();
            foreach (DataRow item in dt.Rows)
            {
                SearchFormType wf = new SearchFormType()
                {
                    Id = item["Name"].ToString(),
                    Name = item["Name"].ToString(),
                };
                list.Add(wf);
            }
            return list;
        }
        #endregion
    }
}