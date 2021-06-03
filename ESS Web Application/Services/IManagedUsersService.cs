using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using ESS_Web_Application.ViewModels;
namespace ESS_Web_Application.Services
{
    public interface IManagedUsersService
    {
        List<UserViewModel> GetAllUsers(Hashtable hs);
        List<string> GetUserCompany(Hashtable hs);
        List<DropDownBindViewModel> GetRoles(Hashtable htSearchParams);
        List<DropDownBindViewModel> GetEmployeeForReg();
        List<DropDownBindViewModel> GetEmployeeForUpdate();
        string InsertUpdate(string operation, string EmployeeID, string PasswordHash, string ID, string Username,
            string IsAdmin, string IsActive, string[] UserRoleId, string ImgBody, Guid CompanyId, List<string> CompaniesSelectedList);
        void DeleteUser(string IDs);
        void UpdateUserPassword(int UserID, string Password);
        List<RolesViewModel> GetAllRoles(Hashtable ht);
        string InsertUpdateRole(string operation, string ID, string Name, string IsActive, Guid CompanyId);
        void DeleteRole(string IDs);
        List<UserActionACLMappingHelperClass> GetAcl(Hashtable hs);
        void ACLInsertUpdate(string ActionId, string RoleId, string Allow, string Access);
    }
}