using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Collections;
using ESS_Web_Application.Services;
using ESS_Web_Application.Entity;
using System.Data.SqlClient;

namespace ESS_Web_Application.Repository
{
    public class ManagedUsersRespository : IManagedUsersRespository
    {
        DBContext dbcontext = new DBContext();

        #region User
        public DataTable GetAllUsers(Hashtable hs)
        {
            var AllUsers = DBContext.GetDataSet("sp_Admin_Get_All_Users", hs);
            DataTable firstTable = AllUsers.Tables[0];
            return firstTable;
        }
        public DataTable GetAllUserCompany(Hashtable hs)
        {
            var AllUsers = DBContext.GetDataSet("sp_Admin_Get_User_Company", hs);
            DataTable firstTable = AllUsers.Tables[0];
            return firstTable;
        }

        public DataTable GetAllRoles(Hashtable htSearchParams)
        {
            var AllUsers = DBContext.GetDataSet("sp_Admin_get_User_Roles_4_DDL", htSearchParams);
            DataTable firstTable = AllUsers.Tables[0];
            return firstTable;
        }
        public DataTable GetEmployeeforReg()
        {
            var AllEmployee = DBContext.GetDataSet("sp_Admin_Get_Employees_4_Reg_4_DDL").Tables[0];
            DataTable firstTable = AllEmployee;
            return firstTable;
        }
        public DataTable GetEmployeeforUpdate()
        {
            var AllEmployee = DBContext.GetDataSet("sp_Admin_Get_All_Employees_4_DDL").Tables[0];
            DataTable firstTable = AllEmployee;
            return firstTable;
        }
        public string InsertUpdateUser(string operation, Hashtable newValues)
        {
            string DBMessage = "";
            if (operation == "Insert")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Insert_User", newValues, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            else if (operation == "Update")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Update_User", newValues, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            return DBMessage;
        }
        public void DeleteUserRoleMapping(Hashtable htTemp)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_User_UserRole_Mappings", htTemp);

        }
        public void DeleteUserCompanyMapping(Hashtable htTemp)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_User_UserCompany_Mapping", htTemp);

        }
        public void InsertUserRole(Hashtable htTemp)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Insert_User_UserRole_Mapping", htTemp);

        }
        public void InsertUserCompany(Hashtable htTemp)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Insert_User_UserCompany_Mapping", htTemp);

        }
        public void DeleteUser(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_User", ht);
        }
        public void UpdateUserPassword(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Update_Password", ht);
        }
        public SqlDataReader GetCompany()
        {
            return DBContext.ExecuteReaderWithCommand("Select * from Company");
        }
        #endregion

        #region Role
        public DataSet GetRoles(Hashtable ht)
        {
            var Roles = DBContext.GetDataSet("sp_Admin_Get_All_Roles", ht);
            return Roles;
        }
        public string InsertUpdateRole(string operation, Hashtable newValues)
        {
            string DBMessage = "";
            if (operation == "Insert")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Insert_Role", newValues, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            else if (operation == "Update")
            {
                DBMessage = DBContext.ExecuteNonQuery("sp_Admin_Update_Role", newValues, "@DBMessage", System.Data.SqlDbType.NVarChar, 255) as string;
            }
            return DBMessage;
        }
        public void DeleteRole(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Delete_Role", ht);
        }
        #endregion

        #region ACL
        public DataTable GetAllUserActions()
        {
            return DBContext.GetDataSet("sp_Admin_Get_All_User_Actions").Tables[0];
        }
        public DataTable GetAllUserRoles(Hashtable hs)
        {
            return DBContext.GetDataSet("sp_Admin_Get_All_Roles", hs).Tables[0];
        }
        public DataTable GetACL(Hashtable ht)
        {
            return DBContext.GetDataSet("sp_Admin_Get_ACL", ht).Tables[0];
        }
        public void UpdateACL(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Update_ACL", ht);
        }
        public void InsertACL(Hashtable ht)
        {
            DBContext.ExecuteNonQuery("sp_Admin_Insert_ACL", ht);
        }
        #endregion
    }
}