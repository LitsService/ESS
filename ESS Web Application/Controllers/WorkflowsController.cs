using ESS_Web_Application.Helper;
using ESS_Web_Application.Infrastructure;
using ESS_Web_Application.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESS_Web_Application.Controllers
{
    [AuthorizeActionFilter]
    public class WorkflowsController : Controller
    {
        IWorkflowsService _workflowsService = new WorkflowsService();
        // GET: Workflows

        #region WorkFlows
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageWorkFlow()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "ManageWorkflows"))
            {
                ViewBag.WorkflowsDropDown = _workflowsService.GetWorkFlowsDropDown();
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetWorkFlows()
        {
            var data = _workflowsService.WorkFlow_GridBinding();
            return Json(data);
        }
        public JsonResult GetFormType()
        {
            var data = _workflowsService.GeFormTypeDropDown();
            return Json(data);
        }
        public JsonResult WorkflowInsertUpdate(string Operation, string FormType, string Name, string ID,
           string Description, string IsActive)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                Operation = "Update";
            }
            string result = _workflowsService.InsertUpdateWorkflow(Operation, FormType, Name, ID,
             Description, IsActive);
            return Json(result);
        }

        public ActionResult DeleteWorkFlow(string ID)
        {
            _workflowsService.DeleteWorkFlow(ID);
            return Json("");
        }
        public JsonResult GetWorkFlowSubList(string Id)
        {
            int ID = int.Parse(Id);
            var data = _workflowsService.GetWorkFlowSubList(ID);
            return Json(data);
        }
        public JsonResult GetAllWorkFlows()
        {
            var result = _workflowsService.GetAllWorkFlows();
            return Json(result);
        }
        public JsonResult GetAllUsers()
        {
            var result = _workflowsService.GetAllUsers();
            return Json(result);
        }
        public JsonResult SubWorkFlowInsertUpdate(string subworkflow, string proxyapprover, string approver, string WorkFlow, string days, string level)
        {
            string result = _workflowsService.SubWorkFlowInsertUpdate(subworkflow, proxyapprover, approver, WorkFlow, days, level);
            return Json(result);
        }
        public ActionResult DeleteSubWorkFlow(string ID)
        {
            _workflowsService.DeleteSubWorkFlow(ID);
            return Json("");
        }
        public JsonResult GetWorkFlowMapping(string Keyword, string WorkflowMasterID)
        {
            Hashtable htSearchParams = new Hashtable();
            if ((!string.IsNullOrEmpty(WorkflowMasterID) && WorkflowMasterID != "all") || !string.IsNullOrEmpty(Keyword))
            {
                htSearchParams.Add("@WorkflowMasterID", int.Parse(WorkflowMasterID));
                if (string.IsNullOrEmpty(Keyword))
                {
                    htSearchParams.Add("@Keyword", "all");
                }
                else
                {
                    htSearchParams.Add("@Keyword", Keyword.Trim());
                }
            }

            var Users = _workflowsService.GetWorkFlowMapping(htSearchParams);
            return Json(Users);
        }
        public JsonResult GetUserName_EmpId()
        {
            var data = _workflowsService.GetUserName_EmpId();
            return Json(data);
        }
        public JsonResult GetWorkFlowsDropDown()
        {
            var data = _workflowsService.GetWorkFlowsMappingDropDown();
            return Json(data);
        }
        public JsonResult WorkFlowMappingInsert(string Name, string Workflow)
        {
            _workflowsService.WorkFlowMappingInsert(Name, Workflow);
            return Json("");
        }
        public ActionResult DeleteWorkFlowMapping(string UserID, string WorkflowMasterID)
        {
            _workflowsService.DeleteWorkFlowMapping(UserID, WorkflowMasterID);
            return Json("");
        }
        #endregion

        #region FormTypes
        public ActionResult FormTypes()
        {
            if (string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                return RedirectToAction("Login", "Account");
            }
            if (clsCommon.ValidatePageSecurity(Session["UserName"].ToString(), "ManageForms"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
        public JsonResult GetAllForms()
        {
            var data = _workflowsService.FormType_GridBinding();
            return Json(data);
        }

        public JsonResult FormTypeInsertUpdate(string Operation, string Name, string ID,
           string Description, string IsActive)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                Operation = "Update";
            }
            string result = _workflowsService.InsertUpdateFormType(Operation, Name, ID,
             Description, IsActive);
            return Json(result);
        }

        public ActionResult DeleteFormType(string ID)
        {
            _workflowsService.DeleteFormType(ID);
            return Json("");
        }
        #endregion
    }
}