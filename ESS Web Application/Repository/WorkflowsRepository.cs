using ESS_Web_Application.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Repository
{
    public class WorkflowsRepository : IWorkflowsRepository
    {
        #region WorkFlow

        public DataTable GetWorkFlowsDropDown()
        {
            return DBContext.GetDataSet("sp_Admin_Get_WorkFlowMaster_4_DDL", null).Tables[0];
        }
        public DataTable WorkFlow_GridBinding()
        {
            return DBContext.GetDataSet("sp_Admin_Get_All_WorkFlowMaster").Tables[0];
        }
        public DataTable WorkflowUserMapping_NeedDataSource()
        {
            return DBContext.GetDataSet("sp_Admin_Get_All_User_Workflow_Mappings", null/*htSearchParams*/).Tables[0];
        }

        public DataTable FormTypesDDropDown()
        {
            return DBContext.GetDataSet("sp_Admin_Get_FormType_4_DDL").Tables[0];
        }
        public DataTable HRDropDown()
        {
            return DBContext.GetDataSet("sp_Admin_Get_HR_DDL").Tables[0];
        }
        public string InsertUpdateWorkflow(string operation, Hashtable ht)
        {
            string DBMessage = "";
            if (operation == "Insert")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Insert_WorkFlowMaster", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            else if (operation == "Update")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Update_WorkFlowMaster", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            return DBMessage;
        }
        public void DeleteWorkFlow(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_WorkFlowMaster", ht);
        }
        public DataTable GetWorkFlowSubList(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_Admin_Get_All_Workflows", ht).Tables[0];
        }
        public DataTable GetAllWorkFlows()
        {
            return DBContext.GetDataSet("sp_Admin_Get_WorkFlowMaster_4_DDL").Tables[0];
        }
        public DataTable GetAllUsers()
        {
            return DBContext.GetDataSet("sp_Admin_Get_Users_4_DDL").Tables[0];
        }
        public string SubWorkFlowInsertUpdate(string operation, Hashtable ht)
        {
            string DBMessage = "";
            if (operation == "Insert")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Insert_WorkFlow", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            else if (operation == "Update")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Update_WorkFlow", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            return DBMessage;
        }
        public void DeleteSubWorkFlow(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_WorkFlow", ht);
        }
        public DataTable GetWorkFlowMapping(Hashtable hs)
        {
            var AllUsers = DBContext.GetDataSet("sp_Admin_Get_All_User_Workflow_Mappings", hs);
            DataTable firstTable = AllUsers.Tables[0];
            return firstTable;
        }
        public DataTable GetUserName_EmpId()
        {
            return DBContext.GetDataSet("sp_Admin_Get_Users_4_DDL", null).Tables[0];
        }
        public DataTable GetWorkFlowsMappingDropDown()
        {
            return DBContext.GetDataSet("sp_Admin_Get_WorkFlowMaster_4_DDL", null).Tables[0];
        }
        public string WorkFlowMappingInsert(Hashtable ht)
        {
            return DBContext.ExecuteNonQuery("sp_Admin_Insert_User_Workflow_Mapping", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
        }
        public void DeleteWorkFlowMapping(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_User_WorkFlow_Mapping", ht);
        }
        #endregion

        #region FormType
        public DataTable FormType_GridBinding()
        {
            return DBContext.GetDataSet("sp_Admin_Get_All_Forms").Tables[0];
        }
        public string InsertUpdateFormType(string operation, Hashtable ht)
        {
            string DBMessage = "";
            if (operation == "Insert")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Insert_Form", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            else if (operation == "Update")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Update_Form", ht, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            return DBMessage;
        }
        public void DeleteFormType(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_Form", ht);
        }

        #endregion
    }
}