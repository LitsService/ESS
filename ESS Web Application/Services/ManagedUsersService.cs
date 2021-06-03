using System.Collections.Generic;
using System.Data;
using System.Collections;
using ESS_Web_Application.Repository;
using ESS_Web_Application.ViewModels;
using ESS_Web_Application.Helper;
using System;

namespace ESS_Web_Application.Services
{
    public class ManagedUsersService : IManagedUsersService
    {
        IManagedUsersRespository _manageuserrepository = new ManagedUsersRespository();
        //private readonly ManagedUsersRespository _manageuserrepository;
        //public ManagedUsersService(ManagedUsersRespository mangedUser)
        //{
        //    _manageuserrepository = mangedUser;
        //}

        #region Users
        public List<UserViewModel> GetAllUsers(Hashtable hs)
        {
            var AllUsers = _manageuserrepository.GetAllUsers(hs);
            List<UserViewModel> UVM = new List<UserViewModel>();
            foreach (DataRow item in AllUsers.Rows)
            {
                UserViewModel userviewmodel = new UserViewModel()
                {
                    ID = item["ID"].ToString(),
                    UserName = item["Username"].ToString(),
                    EmployeeId = item["EmployeeID"].ToString(),
                    FirstName = item["FRSTNAME"].ToString(),
                    LastName = item["LASTNAME"].ToString(),
                    Roles = item["role"].ToString(),
                    IsActive = bool.Parse(item["IsActive"].ToString()),
                    IsAdmin = bool.Parse(item["IsAdmin"].ToString()),
                    CompanyId = item["CompanyId"].ToString(),
                    CompanyName = item["CompanyName"].ToString()
                };

                UVM.Add(userviewmodel);
            }
            return UVM;
        }

