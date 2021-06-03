using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

namespace ESS_Web_Application.Repository
{
    public interface IManagedUsersRespository
    {
        DataTable GetAllUsers(Hashtable hs);
        DataTable GetAllUserCompany(Hashtable hs);
        DataTable GetAllRoles(Hashtable htSearchParams);
        DataTable GetEmployeeforReg();
        DataTable GetEmployeeforUpdate();
        string InsertUpdateUser(string operation, Hashtable newValues);
        void DeleteUserRoleMapping(Hashtable htTemp);
        void DeleteUserCompanyMapping(Hashtable htTemp);
        void InsertUserRole(Hashtable htTemp);
        void InsertUserCompany(Hashtable htTemp);
        void DeleteUser(Hashtable ht);
        void UpdateUserPassword(Hashtable ht);
        SqlDataReader GetCompany();

        DataSet GetRoles(Hashtable ht);
        string InsertUpdateRole(string operation, Hashtable newValues);
        void DeleteRole(Hashtable ht);

        DataTable GetAllUserActions();
        DataTable GetAllUserRoles(Hashtable hs);
        DataTable GetACL(Hashtable ht);
        void UpdateACL(Hashtable ht);
        void InsertACL(Hashtable ht);
    }
}