        public List<DropDownBindViewModel> GetEmployeeForReg()
        {

            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Roles = _manageuserrepository.GetEmployeeforReg();
            DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
            {
                Text = "EmployeeID" + " | " + "First Name" + " | " + "Last Name",
                Value = "",
            };
            DD.Add(roleviewmodel);
            foreach (DataRow item in Roles.Rows)
            {
                roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["EmployeeID"].ToString() + " | " + item["FirstName"].ToString() + " | " + item["LastName"].ToString(),
                    Value = item["EmployeeID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }

        public List<DropDownBindViewModel> GetEmployeeForUpdate()
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Roles = _manageuserrepository.GetEmployeeforUpdate();
            DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
            {
                Text = "EmployeeID" + " | " + "First Name" + " | " + "Last Name",
                Value = "",
            };
            DD.Add(roleviewmodel);
            foreach (DataRow item in Roles.Rows)
            {
                roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["EmployeeID"].ToString() + " | " + item["FirstName"].ToString() + " | " + item["LastName"].ToString(),
                    Value = item["EmployeeID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;
        }
        public List<string> GetUserCompany(Hashtable hs)
        {
            var AllUsers = _manageuserrepository.GetAllUserCompany(hs);
            List<string> UVM = new List<string>();
            foreach (DataRow item in AllUsers.Rows)
            {
                string CompanyGuid = item["CompanyGuid"].ToString();

                UVM.Add(CompanyGuid);
            }
            return UVM;
        }
        public List<DropDownBindViewModel> GetRoles(Hashtable htSearchParams)
        {
            List<DropDownBindViewModel> DD = new List<DropDownBindViewModel>();
            var Roles = _manageuserrepository.GetAllRoles(htSearchParams);
            foreach (DataRow item in Roles.Rows)
            {
                DropDownBindViewModel roleviewmodel = new DropDownBindViewModel()
                {
                    Text = item["Name"].ToString(),
                    Value = item["UserRoleID"].ToString(),
                };
                DD.Add(roleviewmodel);
            }
            return DD;

        }

        public string InsertUpdate(string operation, string EmployeeID, string PasswordHash,
            string ID, string Username, string IsAdmin, string IsActive, string[] UserRoleId, string ImgBody, Guid CompanyId, List<string> CompaniesSelectedList)
        {
            string lblError = "";
            Hashtable newValues = new Hashtable();

            if (operation == "Insert")
            {
                // MultiView mv = (MultiView)editedItem.FindControl("MultiView1");
                newValues["@PasswordHash"] = clsEncryption.EncryptData(PasswordHash);
                newValues["@EmployeeID"] = EmployeeID.Trim();
                newValues["@CompanyId"] = CompanyId;

            }
            else if (operation == "Update")
            {
                newValues["@ID"] = int.Parse(ID);
                newValues["@EmployeeID"] = EmployeeID.Trim();
            }

            newValues["@Username"] = Username;
            newValues["@IsAdmin"] = bool.Parse(IsAdmin);
            newValues["@IsActive"] = bool.Parse(IsActive);
            newValues["@DBMessage"] = "";

            //RadAsyncUpload radAsyncUpload = editedItem.FindControl("AsyncUpload1") as RadAsyncUpload;
            byte[] fileData = new byte[1];
            //if (radAsyncUpload.UploadedFiles != null && radAsyncUpload.UploadedFiles.Count > 0)
            //{
            //    UploadedFile file = radAsyncUpload.UploadedFiles[0];
            //    fileData = new byte[file.InputStream.Length];
            //    file.InputStream.Read(fileData, 0, (int)file.InputStream.Length);
            //}
            newValues["@ImgBody"] = fileData;

            try
            {
                #region insert Update Operation
                string DBMessage = "";

                DBMessage = _manageuserrepository.InsertUpdateUser(operation, newValues);

                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblError = DBMessage;
                }
                else
                {
                    int IDs = 0;
                    int.TryParse(DBMessage, out IDs);

                    if (IDs > 0)
                    {
                        //CheckBoxList chkLst = editedItem.FindControl("chkBoxLstUserRoles") as CheckBoxList;

                        if (UserRoleId != null)
                        {
                            Hashtable htTemp = new Hashtable();
                            htTemp.Add("@UserID", IDs);
                            _manageuserrepository.DeleteUserRoleMapping(htTemp);
                            htTemp.Clear();

                            foreach (var li in UserRoleId)
                            {
                                htTemp = new Hashtable();
                                htTemp.Add("@UserID", IDs);
                                htTemp.Add("@UserRoleID", int.Parse(li));
                                _manageuserrepository.InsertUserRole(htTemp);
                            }
                        }
                        if (CompaniesSelectedList != null)
                        {
                            Hashtable htTemp = new Hashtable();
                            htTemp.Add("@UserID", IDs);
                            _manageuserrepository.DeleteUserCompanyMapping(htTemp);
                            htTemp.Clear();

                            foreach (var li in CompaniesSelectedList)
                            {
                                htTemp = new Hashtable();
                                htTemp.Add("@UserID", IDs);
                                htTemp.Add("@UserCompanyID", li);
                                _manageuserrepository.InsertUserCompany(htTemp);
                            }
                        }
                    }
                }
                #endregion
                return DBMessage;
            }
            catch (Exception ex)
            {
                lblError = "Unable to update/insert User. Reason: " + ex.Message;
                return lblError;
            }
        }

        public void DeleteUser(string IDs)
        {
            int ID = int.Parse(IDs);

            if (ID > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("@ID", ID);
                _manageuserrepository.DeleteUser(ht);
            }
        }
        public void UpdateUserPassword(int UserID, string Password)
        {
            Hashtable ht = new Hashtable();
            ht.Add("@UserID", UserID);
            ht.Add("@PasswordHash", clsEncryption.EncryptData(Password));
            _manageuserrepository.UpdateUserPassword(ht);
        }
        #endregion

        #region Roles
        public List<RolesViewModel> GetAllRoles(Hashtable ht)
        {
            var AllRoles = _manageuserrepository.GetRoles(ht).Tables[0];
            List<RolesViewModel> RVM = new List<RolesViewModel>();
            foreach (DataRow item in AllRoles.Rows)
            {
                RolesViewModel rolesviewmodel = new RolesViewModel()
                {
                    ID = item["ID"].ToString(),
                    Name = item["Name"].ToString(),
                    IsActive = bool.Parse(item["IsActive"].ToString())
                };
                RVM.Add(rolesviewmodel);
            }
            return RVM;
        }
        public string InsertUpdateRole(string operation, string ID, string Name, string IsActive, Guid CompanyId)
        {
            string lblError = "";
            Hashtable newValues = new Hashtable();

            if (operation == "Update")
            {
                newValues["@ID"] = int.Parse(ID);
            }

            newValues["@Name"] = Name;
            newValues["@IsActive"] = bool.Parse(IsActive);
            newValues["@DBMessage"] = "";
            newValues["@CompanyId"] = CompanyId;
            try
            {
                string DBMessage = "";
                DBMessage = _manageuserrepository.InsertUpdateRole(operation, newValues);
                if ((null != DBMessage) && DBMessage.Contains("ERROR:"))
                {
                    lblError = DBMessage;
                }
                return DBMessage;
            }
            catch (Exception ex)
            {
                lblError = "Unable to update/insert User. Reason: " + ex.Message;
                return lblError;
            }
        }
        public void DeleteRole(string IDs)
        {
            int ID = int.Parse(IDs);
            if (ID > 0)
            {
                Hashtable ht = new Hashtable();
                ht.Add("@ID", ID);
                _manageuserrepository.DeleteRole(ht);
            }
        }
        #endregion

        #region ACL
        public List<UserActionACLMappingHelperClass> GetAcl(Hashtable hs)
        {
            DataTable dtUserActions = _manageuserrepository.GetAllUserActions();
            DataTable dtUserRoles = _manageuserrepository.GetAllUserRoles(hs);
            List<UserActionACLMappingHelperClass> dt = new List<UserActionACLMappingHelperClass>();

            for (int i = 0; i <= dtUserActions.Rows.Count - 1; i++)
            {
                UserActionACLMappingHelperClass map1 = new UserActionACLMappingHelperClass();
                map1.UserActionID = int.Parse(dtUserActions.Rows[i]["UserActionID"].ToString());
                map1.UserActionName = dtUserActions.Rows[i]["Name"].ToString();
                map1.Allow = new Dictionary<int, bool>();
                map1.DataAccessID = new Dictionary<int, int>();

                for (int j = 0; j <= dtUserRoles.Rows.Count - 1; j++)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("@UserRoleID", int.Parse(dtUserRoles.Rows[j]["ID"].ToString()));
                    ht.Add("@UserActionID", int.Parse(dtUserActions.Rows[i]["UserActionID"].ToString()));
                    DataTable dtACLs = _manageuserrepository.GetACL(ht);

                    if (dtACLs.Rows.Count > 0)
                    {
                        map1.Allow.Add(int.Parse(dtUserRoles.Rows[j]["ID"].ToString()), bool.Parse(dtACLs.Rows[0]["Allow"].ToString()));
                        map1.DataAccessID.Add(int.Parse(dtUserRoles.Rows[j]["ID"].ToString()), int.Parse(dtACLs.Rows[0]["DataAccessID"].ToString()));
                    }
                    else
                    {
                        map1.Allow.Add(int.Parse(dtUserRoles.Rows[j]["ID"].ToString()), false);
                        map1.DataAccessID.Add(int.Parse(dtUserRoles.Rows[j]["ID"].ToString()), 0);
                    }
                }
                dt.Add(map1);
            }
            return dt;
        }
        public void ACLInsertUpdate(string ActionId, string RoleId, string Allow, string Access)
        {
            try
            {
                bool allow = bool.Parse(Allow);
                int userActionID = int.Parse(ActionId);
                int DataAccessTypeID = int.Parse(Access);
                Hashtable ht = new Hashtable();
                ht.Add("@UserRoleID", int.Parse(RoleId));
                ht.Add("@UserActionID", userActionID);
                DataTable dtACLs = _manageuserrepository.GetACL(ht);
                if (dtACLs.Rows.Count > 0)
                {
                    ht.Clear();
                    ht.Add("@ACLID", dtACLs.Rows[0]["ACLID"]);
                    ht.Add("@AllowUserAction", allow);
                    ht.Add("@DataAccessTypeID", DataAccessTypeID);
                    _manageuserrepository.UpdateACL(ht);
                }
                else
                {
                    ht.Clear();
                    ht.Add("@UserRoleID", int.Parse(RoleId));
                    ht.Add("@UserActionID", userActionID);
                    ht.Add("@AllowUserAction", allow);
                    ht.Add("@DataAccessTypeID", DataAccessTypeID);
                    _manageuserrepository.InsertACL(ht);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

    }
}